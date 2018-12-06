// ======================================
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

namespace Ebrain.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Security("8AA6E971-1C3D-4835-B154-D662CE12AE94")]
    public class ExamineController : BaseController
    {
        private IUnitOfWork _unitOfWork;
        readonly ILogger _logger;


        public ExamineController(IUnitOfWork unitOfWork, ILogger<ExamineController> logger) : base(unitOfWork, logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("search")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> Search(string filter, string value, int page, int size)
        {
            var userId = Utilities.GetUserId(this.User);
            var unit = this._unitOfWork.Examines;
            var ret = from c in await unit.Search(filter, value, this._unitOfWork.Branches.GetAllBranchOfUserString(userId), page, size)
                      select new ExamineViewModel
                      {
                          ID = c.ExamineId,
                          Code = c.ExamineCode,
                          Name = c.ExamineName,
                          Note = c.Note
                      };

            return Json(new
            {
                Total = unit.Total,
                List = ret
            });
        }

        [HttpGet("getexaminematerial")]
        [Produces(typeof(UserViewModel))]
        public async Task<IEnumerable<ExamineMaterialViewModel>> GetExamineMaterial(Guid? materialId)
        {
            var ret = from c in await this._unitOfWork.Examines.SearchExamineMaterial(materialId, this._unitOfWork.Branches.GetAllBranchOfUserString(userId))
                      select new ExamineMaterialViewModel
                      {
                          ExamineMaterialId = c.ExamineMaterialId,
                          BranchId = c.BranchId,
                          ExamineId = c.ExamineId,
                          MaterialId = c.MaterialId,
                          ExamineName = c.ExamineName,
                          ParentExamineId = c.ParentExamineId,
                          ExamineCode = c.ExamineCode,
                          IsExist = c.IsExist,
                          PercentMark  = c.PercentMark
                      };

            return ret;
        }

        [HttpGet("getexaminedocument")]
        [Produces(typeof(UserViewModel))]
        public async Task<IEnumerable<ExamineDocumentViewModel>> GetExamineDocumentId(Guid? materialId, Guid? examineId)
        {
            var ret = from c in await this._unitOfWork.Examines.SearchExamineDocument(examineId, materialId, 
                this._unitOfWork.Branches.GetAllBranchOfUserString(userId))
                      select new ExamineDocumentViewModel
                      {
                          ExamineDocumentId = c.ExamineDocumentlId,
                          BranchId = c.BranchId,
                          ExamineId = c.ExamineId,
                          MaterialId = c.MaterialId,
                          DocumentId = c.DocumentId,
                          DocumentName = c.DocumentName,
                          ParentExamineId = c.ParentExamineId,
                          DocumentCode = c.DocumentCode,
                          IsExist = c.IsExist,
                      };

            return ret;
        }

        [HttpGet("getall")]
        [Produces(typeof(UserViewModel))]
        public async Task<IEnumerable<ExamineViewModel>> GetAll()
        {
            var ret = from c in await this._unitOfWork.Examines.GetAllExamines(this._unitOfWork.Branches.GetAllBranchOfUserString(userId))
                      select new ExamineViewModel
                      {
                          ID = c.ExamineId,
                          Code = c.ExamineCode,
                          Name = c.ExamineName,
                          Note = c.Note
                      };

            return ret;
        }

        [HttpGet("get")]
        [Produces(typeof(UserViewModel))]
        public async Task<ExamineViewModel> Get(Guid? index)
        {
            var c = await this._unitOfWork.Examines.FindById(index);

            return new ExamineViewModel
            {
                ID = c.ExamineId,
                Code = c.ExamineCode,
                Name = c.ExamineName,
                Note = c.Note
            }; ;
        }

        private Guid userId
        {
            get
            {
                return Utilities.GetUserId(this.User);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] ExamineViewModel value)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.Examines.Save(new Examine
                {
                    ExamineId = Guid.NewGuid(),
                    ExamineCode = value.Code,
                    BranchId = Guid.NewGuid(),
                    ExamineName = value.Name,
                    Note = value.Note,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                }, value.ID);

                return Ok(ret);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("updateexaminematerial")]
        public async Task<IActionResult> UpdateExamineMaterial([FromBody] ExamineMaterialViewModel[] values)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.Examines.Save(values.Select(value => new ExamineMaterial
                {
                    ExamineMaterialId = Guid.NewGuid(),
                    BranchId = value.BranchId,
                    ExamineId = value.ExamineId,
                    MaterialId = value.MaterialId,
                    IsDeleted = !value.IsExist,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    PercentMark = value.PercentMark
                }));

                return Ok(ret);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("updateexaminedocument")]
        public async Task<IActionResult> UpdateExamineDocument([FromBody] ExamineDocumentViewModel[] values)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.Examines.Save(values.Select(value => new ExamineDocument
                {
                    ExamineDocumentId = Guid.NewGuid(),
                    BranchId = value.BranchId,
                    ExamineId = value.ExamineId,
                    DocumentId = value.DocumentId,
                    MaterialId = value.MaterialId,
                    IsDeleted = !value.IsExist,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                }));

                return Ok(ret);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] String id)
        {
            if (ModelState.IsValid)
            {
                var userId = Utilities.GetUserId(this.User);
                var ret = await this._unitOfWork.Examines.Delete(id);
                return Ok(ret);
            }

            return BadRequest(ModelState);
        }
    }
}
