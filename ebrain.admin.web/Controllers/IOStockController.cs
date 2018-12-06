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
using ebrain.admin.bc.Utilities;
using ebrain.admin.bc.Report;

namespace Ebrain.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Security("9ECBD467-7642-467B-AAE2-96484AD182A1", "9ECBD467-7642-467B-AAE2-96484AD182A2",
        "9ECBD467-7642-467B-AAE2-96484AD182A3", "9ECBD467-7642-467B-AAE2-96484AD182A4", "9ECBD467-7642-467B-AAE2-96484AD182A5",
        "9ECBD467-7642-467B-AAE2-96484AD182A5", "9ECBD467-7642-467B-AAE2-96484AD182A8", "9ECBD467-7642-467B-AAE2-96484AD182A9")]
    public class IOStockController : BaseController
    {
        private IUnitOfWork _unitOfWork;
        readonly ILogger _logger;

        public IOStockController(IUnitOfWork unitOfWork, ILogger<IOStockController> logger) : base(unitOfWork, logger)
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

        [HttpGet("getiobyiotypeid")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetIOByIOTypeId(string filter, string value, string fromDate, string toDate, int page, int size)
        {
            var results = GetIOByIOTypeIdMain(filter, value, fromDate.BuildDateTimeFromSEFormat(), toDate.BuildLastDateTimeFromSEFormat(), 0, page, size);
            return Json(new
            {
                Total = this._unitOfWork.IOStocks.Total,
                List = results
            });
        }

        [HttpGet("reportiobyiotypeid")]
        [Produces(typeof(UserViewModel))]
        public async Task<IActionResult> ReportIOByIOTypeId(string filter, string value, string fromDate, string toDate, int page, int size)
        {

            var list = GetIOByIOTypeIdMain(filter, value, fromDate.BuildDateTimeFromSEFormat(), toDate.BuildLastDateTimeFromSEFormat(), 0, 0, 0);
            var item = new ChartViewModel();
            if (list != null && list.Count() > 0)
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

        public IEnumerable<IOStockViewModel> GetIOByIOTypeIdMain(string filter, string value, DateTime fromDate, DateTime toDate, int ioTypeId, int page, int size)
        {
            // var results = await this._unitOfWork.IOStocks.Search(filter, value);
            var results = this._unitOfWork.IOStocks.GetIOStockList(
                            fromDate,
                            toDate,
                            value, ioTypeId, this._unitOfWork.Branches.GetAllBranchOfUserString(userId),
                            page, size
                            );
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

        [HttpGet("getionew")]
        [Produces(typeof(UserViewModel))]
        public IActionResult GetIONew()
        {
            var dt = DateTime.Now;
            var list = GetIOByIOTypeIdMain(string.Empty, string.Empty, dt.Date, new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59), (int)EnumIOType.IORegisCourse, 0, 0);
            return this.Ok(list.Count());

        }

        [HttpGet("getioall")]
        [Produces(typeof(UserViewModel))]
        public IActionResult GetIOAll()
        {
            var dt = DateTime.Now;
            var list = GetIOByIOTypeIdMain(string.Empty, string.Empty, new DateTime(1900, 01, 01), dt, (int)EnumIOType.IORegisCourse, 0, 0);
            return this.Ok(list.Count());

        }

        [HttpGet("getiodetailbyiotypeid")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetIODetailByIOTypeId(string filter, string value, string fromDate, string toDate, int page, int size)
        {
            var list = await GetIODetailByIOTypeIdMain(filter, value, fromDate, toDate, page, size);

            return Json(new
            {
                Total = this._unitOfWork.IOStocks.Total,
                List = list
            });
        }

        [HttpGet("reportiodetailbyiotypeid")]
        [Produces(typeof(UserViewModel))]
        public async Task<IActionResult> ReportIODetailByIOTypeId(string filter, string value, string fromDate, string toDate, int page, int size)
        {

            var list = GetIOByIOTypeIdMain(filter, value, fromDate.BuildDateTimeFromSEFormat(), toDate.BuildLastDateTimeFromSEFormat(), 0, 0, 0);
            var item = new ChartViewModel();
            if (list != null && list.Count() > 0)
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

        public async Task<List<IOStockViewModel>> GetIODetailByIOTypeIdMain(string filter, string value, string fromDate, string toDate, int page, int size)
        {
            var unit = this._unitOfWork.IOStocks;
            var results = unit.GetIOStockDetailList(
                            fromDate.BuildDateTimeFromSEFormat(),
                            toDate.BuildLastDateTimeFromSEFormat(),
                            value,
                            0,
                            this._unitOfWork.Branches.GetAllBranchOfUserString(userId),
                            page, size
                        );
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
                    IOStockDetailId = item.IOStockDetailId,
                    MaterialId = item.MaterialId,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    Quantity = item.InputQuantity,
                    BranchName = item.BranchName
                });
            }
            return list;
        }

        [HttpGet("getiostockdetaildept")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetIOStockDetailDept(string filter, string value, Guid? studentId, Guid? ioStockId, int dept, int page, int size)
        {
            var list = IOStockDetailDeptMain(filter, value, studentId, ioStockId, dept, page, size);
            return Json(new
            {
                Total = this._unitOfWork.IOStocks.Total,
                List = list
            });
        }

        [HttpGet("csviostockdetaildept")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> CSVIOStockDetailDept(string filter, string value, Guid? studentId, Guid? ioStockId, int dept, int page, int size)
        {
            var list = IOStockDetailDeptMain(filter, value, studentId, ioStockId, dept, page, size);
            var contents = base.CSV<IOStockViewModel>(list);
            return Json(contents);
        }

        private List<IOStockViewModel> IOStockDetailDeptMain(string filter, string value, Guid? studentId, Guid? ioStockId, int dept, int page, int size)
        {
            var unit = this._unitOfWork.IOStocks;
            var results = unit.GetIOStockDetailListDept(
                            studentId,
                            ioStockId,
                            value,
                            dept > 1 ? true : false,
                            this._unitOfWork.Branches.GetAllBranchOfUserString(userId),
                            page, size
                        );
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
                    Note = item.Note,
                    StudentId = item.StudentId,
                    IOStockDetailId = item.IOStockDetailId,
                    MaterialId = item.MaterialId,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    Quantity = item.InputQuantity,
                    FullNameExport = item.FullNameExport,
                    InputExport = item.InputExport,
                    ByExport = item.ByExport
                });
            }
            return list;
        }

        [HttpGet("getwarehousecard")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetWarehouseCard(string filter, string value, string fromDate, string toDate, int page, int size)
        {
            var unit = this._unitOfWork.IOStocks;
            var results = unit.GetWarehouseCard(
                            fromDate.BuildDateTimeFromSEFormat(),
                            toDate.BuildLastDateTimeFromSEFormat(),
                            value,
                            0,
                            this._unitOfWork.Branches.GetAllBranchOfUserString(userId),
                            page, size
                        );
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
                    IOStockDetailId = item.IOStockDetailId,
                    MaterialId = item.MaterialId,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    Quantity = item.InputQuantity,
                    QuantityInput = item.QuantityInput,
                    QuantityOutput = item.QuantityOutput
                });
            }
            return Json(new
            {
                Total = unit.Total,
                List = list
            });
        }

        [HttpGet("getiopaymentwaitingclass")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetIOPaymentWaitingClass(string filter, string value, int getAll, string ioId, string fromDate, string toDate, int page, int size)
        {
            return await GetIOPayment(filter, value, getAll, false, ioId, fromDate, toDate, true, page, size);
        }

        [HttpGet("getiopayment")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetIOPaymentReceipt(string filter, string value, int isInput, int getAll, int isWaitingClass, string ioId, string fromDate, string toDate, int page, int size)
        {
            return await GetIOPayment(filter, value, getAll, (isInput > 0 ? true : false), ioId, fromDate, toDate, (isWaitingClass > 0 ? true : false), page, size);
        }

        [HttpGet("getiostockdetailprolist")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetIOStockDetailProList(Guid? iostockDetailId, int page, int size)
        {
            var unit = this._unitOfWork.IOStocks;
            var results = unit.GetIOStockDetailProList(
               iostockDetailId, page, size);
            return Json(new
            {
                Total = unit.Total,
                List = results
            });
        }

        [HttpGet("getpromotionmaterial")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetPromotionMaterial(Guid? materialId, int isForever, int page, int size)
        {
            var unit = this._unitOfWork.IOStocks;
            var results = unit.GetPromotionMaterials(
               materialId, isForever == 0 ? false : true,
                this._unitOfWork.Branches.GetAllBranchOfUserString(userId),
                page, size);
            return Json(new
            {
                Total = unit.Total,
                List = results
            });
        }

        [HttpGet("getpromotioniostockdetail")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetPromotionIOStockDetail(Guid? ioStockDetailId)
        {
            var unit = this._unitOfWork.IOStocks;
            var results = unit.GetPromotionIOStockDetail(ioStockDetailId);
            return Json(new
            {
                Total = unit.Total,
                List = results
            });
        }

        [HttpGet("csviopayment")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> CSVIOPaymentReceipt(string filter, string value, int isInput, int getAll, int isWaitingClass, string ioId, string fromDate, string toDate, int page, int size)
        {
            var list = await GetIOPaymentMain(filter, value, getAll, (isInput > 0 ? true : false), ioId, fromDate, toDate, (isWaitingClass > 0 ? true : false), page, size);
            var contents = base.CSV<IOStockViewModel>(list);
            return Json(contents);
        }

        [HttpGet("getiopaymentdetail")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetIOPaymentDetail(string filter, string value, int isInput, int getAll, int isWaitingClass, int isLearning, string ioId, string fromDate, string toDate, int page, int size)
        {
            var list = await GetIOPaymentDetailMain
                (filter, value, isInput, getAll, isWaitingClass,
                 isLearning, ioId, fromDate, toDate, page, size);
            return Json(new
            {
                Total = this._unitOfWork.IOStocks.Total,
                List = list
            });
        }

        [HttpGet("csviopaymentdetail")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> CSVIOPaymentDetail(string filter, string value, int isInput, int getAll, int isWaitingClass, int isLearning, string ioId, string fromDate, string toDate, int page, int size)
        {
            var list = await GetIOPaymentDetailMain
                (filter, value, isInput, getAll, isWaitingClass,
                 isLearning, ioId, fromDate, toDate, page, size);
            var contents = base.CSV<IOStockViewModel>(list);
            return Json(contents);
        }

        public async Task<List<IOStockViewModel>> GetIOPaymentDetailMain(string filter, string value, int isInput, int getAll, int isWaitingClass, int isLearning, string ioId, string fromDate, string toDate, int page, int size)
        {
            var frDate = fromDate.BuildDateTimeFromSEFormat();
            var tDate = toDate.BuildLastDateTimeFromSEFormat();
            if (!string.IsNullOrEmpty(ioId))
            {
                var io = await this._unitOfWork.IOStocks.FindById(Guid.Parse(ioId));
                if (io != null)
                {
                    frDate = new DateTime(io.CreatedDate.Year, io.CreatedDate.Month, io.CreatedDate.Day);
                }
            }

            var unit = this._unitOfWork.IOStocks;
            var results = this._unitOfWork.IOStocks
                 .GetIOStockPaymentListDetail(
                    frDate,
                    tDate,
                    value,
                    ioId,
                    0,
                    (isInput > 0 ? true : false),
                    (isWaitingClass > 0 ? true : false), (isLearning > 0 ? true : false)
                 , this._unitOfWork.Branches.GetAllBranchOfUserString(userId), page, size);

            return MappingIOStockList(results, getAll);
        }

        [HttpGet("getiopaymentvoucher")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetIOPaymentVoucher(string filter, string value, int getAll, string ioId, string fromDate, string toDate, int page, int size)
        {
            return await GetIOPayment(filter, value, getAll, true, ioId, fromDate, toDate, false, page, size);
        }

        public async Task<List<IOStockViewModel>> GetIOPaymentMain(string filter, string value, int getAll, bool isInput,
           string ioId, string fromDate, string toDate, bool isWaitingClass, int page, int size)
        {
            var frDate = fromDate.BuildDateTimeFromSEFormat();
            var tDate = toDate.BuildLastDateTimeFromSEFormat();
            if (!string.IsNullOrEmpty(ioId))
            {
                var io = await this._unitOfWork.IOStocks.FindById(Guid.Parse(ioId));
                if (io != null)
                {
                    frDate = new DateTime(io.CreatedDate.Year, io.CreatedDate.Month, io.CreatedDate.Day);
                }
            }

            var unit = this._unitOfWork.IOStocks;
            var results = this._unitOfWork.IOStocks
                 .GetIOStockPaymentList(
                    frDate,
                    tDate,
                    value,
                    ioId,
                    0, isInput, isWaitingClass
                 , this._unitOfWork.Branches.GetAllBranchOfUserString(userId), page, size);

            return MappingIOStockList(results, getAll);
        }
        public async Task<JsonResult> GetIOPayment(string filter, string value, int getAll, bool isInput,
        string ioId, string fromDate, string toDate, bool isWaitingClass, int page, int size)
        {
            var list = await GetIOPaymentMain(filter, value, getAll, isInput,
             ioId, fromDate, toDate, isWaitingClass, page, size);

            return Json(new
            {
                Total = this._unitOfWork.IOStocks.Total,
                List = list
            });
        }

        public List<IOStockViewModel> MappingIOStockList(IEnumerable<IOStockListPayment> results, int getAll)
        {
            if (getAll == 0)
            {
                results = results.Where(p => p.TotalPriceExist > 0);
            }

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
                    TotalPriceExist = item.TotalPriceExist,
                    TotalPricePayment = item.TotalPricePayment,
                    IOStockDetailId = item.IOStockDetailId.HasValue ? item.IOStockDetailId.Value : Guid.Empty,
                    MaterialId = item.MaterialId.HasValue ? item.MaterialId.Value : Guid.Empty,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName
                });
            }
            return list;
        }

        [HttpPost("updatioedept")]
        public async Task<IActionResult> SaveIOStockDept([FromBody]List<IOStockDetailSmallViewModel> ios)
        {
            if (ModelState.IsValid)
            {
                this._unitOfWork.IOStocks.SaveDept(ios.Select(p => new IOStockDetail
                {
                    IOStockId = p.IOStockId.HasValue ? p.IOStockId.Value : Guid.Empty,
                    IOStockDetailId = p.IOStockDetailId.HasValue ? p.IOStockDetailId.Value : Guid.Empty,
                    InputExport = p.InputExport,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = userId,
                    UpdatedDate = DateTime.Now
                }).ToArray());
                return Ok(new ClassExamineViewModel());
            }
            return Ok(null);
        }
    }
}
