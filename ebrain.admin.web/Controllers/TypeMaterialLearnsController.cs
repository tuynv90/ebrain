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

namespace Ebrain.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Security("8AA6E971-1C3D-4835-B154-D662CE12AE9E")]
    public class TypeMaterialLearnsController : BaseController
    {
        private IUnitOfWork _unitOfWork;
        readonly ILogger _logger;


        public TypeMaterialLearnsController(IUnitOfWork unitOfWork, ILogger<TypeMaterialLearnsController> logger) : base(unitOfWork, logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("search")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> Search(string filter, string value, int page, int size)
        {
            var unit = this._unitOfWork.TypeMaterials;
            var ret = from c in await unit.SearchLearn(filter, value, this._unitOfWork.Branches.GetAllBranchOfUserString(userId)
                      , page, size)
                      select new TypeMaterialViewModel
                      {
                          ID = c.TypeMaterialId,
                          Code = c.TypeMaterialCode,
                          Name = c.TypeMaterialName,
                          Note = c.Note,
                          IsDocument = c.IsDocument,
                          IsLearning = c.IsLearning,
                          IsTest = c.IsTest,
                      };

            return Json(new
            {
                Total = unit.Total,
                List = ret
            });
        }

        [HttpGet("getalltypelearn")]
        [Produces(typeof(UserViewModel))]
        public async Task<IEnumerable<TypeMaterialViewModel>> GetAllTypeLearn()
        {
            var ret = from c in await this._unitOfWork.TypeMaterials.GetAllTypeLearn(this._unitOfWork.Branches.GetAllBranchOfUserString(userId))
                      select new TypeMaterialViewModel
                      {
                          ID = c.TypeMaterialId,
                          Code = c.TypeMaterialCode,
                          Name = c.TypeMaterialName,
                          Note = c.Note,
                          IsDocument = c.IsDocument,
                          IsLearning = c.IsLearning,
                          IsTest = c.IsTest,
                      };
            return ret;
        }

        [HttpGet("get")]
        [Produces(typeof(UserViewModel))]
        public async Task<TypeMaterialViewModel> Get(Guid? index)
        {
            var item = await this._unitOfWork.TypeMaterials.FindById(index);

            return new TypeMaterialViewModel
            {
                ID = item.TypeMaterialId,
                Code = item.TypeMaterialCode,
                Name = item.TypeMaterialName,
                Note = item.Note,
                IsDocument = item.IsDocument,
                IsLearning = item.IsLearning,
                IsTest = item.IsTest,
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
        public async Task<IActionResult> Update([FromBody] TypeMaterialViewModel value)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.TypeMaterials.Save(new TypeMaterial
                {
                    TypeMaterialId = Guid.NewGuid(),
                    TypeMaterialCode = value.Code,
                    BranchId = Guid.NewGuid(),
                    TypeMaterialName = value.Name,
                    Note = value.Note,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsLearning = true,
                    IsDocument = value.IsDocument,
                    IsTest = value.IsTest,
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
                var ret = await this._unitOfWork.TypeMaterials.Delete(id);
                return Ok(ret);
            }

            return BadRequest(ModelState);
        }
    }
}
