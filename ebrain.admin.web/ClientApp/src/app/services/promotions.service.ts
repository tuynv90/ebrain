// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

import { Injectable } from '@angular/core';
import { Router, NavigationExtras } from "@angular/router";
import { Response } from '@angular/http';
import 'rxjs/add/operator/map';
import { ConfigurationService } from './configuration.service';
import { IOStock } from "../models/iostock.model";
import { MaterialsEndpoint } from "./materials-endpoint.service";
import { PromotionsEndpoint } from "./promotions-endpoint.service";
import { Results } from "../models/results.model";
import { Promotion } from '../models/promotion.model';

@Injectable()
export class PromotionsService {

    constructor(private router: Router, private configurations: ConfigurationService, 
        private endpointFactory: PromotionsEndpoint,
        private materialendpoint: MaterialsEndpoint) {
        this.initializeStatus();
    }
    getPromotionListReportIODetail(promotionId: string, page: number, size: number) {
        return this.endpointFactory.getPromotionListReportIODetail(promotionId, page, size)
            .map((response: Response) => <Results<Promotion>>response.json());
    }
    getPromotionListReport(filter: string, value: string, fromDate: Date, toDate: Date, page: number, size: number) {
        return this.endpointFactory.getPromotionListReport(filter, value, fromDate, toDate, page, size)
            .map((response: Response) => <Results<Promotion>>response.json());
    }

    getPromotionList(filter: string, value: string, fromDate: Date, toDate: Date, page: number, size: number) {
        return this.endpointFactory.getPromotionList(filter, value, fromDate, toDate, page, size)
            .map((response: Response) => <Results<Promotion>>response.json());
    }

    getPromotionListDetail(filter: string, value: string, fromDate: Date, toDate: Date, page: number, size: number) {
        return this.endpointFactory.getPromotionListDetail(filter, value, fromDate, toDate, page, size)
            .map((response: Response) => <Results<Promotion>>response.json());
    }

    getDefault(index: string, isClone: number) {
        return this.endpointFactory.getDefault(index, isClone)
            .map((response: Response) => <Promotion>response.json());
    }

    save(value: Promotion) {
        return this.endpointFactory.save(value)
            .map((response: Response) => <Promotion>response.json());
    }

    saveApproved(value: Promotion) {
        return this.endpointFactory.saveApproved(value)
            .map((response: Response) => <Promotion>response.json());
    }

    deletemaster(index: string) {
        return this.endpointFactory.deletemaster(index)
            .map((response: Response) => <Promotion>response.json());
    }

    private initializeStatus() {

    }
}