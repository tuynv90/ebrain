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

namespace ebrain.admin.bc.Repositories
{
    public class ConsultantRepository : Repository<Consultant>, IConsultantRepository
    {
        public int Total { get; private set; }

        public ConsultantRepository(ApplicationDbContext context) : base(context)
        { }


        public IEnumerable<Consultant> GetTopActive(int count)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Consultant>> Search(string filter, string value, string branchIds, int page, int size)
        {
            var someTypeList = this.appContext.Consultant.Where(p => p.IsDeleted == false &&
                   branchIds.Contains(p.BranchId.ToString())
                );

            //paging
            this.Total = someTypeList.Count();
            if (size > 0 && page >= 0)
            {
                someTypeList = (from c in someTypeList select c).Skip(page * size).Take(size);
            }
            return someTypeList;
        }

        public async Task<IEnumerable<ConsultantContact>> SearchConsultant(string filter, string value, 
                        DateTime fromDate, DateTime toDate, string branchIds, int page, int size)
        {
            var someTypeList = this.appContext.ConsultantContact.Where(
                               p => p.IsDeleted == false && branchIds.Contains(p.BranchId.ToString())
                               && (string.IsNullOrEmpty(value) || p.ConsultantContactCode.Contains(value)
                                    || p.ConsultantContactName.Contains(value)  || p.ContactName.Contains(value)
                                    || p.PhoneContact.Contains(value) || p.ScheduleNote.Contains(value))
                               && p.ScheduleStartDate.Value.Date >= fromDate.Date
                               && p.ScheduleEndDate.Value.Date <= toDate.Date
                            );

            //paging
            this.Total = someTypeList.Count();
            if (size > 0 && page >= 0)
            {
                someTypeList = (from c in someTypeList select c).Skip(page * size).Take(size);
            }
            return someTypeList;
        }

        public async Task<ConsultantContact> SaveConsultant(ConsultantContact value, Guid? index)
        {
            value.BranchId = value.CreatedBy.GetBranchOfCurrentUser(this.appContext);
            var itemExist = await GetConsultant(index);
            if (itemExist != null)
            {
                itemExist.ConsultantContactCode = value.ConsultantContactCode;
                itemExist.ConsultantContactName = value.ConsultantContactName;
                itemExist.ContactName = value.ContactName;
                itemExist.PhoneContact = value.PhoneContact;
                itemExist.IsConfirm = value.IsConfirm;
                itemExist.Note = value.Note;
                itemExist.UpdatedBy = value.UpdatedBy;
                itemExist.UpdatedDate = DateTime.Now;
                itemExist.ScheduleStartDate = value.ScheduleStartDate;
                itemExist.ScheduleEndDate = value.ScheduleEndDate;
                itemExist.ScheduleNote = value.ScheduleNote;
            }
            else
            {
                var result = await appContext.ConsultantContact.AddAsync(value);
                itemExist = result.Entity;
            }
            await appContext.SaveChangesAsync();
            //
            return itemExist;
        }
        public async Task<Consultant> Save(Consultant value, Guid? index)
        {
            value.BranchId = value.CreatedBy.GetBranchOfCurrentUser(this.appContext);
            var itemExist = await Get(index);
            if (itemExist != null)
            {
                itemExist.ConsultantCode = value.ConsultantCode;
                itemExist.ConsultantName = value.ConsultantName;
                itemExist.Note = value.Note;
                itemExist.UpdatedBy = value.UpdatedBy;
                itemExist.UpdatedDate = DateTime.Now;
                itemExist.ScheduleStartDate = value.ScheduleStartDate;
                itemExist.ScheduleEndDate = value.ScheduleEndDate;
                itemExist.ScheduleNote = value.ScheduleNote;
            }
            else
            {
                var result = await appContext.Consultant.AddAsync(value);
                itemExist = result.Entity;
            }
            await appContext.SaveChangesAsync();
            //
            return itemExist;
        }

        public async Task<bool> Delete(string id)
        {
            var itemExist = appContext.Consultant.FirstOrDefault(p => p.ConsultantId.Equals(new Guid(id)));
            if (itemExist != null)
            {
                itemExist.IsDeleted = true;
            }
            await appContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteConsultant(string id)
        {
            var itemExist = appContext.ConsultantContact.FirstOrDefault(p => p.ConsultantContactId.Equals(new Guid(id)));
            if (itemExist != null)
            {
                itemExist.IsDeleted = true;
            }
            await appContext.SaveChangesAsync();
            return true;
        }

        public Task<Consultant> Get(Guid? index)
        {
            return this.appContext.Consultant.FirstOrDefaultAsync(p => p.ConsultantId == index);
        }

        public Task<ConsultantContact> GetConsultant(Guid? index)
        {
            return this.appContext.ConsultantContact.FirstOrDefaultAsync(p => p.ConsultantContactId == index);
        }

        private ApplicationDbContext appContext
        {
            get { return (ApplicationDbContext)_context; }
        }
    }
}
