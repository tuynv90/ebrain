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
using ebrain.admin.bc.Utilities;
using Microsoft.AspNetCore.Hosting;

namespace Ebrain.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Security("8AA6E971-1C3D-4835-B154-D662CE12AE98", "8AA6E971-1C3D-4835-B154-D662CE12AE14",
        "8AA6E971-1C3D-4835-B154-D662CE12AE99")]
    public class StudentsController : BaseController
    {
        private IUnitOfWork _unitOfWork;
        readonly ILogger _logger;
        readonly IHostingEnvironment _env;

        public StudentsController(IUnitOfWork unitOfWork, ILogger<StudentsController> logger, IHostingEnvironment env) : base(unitOfWork, logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            this._env = env;
        }

        [HttpGet("getall")]
        [Produces(typeof(List<UserViewModel>))]
        public async Task<IActionResult> GetAll(int page, int pageSize)
        {
            var students = await _unitOfWork.Students.GetAll(page, pageSize, this._unitOfWork.Branches.GetAllBranchOfUserString(userId));
            var list = await MappingMaterial(students);
            return Ok(list);
        }

        [HttpGet("search")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> Search(string filter, string value, int page, int size)
        {
            var results = await SearchMain(filter, value, page, size);
            return Json(new
            {
                Total = _unitOfWork.Students.Total,
                List = results
            });
        }

        [HttpGet("csv")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> OutputCSV(string filter, string value, int page, int size)
        {
            var results = await SearchMain(filter, value, page, size);
            var contents = base.CSV<StudentViewModel>(results);
            return Json(contents);
        }

        private async Task<IEnumerable<StudentViewModel>> SearchMain(string filter, string value, int page, int size)
        {
            var students = await _unitOfWork.Students.Search(filter, value, this._unitOfWork.Branches.GetAllBranchOfUserString(userId), page, size);
            return await MappingMaterial(students);
        }

        private Guid userId
        {
            get
            {
                return Utilities.GetUserId(this.User);
            }
        }

        public async Task<IEnumerable<StudentViewModel>> MappingMaterial(IEnumerable<Student> students)
        {
            List<StudentViewModel> list = new List<StudentViewModel>();

            foreach (var item in students)
            {
                var itemNew = await ProcessMappingItem(item);
                list.Add(itemNew);
            }

            return list;
        }

        private async Task<StudentViewModel> ProcessMappingItem(Student item)
        {
            var itemStudentRe = await this._unitOfWork.Students.FindRelationShipByStudentId(item.StudentId);
            var itemNew = new StudentViewModel
            {
                ID = item.StudentId,
                Code = item.StudentCode,
                Name = item.StudentName,
                Note = item.Note,
                Address = item.Address,
                Taxcode = item.TaxCode,
                AccountBank = item.AccountBank,
                Email = item.Email,
                GenderId = item.GenderId,
                Username = item.UserName,
                Password = item.Password,
                SchoolName = item.SchoolName,
                ClassName = item.ClassName,
                Birthday = item.Birthday,
                StudentStatusId = item.StudentStatusId,
                Phone = item.Phone,
                FaUsername = itemStudentRe != null ? itemStudentRe.FullName : string.Empty,
                FaAddress = itemStudentRe != null ? itemStudentRe.Address : string.Empty,
                FaEmail = itemStudentRe != null ? itemStudentRe.Email : string.Empty,
                FaJob = itemStudentRe != null ? itemStudentRe.Job : string.Empty,
                FaRelationship = itemStudentRe != null ? itemStudentRe.RelationRequire : string.Empty,
                FaPhone = itemStudentRe != null ? itemStudentRe.Phone : string.Empty,
                Logo = new FileViewModel
                {
                    Name = string.IsNullOrEmpty(item.LogoName) ? Constants.IMAGE_DEFAULT : item.LogoName.WebRootPathStudent()
                }
            };
            return itemNew;
        }

        [HttpGet("getdefault")]
        [Produces(typeof(UserViewModel))]
        public async Task<StudentViewModel> GetDefault(Guid? index)
        {
            var item = await this._unitOfWork.Students.GetDefault(index);

            var itemMap = await ProcessMappingItem(item);
            if (itemMap != null)
            {
                itemMap.Becomes = item.Becomes.Select(p => new StudentViewModel
                {
                    StudentBecomeDesId = p.StudentBecomeDesId,
                    StudentBecomeDesCode = p.StudentBecomeDesCode,
                    StudentBecomeDesName = p.StudentBecomeDesName,
                    IsExist = p.IsExist,
                    ID = p.StudentId
                }).ToArray();
            }
            return itemMap;
        }

        [HttpGet("get")]
        [Produces(typeof(UserViewModel))]
        public async Task<StudentViewModel> Get(Guid? index)
        {
            var item = await this._unitOfWork.Students.Get(index);

            var itemMap = await ProcessMappingItem(item);
            if (itemMap != null)
            {
                itemMap.Becomes = item.Becomes.Select(p => new StudentViewModel
                {
                    StudentBecomeDesId = p.StudentBecomeDesId,
                    StudentBecomeDesCode = p.StudentBecomeDesCode,
                    StudentBecomeDesName = p.StudentBecomeDesName,
                    IsExist = p.IsExist,
                    ID = p.StudentId
                }).ToArray();
            }
            return itemMap;
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] StudentViewModel value)
        {
            if (ModelState.IsValid)
            {
                var studentId = Guid.NewGuid();

                var ret = await this._unitOfWork.Students.Save(new Student
                {
                    StudentId = studentId,
                    StudentCode = value.Code,
                    BranchId = Guid.NewGuid(),
                    StudentName = value.Name,
                    AccountBank = value.AccountBank,
                    Address = value.Address,
                    Birthday = value.Birthday,
                    ClassName = value.ClassName,
                    SchoolName = value.SchoolName,
                    Phone = value.Phone,
                    TaxCode = value.Taxcode,
                    Email = value.Email,
                    GenderId = value.GenderId,
                    StudentStatusId = value.StudentStatusId,

                    UserName = value.Username,
                    Password = value.Password,
                    Note = value.Note,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,

                }, new StudentRelationShip
                {
                    StudentRelationShipId = Guid.NewGuid(),
                    StudentId = studentId,
                    FullName = value.FaUsername,
                    Facebook = value.FaFacebook,
                    Address = value.FaAddress,
                    Email = value.FaEmail,
                    Job = value.FaJob,
                    Phone = value.FaPhone,
                    Birthday = DateTime.Now,//value.Birthday,
                    BranchId = Guid.NewGuid(),
                    RelationRequire = value.FaRelationship,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                }, value.Becomes.Select(p => new StudentBecome
                {
                    StudentBecomeId = Guid.NewGuid(),
                    StudentId = p.ID,
                    StudentBecomeDesId = p.StudentBecomeDesId,
                    IsExist = p.IsExist,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                }), value.ID);

                //save logo to physical file
                if (value.Logo != null && !string.IsNullOrEmpty(value.Logo.Name) && !string.IsNullOrEmpty(value.Logo.Value))
                {
                    //Convert Base64 Encoded string to Byte Array.
                    var base64String = value.Logo.Value;
                    var fileName = value.Logo.Name;
                    byte[] imageBytes = Convert.FromBase64String(base64String);

                    //Save the Byte Array as Image File.
                    string filePath = fileName.WebRootPathStudent(Guid.NewGuid().ToString(), _env);
                    imageBytes.WriteAllBytes(filePath);

                    //store filename to DB
                    ret.LogoName = filePath.GetFileName();
                }

                // cache id
                studentId = ret.StudentId;

                //commit
                ret = await this._unitOfWork.Students.SaveLogo(ret, value.ID);

                return Ok(this.Get(studentId));
            }

            return BadRequest(ModelState);
        }

        [HttpGet("getbirthdaytudents")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetBirthdayStudent(string filter, string value, string fromDate, string toDate)
        {
            var list = GetBirthdayStudentMain(filter, value, fromDate, toDate);
            return Json(new
            {
                Total = this._unitOfWork.Students.Total,
                List = list
            });
        }

        [HttpGet("csvbirthdaytudents")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> CSVBirthdayStudent(string filter, string value, string fromDate, string toDate)
        {
            var list = GetBirthdayStudentMain(filter, value, fromDate, toDate);
            var contents = base.CSV<StudentViewModel>(list);
            return Json(contents);
        }

        private IEnumerable<StudentViewModel> GetBirthdayStudentMain(string filter, string value, string fromDate, string toDate)
        {
            var unit = this._unitOfWork.Students;
            var results = unit.GetStudentBirthday
                        (
                            this._unitOfWork.Branches.GetAllBranchOfUserString(userId),
                            fromDate.BuildDateTimeFromSEFormat(),
                            toDate.BuildLastDateTimeFromSEFormat()
                        );
            return results.Select(p => new StudentViewModel
            {
                ID = p.StudentId,
                Code = p.StudentCode,
                Name = p.StudentName,
                Birthday = p.Birthday,
                Phone = p.Phone,
                Email = p.Email,
                GenderName = p.GenderName,
                TotalDay = p.TotalDay
            });
        }

        [HttpGet("getstudentendclass")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetStudentEndClass(string filter, string value, string classId, string toDate)
        {
            var unit = this._unitOfWork.Students;
            var list = GetStudentEndClassMain(filter, value, classId, toDate);
            return Json(new
            {
                Total = unit.Total,
                List = list
            });
        }

        [HttpGet("csvstudentendclass")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> CSVStudentEndClass(string filter, string value, string classId, string toDate)
        {
            var list = GetStudentEndClassMain(filter, value, classId, toDate);
            var contents = base.CSV<StudentViewModel>(list);
            return Json(contents);
        }

        private IEnumerable<StudentViewModel> GetStudentEndClassMain(string filter, string value, string classId, string toDate)
        {
            var unit = this._unitOfWork.Students;
            var results = unit.GetStudentEndClass
                        (
                            this._unitOfWork.Branches.GetAllBranchOfUserString(userId),
                            classId,
                            toDate.BuildLastDateTimeFromSEFormat()
                        );
            return results.Select(p => new StudentViewModel
            {
                ID = p.StudentId,
                Code = p.StudentCode,
                Name = p.StudentName,
                Birthday = p.Birthday,
                ClassName = p.ClassName,
                ClassCode = p.ClassCode,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Phone = p.Phone,
                Email = p.Email,
                GenderName = p.GenderName,
                TotalDay = p.TotalDay
            });
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] String id)
        {
            if (ModelState.IsValid)
            {
                var ret = await this._unitOfWork.Students.Delete(id);
                return Ok(ret);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("getstudentbycreatedate")]
        [Produces(typeof(UserViewModel))]
        public async Task<JsonResult> GetStudentByCreateDate(string filter, string value, string fromDate, string toDate, int page, int size)
        {
            var list = GetStudentByCreateDateMain(filter, value, fromDate.BuildDateTimeFromSEFormat(), toDate.BuildLastDateTimeFromSEFormat(), page, size);
            return Json(new
            {
                Total = this._unitOfWork.Students.Total,
                List = list
            });

        }

        private IEnumerable<StudentViewModel> GetStudentByCreateDateMain(string filter, string value, DateTime? fromDate, DateTime? toDate, int page, int size)
        {
            // var results = await this._unitOfWork.IOStocks.Search(filter, value);
            var results = this._unitOfWork.Students.GetStudentByCreateDate
                        (
                            this._unitOfWork.Branches.GetAllBranchOfUserString(userId),
                            fromDate,
                            toDate,
                            page,
                            size
                        );
            return results.Select(p => new StudentViewModel
            {
                ID = p.StudentId,
                Code = p.StudentCode,
                Name = p.StudentName,
                Birthday = p.Birthday,
                Phone = p.Phone,
                Email = p.Email,
                GenderName = p.GenderName,
                TotalDay = p.TotalDay
            });

        }

        [HttpGet("getnewstudent")]
        [Produces(typeof(UserViewModel))]
        public IActionResult GetNewStudent()
        {
            var list = GetStudentByCreateDateMain(string.Empty, string.Empty, DateTime.Now, DateTime.Now, 0, 0);
            return Ok(list.Count());
        }

        [HttpGet("getalltudent")]
        [Produces(typeof(UserViewModel))]
        public IActionResult GetAllStudent()
        {
            var list = GetStudentByCreateDateMain(string.Empty, string.Empty, new DateTime(1900, 01, 01), DateTime.Now, 0, 0);
            return Ok(list.Count());
        }

        [HttpGet("getstudentcourse")]
        [Produces(typeof(UserViewModel))]
        public IActionResult GetStudentCourse(string filterValue, string studentId, int page, int size)
        {
            var unit = this._unitOfWork.Students;
            var results = unit.GetStudentCourse
                        (
                            filterValue,
                            studentId,
                            this._unitOfWork.Branches.GetAllBranchOfUserString(userId),
                            page,
                            size
                        );
            var list = results.Select(p => new StudentViewModel
            {
                ID = p.StudentId,
                Code = p.StudentCode,
                Name = p.StudentName,
                Birthday = p.Birthday,
                ClassName = p.ClassName,
                ClassCode = p.ClassCode,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Phone = p.Phone,
                Email = p.Email,
                GenderName = p.GenderName,
                TotalDay = p.TotalDay,
                MaterialName = p.MaterialName,
                CountAbsent = p.CountAbsent,
                CountNotAbsent = p.CountNotAbsent
            });


            return Json(new
            {
                Total = unit.Total,
                List = list
            });
        }

        [HttpGet("getteachercourse")]
        [Produces(typeof(UserViewModel))]
        public IActionResult GetTeacherCourse(string filterValue, string studentId, int page, int size)
        {
            var unit = this._unitOfWork.Students;
            var results = unit.GetTeacherCourse
                        (
                            filterValue,
                            this._unitOfWork.Branches.GetAllBranchOfUserString(userId),
                            page,
                            size
                        );
            var list = results.Select(p => new StudentViewModel
            {
                ID = p.StudentId,
                Code = p.StudentCode,
                Name = p.StudentName,
                Birthday = p.Birthday,
                ClassName = p.ClassName,
                ClassCode = p.ClassCode,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Phone = p.Phone,
                Email = p.Email,
                GenderName = p.GenderName,
                TotalDay = p.TotalDay,
                MaterialName = p.MaterialName,
                SupplierCode = p.SupplierCode,
                SupplierName = p.SupplierName,
                SupplierId = p.SupplierId,
                RoomName = p.RoomName,
                ShiftClassName = p.ShiftClassName,
                TodayName = p.TodayName
            });


            return Json(new
            {
                Total = unit.Total,
                List = list
            });
        }

        [HttpGet("getstudentpotential")]
        [Produces(typeof(UserViewModel))]
        public IActionResult GetStudentPotential(string filterValue, int page, int size)
        {
            var list = GetStudentPotentialMain(filterValue, page, size);
            return Json(new
            {
                Total = this._unitOfWork.Students.Total,
                List = list
            });
        }

        [HttpGet("csvstudentpotential")]
        [Produces(typeof(UserViewModel))]
        public IActionResult CSVStudentPotential(string filterValue, int page, int size)
        {
            var list = GetStudentPotentialMain(filterValue, page, size);
            var contents = base.CSV<StudentViewModel>(list);
            return Json(contents);
        }


        public IEnumerable<StudentViewModel> GetStudentPotentialMain(string filterValue, int page, int size)
        {
            var unit = this._unitOfWork.Students;
            var results = unit.GetStudentPotential
                        (
                            filterValue,
                            this._unitOfWork.Branches.GetAllBranchOfUserString(userId),
                            page,
                            size
                        );
            return results.Select(p => new StudentViewModel
            {
                ID = p.StudentId,
                Code = p.StudentCode,
                Name = p.StudentName,
                Birthday = p.Birthday,
                Phone = p.Phone,
                GenderName = p.GenderName
            });
        }

        [HttpGet("getstudentlearning")]
        [Produces(typeof(UserViewModel))]
        public IActionResult GetStudentLearning(string filterValue, string fromDate, string toDate,
            string studentId, string branchIds, string classId, int learning, int page, int size)
        {
            var unit = this._unitOfWork.Students;

            if (string.IsNullOrEmpty(branchIds))
            {
                branchIds = this._unitOfWork.Branches.GetAllBranchOfUserString(userId);
            }

            var list = GetStudentLearningMain(filterValue, fromDate, toDate, branchIds, studentId, classId, learning, page, size);
            return Json(new
            {
                Total = unit.Total,
                List = list
            });
        }

        [HttpGet("csvstudentlearning")]
        [Produces(typeof(UserViewModel))]
        public IActionResult CSVStudentLearning(string filterValue, string fromDate, string toDate,
            string branchIds, string studentId, string classId, int learning, int page, int size)
        {
            if (string.IsNullOrEmpty(branchIds))
            {
                branchIds = this._unitOfWork.Branches.GetAllBranchOfUserString(userId);
            }
            var list = GetStudentLearningMain(filterValue, fromDate, toDate, branchIds, studentId, classId, learning, page, size);
            var contents = base.CSV<StudentViewModel>(list);
            return Json(contents);
        }


        private IEnumerable<StudentViewModel> GetStudentLearningMain(string filterValue,
            string fromDate, string toDate,
            string branchIds, string studentId, string classId, int learning, int page, int size)
        {
            var unit = this._unitOfWork.Students;
            bool? isLearning = learning == 0 ? (bool?)null : learning == 1 ? true : false;
            var results = unit.GetStudentLearning
                        (
                            filterValue,
                            fromDate.BuildDateTimeFromSEFormat(),
                            toDate.BuildLastDateTimeFromSEFormat(),
                            string.IsNullOrEmpty(studentId) ? (Guid?)null : Guid.Parse(studentId),
                            string.IsNullOrEmpty(classId) ? (Guid?)null : Guid.Parse(classId),
                            isLearning,
                            branchIds,
                            page,
                            size
                        );
            return results.Select(p => new StudentViewModel
            {
                ID = p.StudentId,
                Code = p.StudentCode,
                Name = p.StudentName,
                Birthday = p.Birthday,
                ClassId = p.ClassId,
                ClassName = p.ClassName,
                ClassCode = p.ClassCode,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Phone = p.Phone,
                Email = p.Email,
                GenderName = p.GenderName,
                TotalDay = p.TotalDay,
                MaterialId = p.MaterialId,
                MaterialName = p.MaterialName,
                SupplierCode = p.SupplierCode,
                SupplierName = p.SupplierName,
                SupplierId = p.SupplierId,
                FinalMark = p.FinalMark,
                MaskPassCourse = p.MaskPassCourse
            });
        }
    }
}
