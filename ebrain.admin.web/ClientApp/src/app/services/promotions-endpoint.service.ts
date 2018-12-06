// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

import { Injectable, Injector } from '@angular/core';
import { Http, Headers, RequestOptions, Response, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';

import { EndpointFactory } from './endpoint-factory.service';
import { ConfigurationService } from './configuration.service';
import { Student } from '../models/student.model';


@Injectable()
export class PromotionsEndpoint extends EndpointFactory {

    private readonly _serviceUrl: string = "/api/promotion";

    private get serviceUrl() { return this.configurations.baseUrl + this._serviceUrl; }

    constructor(http: Http, configurations: ConfigurationService, injector: Injector) {
        super(http, configurations, injector);
    }

    getPromotionList(filter: string, value: string, fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = this.getUrl('getpromotionlist?value=' + value 
            + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getPromotionList(filter, value, fromDate, toDate, page, size));
            });
    }

    getPromotionListReportIODetail(promotionId: string, page: number, size: number): Observable<Response> {

        let url = this.getUrl('getpromotionlistreportiodetail?promotionId=' + promotionId + '&size=' + size + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getPromotionListReportIODetail(promotionId, page, size));
            });
    }

    getPromotionListReport(filter: string, value: string, fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = this.getUrl('getpromotionlistreport?value=' + value 
            + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getPromotionListReport(filter, value, fromDate, toDate, page, size));
            });
    }

    getPromotionListDetail(filter: string, value: string, fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = this.getUrl('getpromotionlistdetail?value=' + value 
            + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getPromotionListDetail(filter, value, fromDate, toDate, page, size));
            });
    }

    getDefault(index: string, isClone: number): Observable<Response> {

        let url = this.getUrl('getdefault?index=' + index 
            + '&isClone=' + isClone  + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getDefault(index, isClone));
            });
    }

    save(value: any): Observable<Response> {
        let url = this.getUrl('update');
        let header = this.getAuthHeader(true);
        let params = JSON.stringify(value);

        return this.http.post(url, params, header)
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.save(value));
            });
    }

    saveApproved(value: any): Observable<Response> {
        let url = this.getUrl('updateapproved');
        let header = this.getAuthHeader(true);
        let params = JSON.stringify(value);

        return this.http.post(url, params, header)
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.saveApproved(value));
            });
    }


    deletemaster(value: any): Observable<Response> {
        let url = this.getUrl('deletemaster');
        let header = this.getAuthHeader(true);
        let params = JSON.stringify(value);

        return this.http.post(url, params, header)
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.deletemaster(value));
            });
    }

    protected handleError(error, continuation: () => Observable<any>) {

        if (error.status == 401) {

        }

        if (error.url && error.url.toLowerCase().includes(this.serviceUrl.toLowerCase())) {
            return Observable.throw('session expired');
        }
        else {
            return Observable.throw(error || 'server error');
        }
    }

    private getUrl(prefix: string): string {
        return this.serviceUrl + '/' + prefix;
    }
}