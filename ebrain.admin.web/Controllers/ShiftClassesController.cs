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
using AutoMapper;
using ebrain.admin.bc.Models;
using Microsoft.Extensions.Logging;
using Ebrain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Ebrain.Policies;
using ebrain.admin.bc.Utilities;

namespace Ebrain.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Security("D7239078-E67A-42FA-86D4-4A8C3F73D521", "8AA6E971-1C3D-4835-B154-D662CE12AE93")]
    public class ShiftClassesController : BaseController
    {
        private IUnitOfWork _unitOfWork;
        readonly ILogger _logger;


        public ShiftClassesController(IUnitOfWork unitOfWork, ILogger<ShiftClassesController> logger) : base(unitOfWork, logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("search")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> Search(string filter, string value, int page, int size)
        {
            var unit = this._unitOfWork.ShiftClasses;
            var ret = from c in await unit.Search(filter, value, this._unitOfWork.Branches.GetAllBranchOfUserString(userId), page, size)
                      select new ShiftClassViewModel
                      {
                          ID = c.ShiftClassId,
                          Code = c.ShiftClassCode,
                          Name = c.ShiftClassName,
                          Note = c.Note,
                          StartTime = c.StartTime.ToString(),
                          EndTime = c.EndTime.ToString()
                      };

            return Json(new
            {
                Total = unit.Total,
                List = ret
            });
        }

        private Guid userId
        {
            get
            {
                return Utilities.GetUserId(this.User);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] ShiftClassViewModel value)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.ShiftClasses.Save(new ShiftClass
                {
                    ShiftClassId = Guid.NewGuid(),
                    ShiftClassCode = value.Code,
                    BranchId = Guid.NewGuid(),
                    ShiftClassName = value.Name,
                    Note = value.Note,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    StartTime = value.StartTime.BuildYYYYMMDDHHSSFromSEFormat(),
                    EndTime = value.EndTime.BuildYYYYMMDDHHSSFromSEFormat(),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                }, value.ID);

                return Ok(ret);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] String id)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.ShiftClasses.Delete(id);
                return Ok(ret);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("get")]
        [Produces(typeof(UserViewModel))]
        public async Task<ShiftClassViewModel> Get(Guid? index)
        {
            var c = await this._unitOfWork.ShiftClasses.Get(index);

            return new ShiftClassViewModel
            {
                ID = c.ShiftClassId,
                Code = c.ShiftClassCode,
                Name = c.ShiftClassName,
                Note = c.Note,
                StartTime = c.StartTime.ToString(),
                EndTime = c.EndTime.ToString(),
            };
        }
    }
}
