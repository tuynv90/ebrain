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
    public class IOStockViewModel
    {
        public Guid? ID { get; set; }
        public string Code { get; set; }
        public Guid? SupplierId { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string Note { get; set; }
        public decimal QuantityInput { get; set; }
        public decimal QuantityOutput { get; set; }
        public decimal Quantity { get; set; }
        public decimal SellPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public IOStockDetailViewModel[] IODetails { get; set; }
        
        public Guid? StudentId { get; set; }
        public Guid IOStockDetailId { get; set; }
        public Guid MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public decimal? InputQuantity { get; set; }
        public long IOTypeId { get; set; }
        public string FullName { get; set; }
        public string StudentName { get; set; }
        public decimal TotalPricePayment { get; set; }
        public decimal TotalPriceExist { get; set; }
        public string PurchaseOrderCode { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public string BranchName { get; set; }
        public string FullNameExport { get; set; }
        public decimal? InputExport { get; set; }
        public DateTime? DateExport { get; set; }
        public Guid? ByExport { get; set; }
        public string CreateDate_MMYY
        {
            get
            {
                return CreateDate.ToString("dd/MM");
            }
        }
        public string UpdatedByName { get; set; }
        public string Html { get; set; }
    }
}
