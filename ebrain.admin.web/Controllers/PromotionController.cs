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
using ebrain.admin.bc.Core.Interfaces;
using ebrain.admin.bc.Utilities;
using ebrain.admin.bc.Report;

namespace Ebrain.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PromotionController : BaseController
    {
        private IUnitOfWork _unitOfWork;
        readonly ILogger _logger;

        public PromotionController(IUnitOfWork unitOfWork, ILogger<IOStockController> logger) : base(unitOfWork, logger)
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


        [HttpGet("getpromotionlistdetail")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetPromotionListDetail(string value, string fromDate, string toDate, int page, int size)
        {
            var frDate = fromDate.BuildDateTimeFromSEFormat();
            var tDate = toDate.BuildLastDateTimeFromSEFormat();

            var unit = this._unitOfWork.Promotions;
            var results = unit.GetPromotionListDetail(
                frDate,
                tDate,
                value,
                true,
                this._unitOfWork.Branches.GetAllBranchOfUserString(userId), page, size);
            return Json(new
            {
                Total = unit.Total,
                List = results
            });
        }

        [HttpGet("getpromotionlist")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetPromotionList(string value, string fromDate, string toDate, int page, int size)
        {
            var frDate = fromDate.BuildDateTimeFromSEFormat();
            var tDate = toDate.BuildLastDateTimeFromSEFormat();

            var unit = this._unitOfWork.Promotions;
            var results = unit.GetPromotionList(
                frDate,
                tDate,
                value,
                true,
                this._unitOfWork.Branches.GetAllBranchOfUserString(userId), page, size);
            return Json(new
            {
                Total = unit.Total,
                List = results
            });
        }

        [HttpGet("getpromotionlistreportiodetail")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetPromotionListReportIODetail(Guid? promotionId, int page, int size)
        {
         
            var unit = this._unitOfWork.Promotions;
            var results = unit.GetPromotionListReportIODetail(promotionId, page, size);
            return Json(new
            {
                Total = unit.Total,
                List = results
            });
        }

        [HttpGet("getpromotionlistreport")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetPromotionListReport(string value, string fromDate, string toDate, int page, int size)
        {
            var frDate = fromDate.BuildDateTimeFromSEFormat();
            var tDate = toDate.BuildLastDateTimeFromSEFormat();

            var unit = this._unitOfWork.Promotions;
            var results = unit.GetPromotionListReport(
                frDate,
                tDate,
                value,
                true,
                this._unitOfWork.Branches.GetAllBranchOfUserString(userId), page, size);
            return Json(new
            {
                Total = unit.Total,
                List = results
            });
        }

        [HttpGet("getdefault")]
        [Produces(typeof(UserViewModel))]
        public async Task<IActionResult> GetDefault(Guid? index, int isClone = 0)
        {
            if (index.IsNullOrDefault())
            {
                var itemNew = GenerateDefault();
                return Ok(itemNew);
            }

            var item = await GetPromotion(index, string.Empty, isClone);
            return Ok(item);
        }

        private async Task<Promotions> GetPromotion(Guid? index, string html, int clone = 0)
        {
            var item = await this._unitOfWork.Promotions.Get(index);
            if (item != null)
            {
                if (clone > 0)
                {
                    var generate = this.GenerateDefault();
                    item.PromotionId = Guid.Empty;
                    item.PromotionCode = generate.PromotionCode;
                    item.CreatedBy = userId;
                    item.CreatedDate = generate.CreatedDate;
                    foreach (var detail in item.Details)
                    {
                        detail.PromotionId = Guid.Empty;
                        detail.PromotionDetailId = Guid.Empty;
                    }
                }
            }
            return item;
        }

        private Promotions GenerateDefault()
        {
            var ioNumber = this._unitOfWork.ConfigNumberOfCodes.GenerateCodePromotions(userId.ToString());

            var itemNew = new Promotions
            {
                PromotionCode = ioNumber,
                CreatedDate = DateTime.Now,
                CreatedBy = userId,
                Details = new PromotionsDetail[0]
            };
            return itemNew;
        }

        [HttpPost("update")]
        public async Task<IActionResult> SavePromotions([FromBody]Promotions value)
        {
            if (ModelState.IsValid)
            {
                var promotionId = value.PromotionId;
                var ioId = Guid.NewGuid();
                value.CreatedBy = userId;
                value.CreatedDate = DateTime.Now;
                value.UpdatedDate = DateTime.Now;
                value.UpdatedBy = userId;
                value.IsApproved = value.IsApproved;
                value.CreatedByApproved = value.CreatedByApproved;
                value.CreatedDateApproved = value.CreatedDateApproved;
                value.PromotionId = ioId;
                foreach (var item in value.Details)
                {
                    item.PromotionId = ioId;
                    item.PromotionDetailId = Guid.NewGuid();
                    item.CreatedBy = userId;
                    item.UpdatedBy = userId;
                    item.CreatedDate = DateTime.Now;
                    item.UpdatedDate = DateTime.Now;

                }

                var ret = await this._unitOfWork.Promotions.Save(value, value.Details, promotionId);
                value.UpdatedByName = ret.UpdatedByName;

                ret = await this.GetPromotion(ret.PromotionId, string.Empty, 0);

                return Ok(ret);
            }
            return BadRequest(ModelState); ;
        }

        [HttpPost("updateapproved")]
        public async Task<IActionResult> SaveApproved([FromBody]Promotions value)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.Promotions.SaveApproved(value, value.PromotionId);
                return Ok(await this.GetPromotion(value.PromotionId, string.Empty, 0));
            }
            return Ok(null);
        }

        [HttpPost("deletemaster")]
        public async Task<IActionResult> DeleteMaster([FromBody] Guid? id)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.Promotions.Delete(id);
                return await GetDefault(null);
            }

            return BadRequest(ModelState);
        }
    }
}
