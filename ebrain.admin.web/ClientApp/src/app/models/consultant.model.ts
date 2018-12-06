// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

export class Consultant {
    constructor(consultantId?: string) {
        this.id = consultantId;
    }
    public id: string;
    public name: string;
    public code: string;
    public note: string;
}

export class ConsultantContact {
    constructor(consultantId?: string) {
        this.consultantContactId = consultantId;
    }

    public consultantContactId: string;
    public consultantContactCode: string;
    public consultantContactName: string;
    public phoneContact: string;
    public contactName: string;
    public branchId: string;
    public scheduleStartDate: Date;
    public scheduleEndDate: string;
    public scheduleStartTime: Date;
    public scheduleEndTime: string;
    public scheduleNote: string;
    public isConfirm: boolean;
    public createdBy: string;
    public createdDate: Date;

}