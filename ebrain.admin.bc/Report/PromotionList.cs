using System;
using System.Collections.Generic;
using System.Text;

namespace ebrain.admin.bc.Report
{
    public class PromotionList
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
        public string FullNameCreate { get; set; }
        public string FullNameApprove { get; set; }

        public string FullNameStudent { get; set; }

        public Guid? Materialid { get; set; }
        public Guid? PromotionDetailId { get; set; }
        public decimal? PriceOrigion { get; set; }
        public decimal? PercentDiscount { get; set; }
        public decimal? MoneyDiscount { get; set; }
        public decimal? TotalPrice { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }

        public string IONumber { get; set; }
        public Guid? IOStockId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public decimal? CountPromotion { get; set; }
    }
}
