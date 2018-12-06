using ebrain.admin.bc.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Models
{
    public class PromotionsDetail : HistoricalEntity
    {
        public Guid PromotionDetailId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid? MaterialId { get; set; }
        public decimal? PriceOrigion { get; set; }
        public decimal? PercentDiscount { get; set; }
        public decimal? MoneyDiscount { get; set; }
        public decimal? TotalPrice { get; set; }
        [NotMapped]
        public string MaterialCode { get; set; }
        [NotMapped]
        public string MaterialName { get; set; }
        [NotMapped]
        public string PromotionCode { get; set; }
        [NotMapped]
        public string PromotionName { get; set; }
    }
}