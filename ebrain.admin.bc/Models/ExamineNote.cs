using ebrain.admin.bc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Models
{
    public class ExamineNote : HistoricalEntity
    {
        public Guid ExamineNoteId { get; set; }
        public Guid? BranchId { get; set; }
        public string ExamineNoteCode { get; set; }
        public string ExamineNoteName { get; set; }
        public bool IsSummarize { get; set; }
    }
}