// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================
export class PromotionDetail {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(promotionDetailId?: string) {
    }

    public promotionDetailId : string;
    public promotionId : string;
    public materialId : string;
    public priceOrigion : number;
    public percentDiscount : number;
    public moneyDiscount : number;
    public totalPrice : number;
    public materialCode : string;
    public materialName : string;

    public promotionCode : string;
    public promotionName : string;
}