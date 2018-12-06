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
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Ebrain.ViewModels
{
    public class ClassStudentViewModel
    {
        public Guid ID { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? StudentId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public Guid? MaterialId { get; set; }
        public Guid? IOStockId { get; set; }
        public Guid? IOStockDetailId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string MaterialName { get; set; }
        public long? ClassStudentStatusId { get; set; }
    }
}
