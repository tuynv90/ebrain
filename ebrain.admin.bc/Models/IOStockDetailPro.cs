using ebrain.admin.bc.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Models
{
    public class IOStockDetailPro : HistoricalEntity
    {
        public Guid IOStockDetailProId { get; set; }
        public Guid IOStockDetailId { get; set; }
        public Guid? IOStockId { get; set; }
        public Guid? PromotionDetailId { get; set; }
        public Guid? PromotionId { get; set; }
        public decimal? PercentDiscount { get; set; }
        public decimal? MoneyDiscount { get; set; }
        [NotMapped]
        public string PromotionCode { get; set; }
        [NotMapped]
        public string PromotionName { get; set; }
    }
}