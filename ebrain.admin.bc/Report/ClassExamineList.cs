using System;
using System.Collections.Generic;
using System.Text;

namespace ebrain.admin.bc.Report
{
    public class ClassExamineList
    {
        public Guid ClassExamineId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? ExamineId { get; set; }
        public Guid? StudentId { get; set; }
        public string ExamineCode { get; set; }
        public string ExamineName { get; set; }
        public decimal Mark { get; set; }
        public decimal? PercentMark { get; set; }
    }

    public class ClassExamineNoteList
    {
        public Guid ClassExamineNoteId { get; set; }
        public string ExamineNoteCode { get; set; }
        public string ExamineNoteName { get; set; }
        public Guid? ExamineNoteId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? ExamineId { get; set; }
        public Guid? StudentId { get; set; }
        public string Attend { get; set; }
        public string NotAttend { get; set; }
        public bool IsSummarize { get; set; }
    }

    public class ExamineMaterialList
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

    public class ExamineDocumentList
    {
        public Guid ExamineDocumentlId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? ExamineId { get; set; }
        public Guid? MaterialId { get; set; }
        public Guid? DocumentId { get; set; }
        public Guid? ParentExamineId { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentName { get; set; }
        public bool IsExist { get; set; }
    }
}
