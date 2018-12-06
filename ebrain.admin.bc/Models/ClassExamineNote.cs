using ebrain.admin.bc.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Models
{
    public class ClassExamineNote : HistoricalEntity
    {
        public Guid ClassExamineNoteId { get; set; }
        public Guid? ExamineNoteId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? ExamineId { get; set; }
        public Guid? StudentId { get; set; }
        public string Attend { get; set; }
        public string NotAttend { get; set; }
    }
}