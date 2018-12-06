using ebrain.admin.bc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Models
{
    public class ExamineMaterial : HistoricalEntity
    {
        public Guid ExamineMaterialId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? ExamineId { get; set; }
        public Guid? MaterialId { get; set; }
        public decimal? PercentMark { get; set; }
    }

    public class ExamineDocument : HistoricalEntity
    {
        public Guid ExamineDocumentId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? ExamineId { get; set; }
        public Guid? MaterialId { get; set; }
        public Guid? DocumentId { get; set; }
    }
}