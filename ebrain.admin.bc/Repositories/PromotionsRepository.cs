// ======================================
// Author: Ebrain Team
// Email:  info@ebrain.com.vn
// Copyright (c) 2017 www.ebrain.com.vn
// 
// ==> Contact Us: contact@ebrain.com.vn
// ======================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ebrain.admin.bc.Models;
using ebrain.admin.bc.Repositories.Interfaces;
using ebrain.admin.bc.Utilities;
using ebrain.admin.bc.Report;

namespace ebrain.admin.bc.Repositories
{
    public class PromotionsRepository : Repository<LevelClass>, IPromotionsRepository
    {
        public int Total { get; private set; }
        public PromotionsRepository(ApplicationDbContext context) : base(context)
        { }



        private ApplicationDbContext appContext
        {
            get { return (ApplicationDbContext)_context; }
        }

        public Task<IEnumerable<Promotions>> Search(string filter, string value, DateTime fromDate, DateTime toDate, string branchIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PromotionList> GetPromotionListDetail(DateTime fromDate, DateTime toDate,
               string filterValue, bool isForever, string branchIds, int page, int size)
        {
            try
            {
                List<PromotionList> someTypeList = new List<PromotionList>();
                this.appContext.LoadStoredProc("dbo.sp_PromotionListDetail")
                               .WithSqlParam("fromDate", fromDate)
                               .WithSqlParam("toDate", toDate)
                               .WithSqlParam("@isForever", isForever)
                               .WithSqlParam("branchIds", branchIds)
                               .WithSqlParam("filterValue", filterValue).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<PromotionList>().ToList();
                               });

                //paging
                this.Total = someTypeList.Count();
                if (size > 0 && page >= 0)
                {
                    someTypeList = (from c in someTypeList select c).Skip(page * size).Take(size).ToList();
                }

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<PromotionList> GetPromotionListReport(DateTime fromDate, DateTime toDate,
                string filterValue, bool isForever, string branchIds, int page, int size)
        {
            try
            {
                List<PromotionList> someTypeList = new List<PromotionList>();
                this.appContext.LoadStoredProc("dbo.sp_PromotionList_Report")
                               .WithSqlParam("fromDate", fromDate)
                               .WithSqlParam("toDate", toDate)
                               .WithSqlParam("@isForever", isForever)
                               .WithSqlParam("branchIds", branchIds)
                               .WithSqlParam("filterValue", filterValue).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<PromotionList>().ToList();
                               });

                //paging
                this.Total = someTypeList.Count();
                if (size > 0 && page >= 0)
                {
                    someTypeList = (from c in someTypeList select c).Skip(page * size).Take(size).ToList();
                }

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PromotionList> GetPromotionListReportIODetail(Guid? promotionId, int page, int size)
        {
            try
            {
                List<PromotionList> someTypeList = new List<PromotionList>();
                this.appContext.LoadStoredProc("dbo.sp_PromotionList_ReportIODetail")
                               .WithSqlParam("@promotionId", promotionId).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<PromotionList>().ToList();
                               });

                //paging
                this.Total = someTypeList.Count();
                if (size > 0 && page >= 0)
                {
                    someTypeList = (from c in someTypeList select c).Skip(page * size).Take(size).ToList();
                }

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PromotionList> GetPromotionList(DateTime fromDate, DateTime toDate,
                string filterValue, bool isForever, string branchIds, int page, int size)
        {
            try
            {
                List<PromotionList> someTypeList = new List<PromotionList>();
                this.appContext.LoadStoredProc("dbo.sp_PromotionList")
                               .WithSqlParam("fromDate", fromDate)
                               .WithSqlParam("toDate", toDate)
                               .WithSqlParam("@isForever", isForever)
                               .WithSqlParam("branchIds", branchIds)
                               .WithSqlParam("filterValue", filterValue).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<PromotionList>().ToList();
                               });

                //paging
                this.Total = someTypeList.Count();
                if (size > 0 && page >= 0)
                {
                    someTypeList = (from c in someTypeList select c).Skip(page * size).Take(size).ToList();
                }

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PromotionsDetail> GetDetail(Guid? promotionId)
        {
            try
            {
                List<PromotionsDetail> someTypeList = new List<PromotionsDetail>();
                this.appContext.LoadStoredProc("dbo.sp_PromotionListDetailMaterial")
                               .WithSqlParam("@promotionId", promotionId).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<PromotionsDetail>().ToList();
                               });
                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Promotions> Get(Guid? index)
        {
            var item =  await this.appContext.Promotions.FirstOrDefaultAsync(p => p.IsDeleted == false && p.PromotionId == index);
            if(item != null)
            {
                item.Details = this.GetDetail(index).ToArray();
            }
            return item;
        }

        public async Task<IEnumerable<PromotionsDetail>> GetDetailByIOId(Guid? id)
        {
            return await this.appContext.PromotionsDetail.Where(p => p.IsDeleted == false && p.PromotionId == id).ToListAsync();
        }

        public async Task<bool> SaveApproved(Promotions value, Guid? index)
        {
            var item = this.appContext.Promotions.FirstOrDefault(p => p.PromotionId == index);
            if (item != null)
            {
                item.IsApproved = value.IsApproved;
                item.CreatedByApproved = value.CreatedByApproved;
                item.CreatedDateApproved = value.CreatedDateApproved;
                item.UpdatedBy = value.UpdatedBy;
                item.UpdatedDate = value.UpdatedDate;
            }
            return await appContext.SaveChangesAsync() > 0;
        }

        public async Task<Promotions> Save(Promotions value, PromotionsDetail[] iosd, Guid? index)
        {
            try
            {
                //get branchId
                value.BranchId = value.CreatedBy.GetBranchOfCurrentUser(this.appContext);
                var item = this.appContext.Promotions.FirstOrDefault(p => p.PromotionId == index);
                if (item != null)
                {
                    item.Note = value.Note;
                    item.FromDate = value.FromDate;
                    item.ToDate = value.ToDate;
                    item.IsOff = value.IsOff;
                    item.UpdatedBy = value.UpdatedBy;
                    item.UpdatedDate = value.UpdatedDate;
                    item.IsApproved = value.IsApproved;
                    item.CreatedByApproved = value.CreatedByApproved;
                    item.CreatedDateApproved = value.CreatedDateApproved;

                    var iodExists = await GetDetailByIOId(item.PromotionId);

                    //update deleted
                    var iodIds = iosd.Select(p => p.PromotionDetailId);
                    var iodNotExists = iodExists.Where(p => !iodIds.Contains(p.PromotionDetailId));
                    foreach (var itemDetail in iodNotExists)
                    {
                        itemDetail.IsDeleted = true;
                    }
                    //Insert
                    foreach (var itemDetail in iosd)
                    {
                        var itemExistD = this.appContext.PromotionsDetail.FirstOrDefault(p => p.PromotionDetailId == itemDetail.PromotionDetailId);
                        if (itemExistD != null)
                        {
                            itemExistD.MaterialId = itemDetail.MaterialId;
                            itemExistD.PriceOrigion = itemDetail.PriceOrigion;
                            itemExistD.PercentDiscount = itemDetail.PercentDiscount;
                            itemExistD.MoneyDiscount = itemDetail.MoneyDiscount;
                            itemExistD.TotalPrice = itemDetail.TotalPrice;
                            itemExistD.Note = itemDetail.Note;
                        }
                        else
                        {
                            itemDetail.PromotionId = item.PromotionId;
                            await appContext.PromotionsDetail.AddAsync(itemDetail);
                        }
                    }
                }
                else
                {
                    var result = await appContext.Promotions.AddAsync(value);
                    foreach (var itemD in iosd)
                    {
                        await appContext.PromotionsDetail.AddAsync(itemD);
                    }
                    item = result.Entity;
                }
                //
                await appContext.SaveChangesAsync();

                var itemExist = this.appContext.Users.FirstOrDefault(p => p.Id == item.UpdatedBy.ToString());
                item.UpdatedByName = itemExist != null ? itemExist.FullName : string.Empty;
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Delete(Guid? id)
        {
            var itemExist = appContext.Promotions.FirstOrDefault(p => p.PromotionId == id);
            if (itemExist != null)
            {
                itemExist.IsDeleted = true;
            }
            return await appContext.SaveChangesAsync() > 0;
        }
    }
}
