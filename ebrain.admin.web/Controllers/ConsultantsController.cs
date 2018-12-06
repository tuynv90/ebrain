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
using ebrain.admin.bc.Utilities;
namespace Ebrain.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Security("8AA6E971-1C3D-4835-B154-D662CE12AE97")]
    public class ConsultantsController : BaseController
    {
        private IUnitOfWork _unitOfWork;
        readonly ILogger _logger;


        public ConsultantsController(IUnitOfWork unitOfWork, ILogger<ConsultantsController> logger) : base(unitOfWork, logger)
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

        [HttpGet("search")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> Search(string filter, string value, int page, int size)
        {
            var unit = this._unitOfWork.Consultants;
            var ret = from c in await unit.Search(filter, value, this._unitOfWork.Branches.GetAllBranchOfUserString(userId), page, size)
                      select new ConsultantViewModel
                      {
                          ID = c.ConsultantId,
                          Code = c.ConsultantCode,
                          Name = c.ConsultantName,
                          Note = c.Note
                      };

            return Json(new
            {
                Total = unit.Total,
                List = ret
            });
        }

        [HttpGet("searchconsultant")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> SearchConsultant(string filter, string value, string fromDate, string toDate, int page, int size)
        {
            var unit = this._unitOfWork.Consultants;
            var ret = from c in await unit.SearchConsultant(
                        filter, value,
                        fromDate.BuildDateTimeFromSEFormat(),
                        toDate.BuildDateTimeFromSEFormat(),
                        this._unitOfWork.Branches.GetAllBranchOfUserString(userId), page, size)
                      select new ConsultantContactViewModel
                      {
                          ConsultantContactId = c.ConsultantContactId,
                          ConsultantContactCode = c.ConsultantContactCode,
                          ConsultantContactName = c.ConsultantContactName,
                          ScheduleNote = c.ScheduleNote,
                          PhoneContact = c.PhoneContact,
                          ContactName = c.ContactName,
                          IsConfirm = c.IsConfirm,
                          ScheduleStartDate = c.ScheduleStartDate.Value.Date.ToString(),
                          ScheduleEndDate = c.ScheduleEndDate.Value.Date.ToString(),
                          ScheduleStartTime = c.ScheduleStartDate.ToString(),
                          ScheduleEndTime = c.ScheduleEndDate.ToString(),
                      };

            return Json(new
            {
                Total = unit.Total,
                List = ret
            });
        }

        [HttpPost("updateconsultant")]
        public async Task<IActionResult> UpdateConsultant([FromBody] ConsultantContactViewModel value)
        {
            if (ModelState.IsValid)
            {
                var userId = Utilities.GetUserId(this.User);

                var startDate= value.ScheduleStartDate.BuildYYYYMMDDHHSSFromSEFormat();
                //var endTime = value.ScheduleEndDate.BuildYYYYMMDDHHSSFromSEFormat();

                var startTime = value.ScheduleStartTime.BuildYYYYMMDDHHSSFromSEFormat();
                var endDate = value.ScheduleEndTime.BuildYYYYMMDDHHSSFromSEFormat();

                startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, startTime.Second);
                endDate = startDate; // new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, endTime.Second);

                var ret = await this._unitOfWork.Consultants.SaveConsultant(new ConsultantContact
                {
                    ConsultantContactId = Guid.NewGuid(),
                    ConsultantContactCode = value.ConsultantContactCode,
                    ConsultantContactName = value.ConsultantContactName,
                    ScheduleNote = value.ScheduleNote,
                    PhoneContact = value.PhoneContact,
                    ContactName = value.ContactName,
                    IsConfirm = value.IsConfirm,
                    ScheduleStartDate = startDate,
                    ScheduleEndDate = endDate,
                    CreatedDate = DateTime.Now,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    UpdatedDate = DateTime.Now,
                }, value.ConsultantContactId);

                return Ok(ret);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] ConsultantViewModel value)
        {
            if (ModelState.IsValid)
            {
                var userId = Utilities.GetUserId(this.User);
                var ret = await this._unitOfWork.Consultants.Save(new Consultant
                {
                    ConsultantId = Guid.NewGuid(),
                    ConsultantCode = value.Code,
                    BranchId = Guid.NewGuid(),
                    ConsultantName = value.Name,
                    Note = value.Note,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    ScheduleStartDate = DateTime.Now,
                    ScheduleEndDate = DateTime.Now,
                    ScheduleNote = value.ScheduleNote,
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
                var userId = Utilities.GetUserId(this.User);

                var ret = await this._unitOfWork.Consultants.Delete(id);
                return Ok(ret);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("removeconsultant")]
        public async Task<IActionResult> RemoveConsultant([FromBody] String id)
        {
            if (ModelState.IsValid)
            {
                var userId = Utilities.GetUserId(this.User);

                var ret = await this._unitOfWork.Consultants.DeleteConsultant(id);
                return Ok(ret);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("get")]
        [Produces(typeof(UserViewModel))]
        public async Task<ConsultantViewModel> Get(Guid? index)
        {
            var c = await this._unitOfWork.Consultants.Get(index);

            return new ConsultantViewModel
            {
                ID = c.ConsultantId,
                Code = c.ConsultantCode,
                Name = c.ConsultantName,
                Note = c.Note
            };
        }

        [HttpGet("getconsultant")]
        [Produces(typeof(UserViewModel))]
        public async Task<ConsultantContactViewModel> GetConsultant(Guid? index)
        {
            var c = await this._unitOfWork.Consultants.GetConsultant(index);

            return new ConsultantContactViewModel
            {
                ConsultantContactId = c.ConsultantContactId,
                ConsultantContactCode = c.ConsultantContactCode,
                ConsultantContactName = c.ConsultantContactName,
                ScheduleNote = c.ScheduleNote,
                PhoneContact = c.PhoneContact,
                ContactName = c.ContactName,
                IsConfirm = c.IsConfirm,
                CreatedBy = c.CreatedBy,
                CreatedDate = c.CreatedDate,
                ScheduleStartDate = c.ScheduleStartDate.ToString(),
                ScheduleEndDate = c.ScheduleEndDate.ToString(),
                ScheduleStartTime = c.ScheduleStartDate.ToString(),
                ScheduleEndTime = c.ScheduleEndDate.ToString()
            };
        }
    }
}
