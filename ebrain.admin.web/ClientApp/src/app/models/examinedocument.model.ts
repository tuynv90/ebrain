// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

export class ExamineDocument {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(id?: string) {
    }

    public examineMaterialId: string;
    public branchId: string;
    public examineId: string;
    public materialId: string;
    public documentId: string;
    public parentExamineId: string;
    public documentCode: string;
    public documentName: string;
    public isExist: boolean;

}