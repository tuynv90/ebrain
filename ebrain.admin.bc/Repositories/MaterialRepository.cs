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
using ebrain.admin.bc.Report;
using ebrain.admin.bc.Utilities;

namespace ebrain.admin.bc.Repositories
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        public int Total { get; private set; }
        public MaterialRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Material> Get(Guid? index)
        {
            return await this.appContext.Material.FirstOrDefaultAsync(p => p.MaterialId == index);
        }

        public async Task<List<Tuple<Material, Unit[]>>> GetMaterialsAndUnits(int page, int pageSize, string branchIds)
        {
            IQueryable<Material> materialQuery = this.appContext.Material;
            IQueryable<Unit> unitQuery = this.appContext.Unit;

            if (page != -1)
                materialQuery = materialQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                materialQuery = materialQuery.Take(pageSize);

            var materials = await materialQuery.Where(p => p.IsDeleted == false &&
                    branchIds.Contains(p.BranchId.ToString())
                ).ToListAsync();

            var units = await unitQuery.Where(p => p.IsDeleted == false &&
                    branchIds.Contains(p.BranchId.ToString())
                ).ToListAsync();

            return materials.Select(u => Tuple.Create(u, units.ToArray())).ToList();
        }
        public async Task<IEnumerable<Material>> Search(string filter, string value, int page, int size, string branchIds, bool? isLearn = null)
        {
            var results = this.appContext
                .Material
                .Where(p => p.IsDeleted == false &&
                    (string.IsNullOrEmpty(value) || p.MaterialCode.Contains(value) || p.MaterialName.Contains(value))
                    &&
                    branchIds.Contains(p.BranchId.ToString())
                ).OrderByDescending(p => p.UpdatedDate);
            //paging
            this.Total = results.Count();
            if (size > 0 && page >= 0)
            {
                results = (from c in results select c).Skip(page * size).Take(size).OrderByDescending(p => p.UpdatedDate);
            }
            return results;
        }

        public IEnumerable<MaterialList> GetAllMaterialList(int page, int size, string filterValue, string branchIds, bool? isLearn)
        {
            try
            {
                List<MaterialList> someTypeList = new List<MaterialList>();
                this.appContext.LoadStoredProc("dbo.sp_MaterialList")
                               .WithSqlParam("filterValue", filterValue)
                               .WithSqlParam("branchIds", branchIds)
                               .WithSqlParam("isLearn", isLearn).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<MaterialList>().ToList();
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

        private async Task<IEnumerable<Guid>> TypeLearnIds(string branchIds)
        {
            return await this.appContext.TypeMaterial.Where(p => p.IsLearning == true && p.IsDeleted == false)
                .Select(p => p.TypeMaterialId).ToListAsync();
        }

        private async Task<IEnumerable<Guid>> GrpMaterialIdLearnIds(string branchIds)
        {
            var itemTypeIds = TypeLearnIds(branchIds);
            if (itemTypeIds != null && itemTypeIds.Result != null)
            {
                return await this.appContext.GrpMaterial.Where
                    (p => itemTypeIds.Result.Contains(p.TypeMaterialId.HasValue ? p.TypeMaterialId.Value : new Guid())
                            && p.IsDeleted == false)
                    .Select(p => p.GrpMaterialId).ToListAsync();
            }
            return null;
        }

        public async Task<Material> Save(Material value, MaterialHead valueHead, Guid? id)
        {
            //get branchId
            value.BranchId = value.CreatedBy.GetBranchOfCurrentUser(this.appContext);
            valueHead.BranchId = value.BranchId;

            var item = this.appContext.Material.FirstOrDefault(p => p.MaterialId == id);
            if (item != null)
            {
                item.GrpMaterialId = value.GrpMaterialId;
                item.MaterialCode = value.MaterialCode;
                item.MaterialName = value.MaterialName;
                item.Note = value.Note;
                item.UnitId = value.UnitId;

                var itemHead = this.appContext.MaterialHead.FirstOrDefault(p => p.MaterialId == id);
                itemHead.Price = valueHead.Price;
                itemHead.Note = valueHead.Note;
                itemHead.NumberHourse = valueHead.NumberHourse;
                itemHead.PriceAfterVAT = valueHead.PriceAfterVAT;
                itemHead.SellPrice = valueHead.SellPrice;
                itemHead.SpBeCourse = valueHead.SpBeCourse;
                itemHead.SpEnCourse = valueHead.SpEnCourse;

                itemHead.MaskPassCourse = valueHead.MaskPassCourse;
                itemHead.CalBeCourse = valueHead.CalBeCourse;
                itemHead.CalEnCourse = valueHead.CalEnCourse;
                itemHead.DocumentId = valueHead.DocumentId;
            }

            else
            {
                var result = await appContext.Material.AddAsync(value);
                await appContext.MaterialHead.AddAsync(valueHead);
                item = result.Entity;
            }
            await appContext.SaveChangesAsync();
            return item;
        }

        public async Task<bool> SaveClone(Branch[] values, Guid userId)
        {
            try
            {
                foreach (var itemHead in values)
                {
                    var itemExistD = this.appContext.MaterialBranch.FirstOrDefault(
                            p => p.MaterialId == itemHead.MaterialId
                            && p.BranchId == itemHead.BranchId);

                    if (itemExistD != null)
                    {
                        //itemExistD.IsDeleted = !itemHead.IsExist;
                    }
                    else
                    {
                        // true: clone data
                        if (itemHead.IsExist == true)
                        {
                            itemExistD = new MaterialBranch
                            {
                                MaterialBranchId = Guid.NewGuid(),
                                BranchId = itemHead.BranchId,
                                MaterialId = itemHead.MaterialId,
                                CreatedBy = userId,
                                CreatedDate = DateTime.Now,
                                UpdatedBy = userId,
                                UpdatedDate = DateTime.Now,
                                IsDeleted = !itemHead.IsExist
                            };


                            var mate = this.appContext.Material.FirstOrDefault(p => p.MaterialId == itemHead.MaterialId);
                            if (mate != null)
                            {

                                // clone grpMaterial
                                var grpIdNew = Guid.NewGuid();
                                var typeMaterialIdNew = Guid.NewGuid();
                                var grp = this.appContext.GrpMaterial.FirstOrDefault(p => p.GrpMaterialId == mate.GrpMaterialId);
                                var grpNew = new GrpMaterial
                                {
                                    GrpMaterialId = grpIdNew,
                                    GrpMaterialCode = grp.GrpMaterialCode,
                                    BranchId = itemHead.BranchId,
                                    GrpMaterialName = grp.GrpMaterialName,
                                    Note = grp.Note,
                                    CreatedBy = userId,
                                    UpdatedBy = userId,
                                    TypeMaterialId = typeMaterialIdNew,
                                    CreatedDate = DateTime.Now,
                                    UpdatedDate = DateTime.Now
                                };
                                await appContext.GrpMaterial.AddAsync(grpNew);
                                // clone typeMaterial
                                var type = this.appContext.TypeMaterial.FirstOrDefault(p => p.TypeMaterialId == grp.TypeMaterialId);

                                var typeNew = new TypeMaterial
                                {
                                    TypeMaterialId = typeMaterialIdNew,
                                    TypeMaterialCode = type.TypeMaterialCode,
                                    BranchId = itemHead.BranchId,
                                    TypeMaterialName = type.TypeMaterialName,
                                    Note = type.Note,
                                    CreatedBy = userId,
                                    UpdatedBy = userId,
                                    CreatedDate = DateTime.Now,
                                    UpdatedDate = DateTime.Now,
                                    IsLearning = true,
                                    IsDocument = type.IsDocument,
                                    IsTest = type.IsTest,
                                };
                                await appContext.TypeMaterial.AddAsync(typeNew);
                                // clone material
                                var materialIdNew = Guid.NewGuid();
                                var mateNew = new Material
                                {
                                    MaterialId = materialIdNew,
                                    MaterialCode = mate.MaterialCode,
                                    GrpMaterialId = grpIdNew,
                                    MaterialName = mate.MaterialName,
                                    BranchId = itemHead.BranchId,
                                    Note = mate.Note,
                                    CreatedBy = userId,
                                    UpdatedBy = userId,
                                    CreatedDate = DateTime.Now,
                                    UpdatedDate = DateTime.Now
                                };
                                await appContext.Material.AddAsync(mateNew);
                                // clone material
                                var mateH = this.appContext.MaterialHead.FirstOrDefault(p => p.MaterialId == mate.MaterialId);
                                var mateHNew = new MaterialHead
                                {
                                    MaterialHeadId = Guid.NewGuid(),
                                    MaterialId = materialIdNew,
                                    SellPrice = mateH.SellPrice,
                                    SellPriceAfterVAT = mateH.SellPriceAfterVAT,
                                    Price = mateH.Price,
                                    VAT = 0,
                                    PriceAfterVAT = mateH.PriceAfterVAT,
                                    WholePrice = mateH.WholePrice,
                                    WholePriceAfterVAT = mateH.WholePriceAfterVAT,
                                    BranchId = Guid.NewGuid(),
                                    CalBeCourse = mateH.CalBeCourse,
                                    CalEnCourse = mateH.CalEnCourse,
                                    SpBeCourse = mateH.SpBeCourse,
                                    SpEnCourse = mateH.SpEnCourse,
                                    NumberHourse = mateH.NumberHourse,
                                    DocumentId = mateH.DocumentId,
                                    MaskPassCourse = mateH.MaskPassCourse,
                                    CreatedDate = DateTime.Now,
                                    UpdatedDate = DateTime.Now,
                                    CreatedBy = userId,
                                    UpdatedBy = userId,
                                };
                                await appContext.Material.AddAsync(mateNew);
                            }
                            await appContext.MaterialBranch.AddAsync(itemExistD);
                        }
                    }
                }
                return await appContext.SaveChangesAsync() > 0;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MaterialHead> Save(MaterialHead value, Guid? index)
        {
            var result = await appContext.MaterialHead.AddAsync(value);
            //save material
            await appContext.SaveChangesAsync();
            //
            return result.Entity;
        }

        public async Task<MaterialHead> FindHeadByMaterialId(Guid guid)
        {
            return await this.appContext.MaterialHead.FirstOrDefaultAsync(p => p.MaterialId == guid);
        }

        public async Task<bool> Delete(string id)
        {
            var itemExist = appContext.Material.FirstOrDefault(p => p.MaterialId.Equals(new Guid(id)));
            if (itemExist != null)
            {
                itemExist.IsDeleted = true;
            }
            await appContext.SaveChangesAsync();
            return true;
        }

        private ApplicationDbContext appContext
        {
            get { return (ApplicationDbContext)_context; }
        }
    }
}
