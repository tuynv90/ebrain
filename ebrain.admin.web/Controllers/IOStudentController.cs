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
using Microsoft.Extensions.Options;

namespace Ebrain.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Security("8AA6E971-1C3D-4835-B154-D662CE12AE99", "8AA6E971-1C3D-4835-B154-D662CE12AE12")]
    public class IOStudentController : BaseController
    {
        private IUnitOfWork _unitOfWork;
        readonly ILogger _logger;
        public readonly IOptions<SmtpConfig> _serviceSmtpConfig;

        public IOStudentController(IUnitOfWork unitOfWork, ILogger<IOStudentController> logger, IOptions<SmtpConfig> serviceSmtpConfig) : base(unitOfWork, logger)
        {
            _serviceSmtpConfig = serviceSmtpConfig;
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

        [HttpGet("search")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> Search(string filter, string value, string fromDate, string toDate, int page, int size)
        {
            var unit = this._unitOfWork.IOStocks;
            var list = await SearchMain(filter, value, fromDate, toDate, page, size);
            return Json(new
            {
                Total = unit.Total,
                List = list
            });
        }

        [HttpGet("reportsearch")]
        [Produces(typeof(UserViewModel))]
        public async Task<IActionResult> ReportPurchaseOrderList(string filter, string value, string fromDate, string toDate, int allData, int page, int size)
        {

            var list = await SearchMain(filter, value, fromDate, toDate, 0, 0);
            var item = new ChartViewModel();
            if (list != null && list.Count > 0)
            {
                //sort
                var temps = list.GroupBy(p => new { p.BranchName, p.CreateDate_MMYY }).Select(p => new
                {
                    BranchName = p.Key.BranchName,
                    CreateDate = p.Key.CreateDate_MMYY,
                    TotalPrice = p.Sum(c => c.TotalPrice)

                }).ToList();
                item.ChartModels.AddRange(temps.GroupBy(p => p.BranchName).Select(p => new ChartModel
                {
                    Label = p.Key,
                    Data = p.Select(c => c.TotalPrice).ToArray()
                }));

                item.ChartLabels = temps.Select(p => p.CreateDate).ToArray();
            }

            return this.Ok(item);
        }

        public async Task<List<IOStockViewModel>> SearchMain(string filter, string value, string fromDate, string toDate, int page, int size)
        {
            var unit = this._unitOfWork.IOStocks;
            var results = unit.GetIOStockList(
                            fromDate.BuildDateTimeFromSEFormat(),
                            toDate.BuildLastDateTimeFromSEFormat(),
                            value, (int)EnumIOType.IORegisCourse,
                            this._unitOfWork.Branches.GetAllBranchOfUserString(userId),
                            page, size);
            var list = new List<IOStockViewModel>();
            foreach (var item in results)
            {
                list.Add(new IOStockViewModel
                {
                    ID = item.IOStockId,
                    Code = item.IONumber,
                    FullName = item.FullName,
                    StudentName = item.StudentName,
                    CreateDate = item.CreatedDate,
                    TotalPrice = item.TotalPrice,
                    IOTypeId = (int)item.IOTypeId,
                    Note = item.Note,
                    StudentId = item.StudentId,
                    BranchName = item.BranchName
                });
            }
            return list;
        }


        [HttpGet("getdefault")]
        [Produces(typeof(UserViewModel))]
        public Task<IActionResult> GetDefault(Guid? index, int isClone = 0)
        {
            return GetDefaultMain(index, (int)EnumIOType.IORegisCourse, isClone);
        }

        [HttpGet("getdefaultinput")]
        [Produces(typeof(UserViewModel))]
        public Task<IActionResult> GetDefaultInput(Guid? index, int isClone = 0)
        {
            return GetDefaultMain(index, (int)EnumIOType.IOInput, isClone);
        }

        private async Task<IActionResult> GetDefaultMain(Guid? index, int ioTypeId, int clone = 0)
        {
            if (index.IsNullOrDefault())
            {
                var itemNew = GenerateDefault(ioTypeId);

                return Ok(itemNew);
            }

            return await GetIOStock(index, string.Empty, clone);
        }

        private IOStockViewModel GenerateDefault(int ioTypeId)
        {
            var ioNumber = this._unitOfWork.ConfigNumberOfCodes.GenerateCode(ioTypeId, userId.ToString());

            var itemNew = new IOStockViewModel
            {
                Code = ioNumber,
                CreateDate = DateTime.Now,
                CreateBy = userId,
                IOTypeId = ioTypeId,
                IODetails = new IOStockDetailViewModel[0]
            };
            return itemNew;
        }

        private async Task<IActionResult> GetIOStock(Guid? id, string html, int clone = 0)
        {
            var ret = await this._unitOfWork.IOStocks.FindById(id);
            if (ret != null)
            {
                var iods = await this._unitOfWork.IOStocks.GetDetailByIOId(ret.IOStockId);
                var iodNews = new List<IOStockDetailViewModel>();
                foreach (var item in iods)
                {
                    var itemMate = await this._unitOfWork.Materials.Get(item.MaterialId);
                    var itemGrpMate = await this._unitOfWork.GrpMaterials.FindById(itemMate.GrpMaterialId);
                    var itemType = await this._unitOfWork.TypeMaterials.FindById(itemGrpMate.TypeMaterialId);
                    iodNews.Add(new IOStockDetailViewModel
                    {
                        ID = item.IOStockDetailId,
                        MaterialId = item.MaterialId,
                        MaterialCode = itemMate.MaterialCode,
                        MaterialName = itemMate.MaterialName,
                        GrpMaterial = itemGrpMate != null ? itemGrpMate.GrpMaterialName : string.Empty,
                        TypeMaterial = itemType != null ? itemType.TypeMaterialName : string.Empty,
                        Quantity = item.InputQuantity,
                        SellPrice = item.PriceBeforeVAT,
                        TotalPrice = item.TotalPrice,
                        DisCountMoney = item.DisCountMoney,
                        PurchaseOrderId = item.PurchaseOrderId,
                        PurchaseOrderDetailId = item.PurchaseOrderDetailId
                    });

                }

                // case clone data
                if (clone > 0)
                {
                    var generate = this.GenerateDefault((int)ret.IOTypeId);
                    ret.IOStockId = Guid.Empty;
                    ret.IONumber = generate.Code;
                    ret.CreatedBy = userId;
                    ret.CreatedDate = generate.CreateDate;
                }
                return Ok(new IOStockViewModel
                {
                    ID = ret.IOStockId == Guid.Empty ? (Guid?)null : ret.IOStockId,
                    Code = ret.IONumber,
                    StudentId = ret.StudentId,
                    Note = ret.Note,
                    CreateBy = ret.CreatedBy,
                    CreateDate = ret.CreatedDate,
                    IOTypeId = ret.IOTypeId,
                    PurchaseOrderId = ret.PurchaseOrderId,
                    PurchaseOrderCode = ret.PurchaseOrderCode,
                    IODetails = iodNews.ToArray(),
                    Html = html
                });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] IOStockViewModel value)
        {
            if (ModelState.IsValid)
            {
                var ret = await SaveIOStock(value);

                return await GetIOStock(ret.IOStockId, ret.Html);
            }

            return BadRequest(ModelState);
        }

        private async Task<IOStock> SaveIOStock(IOStockViewModel value)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(value.PurchaseOrderCode))
                {
                    //check purchaseOrder
                    var results = this._unitOfWork.PurchaseOrders.GetPurchaseOrderListDetail(
                                  ObjectHelper.GetDateMin, ObjectHelper.GetDateNow,
                                   value.PurchaseOrderId.ToString(),
                                   this._unitOfWork.Branches.GetAllBranchOfUserString(userId),
                                   value.ID.HasValue ? value.ID.Value.ToString() : string.Empty,
                                   0, 0
                               );
                    var iods = value.IODetails;
                    foreach (var item in results)
                    {
                        var itemExist = iods.FirstOrDefault(p => p.PurchaseOrderDetailId == item.PurchaseOrderDetailId);
                        if (itemExist != null)
                        {
                            if ((item.PurchaseQuantity - item.IOQuantity) >= itemExist.Quantity) continue;
                            else throw new Exception("Đã hết đơn đặt hàng");
                        }
                        else
                        {
                            throw new Exception("Not exist purchase order");
                        }
                    }


                }
                var ioId = Guid.NewGuid();
                var io = new IOStock
                {
                    IOStockId = ioId,
                    IONumber = value.Code,
                    SupplierId = value.SupplierId,
                    CreatedBy = userId,
                    CreatedDate = value.CreateDate,
                    UpdatedBy = userId,
                    UpdatedDate = DateTime.Now,
                    IOTypeId = value.IOTypeId,
                    BranchId = ioId,
                    StudentId = value.StudentId,
                    Note = value.Note,
                    PurchaseOrderCode = value.PurchaseOrderCode,
                    PurchaseOrderId = value.PurchaseOrderId
                };

                var ioDetails = new List<IOStockDetail>();
                foreach(var p in value.IODetails)
                {
                    var ioDetailId = p.ID == null || !value.ID.HasValue ? Guid.NewGuid() : p.ID.Value;
                    var itemNew = new IOStockDetail
                    {
                        IOStockDetailId = ioDetailId,
                        IOStockId = ioId,
                        MaterialId = p.MaterialId,
                        MaterialCode = p.MaterialCode,
                        PriceBeforeVAT = p.SellPrice,
                        PriceAfterVAT = p.SellPrice,
                        VAT = 0,
                        InputQuantity = p.Quantity,
                        DisCountMoney = p.DisCountMoney,
                        TotalPrice = p.SellPrice * p.Quantity - (p.DisCountMoney ?? 0),
                        TotalPriceBeforeVAT = p.SellPrice * p.Quantity - (p.DisCountMoney ?? 0),
                        CreatedBy = userId,
                        CreatedDate = value.CreateDate,
                        UpdatedBy = userId,
                        UpdatedDate = DateTime.Now,
                        PurchaseOrderDetailId = p.PurchaseOrderDetailId,
                        PurchaseOrderId = p.PurchaseOrderId,
                        IOPros = p.Pros.Select(c => new IOStockDetailPro
                        {
                            IOStockDetailId = ioDetailId,
                            IOStockId = ioId,
                            PromotionId = c.PromotionId,
                            PromotionDetailId = c.PromotionDetailId,
                            PercentDiscount = c.PercentDiscount,
                            MoneyDiscount = c.MoneyDiscount,
                            IOStockDetailProId = Guid.NewGuid(),
                            CreatedBy = userId,
                            CreatedDate = value.CreateDate,
                            UpdatedBy = userId,
                            UpdatedDate = DateTime.Now,
                        }).ToArray()
                    };
                    ioDetails.Add(itemNew);
                }
                
                io.TotalPrice = ioDetails.Sum(p => p.TotalPrice) - ioDetails.Sum(p => p.DisCountMoney);
                io.TotalPriceBeforeVAT = ioDetails.Sum(p => p.TotalPriceBeforeVAT) - ioDetails.Sum(p => p.DisCountMoney);



                var ret = await this._unitOfWork.IOStocks.Save(io, ioDetails.ToArray(), value.ID);
                value.UpdatedByName = ret.UpdatedByName;

                // send email
                var bodyHtml = EmailTemplates.SendEmail_IOStock(_serviceSmtpConfig, value);
                ret.Html = EmailTemplates.GetTempalteEmail(_serviceSmtpConfig, value);
                return ret;
            }
            return null;
        }

        [HttpPost("deletemaster")]
        public async Task<IActionResult> DeleteMaster([FromBody] Guid? id)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.IOStocks.DeleteMaster(id);
                return await GetDefault(null);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelMaster([FromBody] Guid? id)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.IOStocks.CancelMaster(id);
                return await GetDefault(null);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] String id)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.Units.Delete(id);
                return Ok(ret);
            }

            return BadRequest(ModelState);
        }
    }
}
