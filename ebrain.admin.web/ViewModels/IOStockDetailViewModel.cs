﻿// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using ebrain.admin.bc.Models;

namespace Ebrain.ViewModels
{
    public class IOStockDetailSmallViewModel
    {
        public Guid? IOStockId { get; set; }
        public Guid? IOStockDetailId { get; set; }
        public decimal? InputExport { get; set; }
    }
    public class IOStockDetailViewModel
    {
        public Guid? ID { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string GrpName { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }

        public string TypeMaterial { get; set; }
        public string GrpMaterial { get; set; }
        public Guid UnitId { get; set; }
        public Guid TypeMaterialId { get; set; }
        public Guid? MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public Guid GrpMaterialId { get; set; }
        public Guid SupplierId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? SellPrice { get; set; }
        public decimal? DisCountMoney { get; set; }
        public decimal? TotalPrice { get; set; }
        public Guid? PurchaseOrderDetailId { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public IOStockDetailPro[] Pros { get; set; }
    }
}
