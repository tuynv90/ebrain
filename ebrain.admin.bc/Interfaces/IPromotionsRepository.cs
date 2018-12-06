// ======================================
// Author: Ebrain Team
// Email:  info@ebrain.com.vn
// Copyright (c) 2017 www.ebrain.com.vn
// 
// ==> Contact Us: contact@ebrain.com.vn
// ======================================

using ebrain.admin.bc.Models;
using ebrain.admin.bc.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Repositories.Interfaces
{
    public interface IPromotionsRepository : IRepository<Promotions>
    {
        int Total { get; }
        Task<IEnumerable<Promotions>> Search(string filter, string value, DateTime fromDate, DateTime toDate, string branchIds);
        IEnumerable<PromotionList> GetPromotionList(DateTime fromDate, DateTime toDate,
                string filterValue, bool isForever, string branchIds, int page, int size);
        IEnumerable<PromotionList> GetPromotionListReport(DateTime fromDate, DateTime toDate,
                string filterValue, bool isForever, string branchIds, int page, int size);
        IEnumerable<PromotionList> GetPromotionListReportIODetail(Guid? promotionId, int page, int size);
        IEnumerable<PromotionList> GetPromotionListDetail(DateTime fromDate, DateTime toDate,
               string filterValue, bool isForever, string branchIds, int page, int size);
        Task<Promotions> Get(Guid? index);
        Task<bool> SaveApproved(Promotions value, Guid? index);
        Task<Promotions> Save(Promotions value, PromotionsDetail[] detals, Guid? index);
        Task<Boolean> Delete(Guid? id);
    }
}
