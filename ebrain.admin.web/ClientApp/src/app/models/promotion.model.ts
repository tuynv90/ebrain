// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

import { Unit } from "./unit.model";
import { File } from './file.model';
import { PromotionDetail } from "./promotiondetail.model";
import { IsPresentGuid } from "../directives/equal-validator.directive";
export class Promotion {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(promotionId?: string) {
    }

    public id: string;
    public promotionId: string;
    public promotionCode: string;
    public promotionName: string;
    public fromDate: Date;
    public createdDate: Date;
    public createdBy: string;
    public toDate: Date;
    public isOff: boolean;
    public branchId: string;
    public createdByApproved: string;
    public createdDateApproved: string;
    public fullNameStudent: string;
    public isApproved: boolean;
    public fullNameCreate: string;
    public countPromotion : number;
    public totalPrice : number;
    public ioNumber: string;
    public ioStockId: string;
    public details: PromotionDetail[];
}