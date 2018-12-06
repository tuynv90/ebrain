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
    public class ClassExamineViewModel
    {
        public Guid ClassExamineId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? StudentId { get; set; }
        public Guid? ExamineId { get; set; }
        public string ExamineCode { get; set; }
        public string ExamineName { get; set; }
        public decimal Mark { get; set; }
        public decimal? PercentMark { get; set; }
    }

    public class ClassExamineNoteViewModel
    {
        public Guid ClassExamineNoteId { get; set; }
        public string ExamineNoteCode { get; set; }
        public string ExamineNoteName { get; set; }
        public bool IsSummarize { get; set; }
        public Guid? ExamineNoteId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? ExamineId { get; set; }
        public Guid? StudentId { get; set; }
        public string Attend { get; set; }
        public string NotAttend { get; set; }
    }

    public class ExamineMaterialViewModel
    {
        public Guid ExamineMaterialId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? ExamineId { get; set; }
        public Guid? MaterialId { get; set; }
        public Guid? ParentExamineId { get; set; }
        public string ExamineCode { get; set; }
        public string ExamineName { get; set; }
        public bool IsExist { get; set; }
        public decimal? PercentMark { get; set; }
    }

    public class ExamineDocumentViewModel
    {
        public Guid ExamineDocumentId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? MaterialId { get; set; }
        public Guid? ExamineId { get; set; }
        public Guid? DocumentId { get; set; }
        public Guid? ParentExamineId { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentName { get; set; }
        public bool IsExist { get; set; }
    }
}


