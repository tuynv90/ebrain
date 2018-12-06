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
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Ebrain.ViewModels
{
    public class ConsultantViewModel
    {
        public Guid? ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }

        public string StartDate { get; set; }
        public string StartTime{ get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public string ScheduleNote { get; set; }
    }

    public class ConsultantContactViewModel
    {
        public Guid? ConsultantContactId { get; set; }
        public string ConsultantContactCode { get; set; }
        public string ConsultantContactName { get; set; }
        public string PhoneContact { get; set; }
        public string ContactName { get; set; }
        public Guid? BranchId { get; set; }
        public string ScheduleStartDate { get; set; }
        public string ScheduleEndDate { get; set; }
        public string ScheduleStartTime { get; set; }
        public string ScheduleEndTime { get; set; }
        public string ScheduleNote { get; set; }
        public bool IsConfirm { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
