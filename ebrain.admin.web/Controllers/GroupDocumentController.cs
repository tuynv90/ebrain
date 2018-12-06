﻿// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ebrain.admin.bc;
using Ebrain.ViewModels;
using ebrain.admin.bc.Models;
using Microsoft.Extensions.Logging;
using Ebrain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace Ebrain.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Security("9C100588-C478-47C8-BE15-40523BB6BA1A")]
    public class GroupDocumentController : BaseController
    {
        private IUnitOfWork _unitOfWork;
        readonly ILogger _logger;
        readonly IHostingEnvironment _env;

        public GroupDocumentController(IUnitOfWork unitOfWork, ILogger<GroupDocumentController> logger, IHostingEnvironment env) : base(unitOfWork, logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            this._env = env;
        }

        private Guid userId
        {
            get
            {
                return Utilities.GetUserId(this.User);
            }
        }

        [HttpGet("getall")]
        [Produces(typeof(UserViewModel))]
        public async Task<IEnumerable<GrpDocumentViewModel>> GetAll()
        {
            var bus = this._unitOfWork.GroupDocuments;
            var ret = from c in await bus.GetAll(this._unitOfWork.Branches.GetAllBranchOfUserString(userId))
                      select new GrpDocumentViewModel
                      {
                          ID = c.GroupDocumentId,
                          Code = c.GroupDocumentCode,
                          Name = c.GroupDocumentName,
                          IsPrintTemplate = c.IsPrintTemplate,
                          Note = c.Note
                      };

            return ret;
        }

        [HttpGet("search")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> Search(string filter, string value, int isPrintTemplate, int page, int size)
        {
            var isPrint = isPrintTemplate == 1 ? true : (bool?)null;
            var bus = this._unitOfWork.GroupDocuments;
            var ret = from c in await bus.Search(filter, value, isPrint, page, size, this._unitOfWork.Branches.GetAllBranchOfUserString(userId))
                      select new GrpDocumentViewModel
                      {
                          ID = c.GroupDocumentId,
                          Code = c.GroupDocumentCode,
                          Name = c.GroupDocumentName,
                          IsPrintTemplate = c.IsPrintTemplate,
                          Note = c.Note
                      };

            return Json(new
            {
                Total = bus.Total,
                List = ret
            });
        }

        [HttpGet("get")]
        [Produces(typeof(UserViewModel))]
        public async Task<GrpDocumentViewModel> Get(Guid index)
        {
            var c = await this._unitOfWork.GroupDocuments.FindById(index);

            var branch = new GrpDocumentViewModel
            {
                ID = c.GroupDocumentId,
                Code = c.GroupDocumentCode,
                Name = c.GroupDocumentName,
                IsPrintTemplate = c.IsPrintTemplate,
                Note = c.Note
            };
            
            return branch;
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] GrpDocumentViewModel value)
        {
            if (ModelState.IsValid)
            {
                //
                var userId = Utilities.GetUserId(this.User);
                //
                var grp = new GroupDocument
                {
                    GroupDocumentId = Guid.NewGuid(),
                    GroupDocumentCode = value.Code,
                    GroupDocumentName = value.Name,
                    Note = value.Note,
                    IsPrintTemplate = value.IsPrintTemplate,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    
                };
                
                //commit
                var ret = await this._unitOfWork.GroupDocuments.Save(grp, value.ID);

                //return client side
                return Ok(ret);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] Guid id)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.GroupDocuments.Delete(id);

                return Ok(ret);
            }

            return BadRequest(ModelState);
        }
        
    }
}
