// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

export class ExamineMaterial {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(id?: string) {
    }

    public examineMaterialId: string;
    public branchId: string;
    public examineId: string;
    public materialId: string;
    public parentExamineId: string;
    public examineCode: string;
    public examineName: string;
    public isExist: boolean;
    public percentMark: number;
}