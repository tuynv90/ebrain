// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================


export class IOStockDetailPro {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(ioid?: string) {
        this.id = ioid;
    }
    public id: string;
    public ioStockDetailProId: string;
    public ioStockDetailId: string;
    public ioStockId: string;
    public promotionDetailId: string;
    public promotionId: string;
    public percentDiscount: number;
    public moneyDiscount: number;
    public promotionCode: string;
    public promotionName: string;
}