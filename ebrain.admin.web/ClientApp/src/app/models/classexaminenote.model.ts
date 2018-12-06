// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

export class ClassExamineNote {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(classId?: string) {
        this.classId = classId;
    }

    public classExamineNoteId: string;
    public classId: string;
    public examineId: string;
    public studentId: string;
    public examineNoteId: string;
    public examineNoteCode: string;
    public examineNoteName: string;
    public attend: string;
    public notAttend: string;
    public isSummarize: boolean;
}