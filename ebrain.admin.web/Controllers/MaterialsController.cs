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
using ebrain.admin.bc.Core.Interfaces;

namespace Ebrain.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Security("D7239078-E67A-42FA-86D6-4A8C3F73D544", "8AA6E971-1C3D-4835-B154-D662CE12AE92")]
    public class MaterialsController : BaseController
    {
        private IUnitOfWork _unitOfWork;
        readonly ILogger _logger;

        public MaterialsController(IUnitOfWork unitOfWork, ILogger<MaterialsController> logger) : base(unitOfWork, logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        private Guid userId
        {
            get
            {
                return Utilities.GetUserId(this.User);
            }
        }

        public async Task<IEnumerable<MaterialViewModel>> MappingMaterial(IEnumerable<Material> materials)
        {
            List<MaterialViewModel> list = new List<MaterialViewModel>();

            foreach (var item in materials)
            {
                var itemHead = await this._unitOfWork.Materials.FindHeadByMaterialId(item.MaterialId);
                var itemType = await this._unitOfWork.GrpMaterials.FindTypeByGrpId(item.GrpMaterialId);
                var itemGrp = await this._unitOfWork.GrpMaterials.FindById(item.GrpMaterialId);
                var itemNew = new MaterialViewModel
                {
                    ID = item.MaterialId,
                    Code = item.MaterialCode,
                    Name = item.MaterialName,
                    TypeName = itemType != null ? itemType.TypeMaterialName : string.Empty,
                    GrpName = itemGrp != null ? itemGrp.GrpMaterialName : string.Empty,
                    GrpMaterialId = item.GrpMaterialId,
                    TypeMaterialId = itemType != null ? itemType.TypeMaterialId : Guid.Empty,
                    Note = item.Note,
                    UnitId = item.MaterialId,
                    SellPrice = itemHead != null ? itemHead.SellPrice : 0,
                    BranchId = item.BranchId,
                    CalBeCourse = itemHead != null ? itemHead.CalBeCourse : string.Empty,
                    CalEnCourse = itemHead != null ? itemHead.CalBeCourse : string.Empty,
                    SpBeCourse = itemHead != null ? itemHead.CalBeCourse : string.Empty,
                    SpEnCourse = itemHead != null ? itemHead.CalBeCourse : string.Empty,
                    NumberHourse = itemHead != null ? itemHead.NumberHourse : 0,
                    MaskPassCourse = itemHead != null ? itemHead.MaskPassCourse : 0,
                };
                list.Add(itemNew);
            }

            return list;
        }

        [HttpGet("search")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> Search(string filter, string value, int page, int size, int isLearn)
        {
            var unit = this._unitOfWork.Materials;
            bool? learn = isLearn == 0 ? (bool?)null : isLearn == 1 ? true : false;
            var materials = await unit.Search(filter, value, page, size, this._unitOfWork.Branches.GetAllBranchOfUserString(userId), learn);
            var results = await MappingMaterial(materials);
            return Json(new
            {
                Total = unit.Total,
                List = results
            });
        }

        [HttpGet("csv")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> OutputCSV(string filter, string value, int page, int size)
        {
            var unit = this._unitOfWork.Materials;
            var materials = await unit.Search(filter, value, page, size, this._unitOfWork.Branches.GetAllBranchOfUserString(userId));
            var results = await MappingMaterial(materials);
            var contents = base.CSV<MaterialViewModel>(results);
            return Json(contents);
        }

        [HttpGet("getallmateriallearnlist")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetAllMaterialList(string filter, string value, int page, int size)
        {
            var unit = this._unitOfWork.Materials;
            var materials = unit.GetAllMaterialList(page, size, value, this._unitOfWork.Branches.GetAllBranchOfUserString(userId), true)
                .Select(p => new MaterialViewModel
                {
                    ID = p.MaterialId,
                    Code = p.MaterialCode,
                    Name = p.MaterialName,
                    TypeName = p.TypeMaterialName,
                    GrpName = p.GrpMaterialName,
                    Note = p.Note,
                });
            return Json(new
            {
                Total = unit.Total,
                List = materials
            });
        }

        [HttpGet("getmateriallist")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetMaterialList(string filter, string value, int isMaterial, int page, int size)
        {
            var unit = this._unitOfWork.Materials;
            bool? isLearn = isMaterial == 0 ? (bool?)null : isMaterial == 1 ? true : false;
            var materials = unit.GetAllMaterialList(page, size, value, this._unitOfWork.Branches.GetAllBranchOfUserString(userId), isLearn)
                .Select(p => new MaterialViewModel
                {
                    ID = p.MaterialId,
                    Code = p.MaterialCode,
                    Name = p.MaterialName,
                    TypeName = p.TypeMaterialName,
                    GrpName = p.GrpMaterialName,
                    Note = p.Note,
                    SellPrice = p.SellPrice
                });
            return Json(new
            {
                Total = unit.Total,
                List = materials
            });
        }

        [HttpGet("get")]
        [Produces(typeof(UserViewModel))]
        public async Task<MaterialViewModel> Get(Guid index)
        {
            var item = await this._unitOfWork.Materials.Get(index);
            var itemHead = await this._unitOfWork.Materials.FindHeadByMaterialId(item.MaterialId);
            var itemType = await this._unitOfWork.GrpMaterials.FindTypeByGrpId(item.GrpMaterialId);
            var itemGrp = await this._unitOfWork.GrpMaterials.FindById(item.GrpMaterialId);

            return new MaterialViewModel
            {
                ID = item.MaterialId,
                Code = item.MaterialCode,
                Name = item.MaterialName,
                TypeName = itemType != null ? itemType.TypeMaterialName : string.Empty,
                GrpName = itemGrp != null ? itemGrp.GrpMaterialName : string.Empty,
                GrpMaterialId = item.GrpMaterialId,
                TypeMaterialId = itemType != null ? itemType.TypeMaterialId : Guid.Empty,
                Note = item.Note,
                UnitId = item.MaterialId,
                SellPrice = itemHead != null ? itemHead.SellPrice : 0,
                Price = itemHead != null ? itemHead.Price : 0,
                WholePrice = itemHead != null ? itemHead.WholePrice : 0,
                BranchId = item.BranchId,
                CalBeCourse = itemHead != null ? itemHead.CalBeCourse : string.Empty,
                CalEnCourse = itemHead != null ? itemHead.CalBeCourse : string.Empty,
                SpBeCourse = itemHead != null ? itemHead.CalBeCourse : string.Empty,
                SpEnCourse = itemHead != null ? itemHead.CalBeCourse : string.Empty,
                NumberHourse = itemHead != null ? itemHead.NumberHourse : 0,
                MaskPassCourse = itemHead != null ? itemHead.MaskPassCourse : 0,
            };
        }

        [HttpPost("clonematerial")]
        public async Task<IActionResult> CloneMaterial([FromBody] BranchViewModel[] values)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.Materials.SaveClone(values.Select(p => new Branch
                {
                    BranchId = p.ID.HasValue ? p.ID.Value : Guid.NewGuid(),
                    IsExist = p.IsExist,
                    MaterialId = p.MaterialId

                }).ToArray(), userId);
                return Ok(ret);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] MaterialViewModel value)
        {
            if (ModelState.IsValid)
            {
                var materialId = Guid.NewGuid();
                var price = value.SellPrice.HasValue ? value.SellPrice.Value : 0;

                var ret = await this._unitOfWork.Materials.Save(new Material
                {
                    MaterialId = materialId,
                    MaterialCode = value.Code,
                    GrpMaterialId = value.GrpMaterialId,
                    MaterialName = value.Name,
                    Note = value.Note,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                }, new MaterialHead
                {
                    MaterialHeadId = Guid.NewGuid(),
                    MaterialId = materialId,
                    SellPrice = price,
                    SellPriceAfterVAT = price,
                    Price = price,
                    VAT = 0,
                    PriceAfterVAT = price,
                    WholePrice = price,
                    WholePriceAfterVAT = price,
                    BranchId = Guid.NewGuid(),
                    CalBeCourse = value.CalBeCourse,
                    CalEnCourse = value.CalEnCourse,
                    SpBeCourse = value.SpBeCourse,
                    SpEnCourse = value.SpEnCourse,
                    NumberHourse = value.NumberHourse,
                    MaskPassCourse = value.MaskPassCourse,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedBy = userId,
                    UpdatedBy = userId,
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
                var ret = await this._unitOfWork.Materials.Delete(id);
                return Ok(ret);
            }
            return BadRequest(ModelState);
        }
    }
}
