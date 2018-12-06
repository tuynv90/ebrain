using ebrain.admin.bc.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Models
{
    public class Promotions : HistoricalEntity
    {
        public Guid PromotionId { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IsOff { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? CreatedByApproved { get; set; }
        public DateTime? CreatedDateApproved { get; set; }
        public bool IsApproved { get; set; }

        [NotMapped]
        public PromotionsDetail[] Details { get; set; }

    }
}