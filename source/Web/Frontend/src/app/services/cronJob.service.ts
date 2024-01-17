import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, switchMap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { FilterObject } from "../models/filter.model";
import { CommonService } from "./common.service";
import { CronJob } from "../models/cronJob.model";

@Injectable()
export class CronJobService extends CommonService {

    private url: string = "api/cronjobs";
    elementos: any[] = [];

    constructor(
        private readonly _http: HttpClient,
    ) {
        super();
    }

    resolve(): Observable<any> | any {
        return this.list().pipe(catchError(() => {
            return of(null);
        }))
    }

    get(id: number): Observable<any> {
        return this._http.get(this.url + '/' + id).pipe(
            switchMap((response: any) => {
                return of(new CronJob(response));
            }));
    }

    list(filters?: FilterObject): Observable<any> {

        this.isLoading = true;
        let params = this.generateFilters(filters || this.filters);

        return this._http.get(this.url + '/list', { params }).pipe(
            switchMap((response: any) => {
                this.updateFilters(filters || this.filters, response);

                this.elementos = response.data;
                this.onElementosChanged.next(response.data);

                this.isLoading = false;
                return of(response);
            }))
    }

    create(elemento: CronJob): Observable<number> {
        return this._http.post(this.url, elemento).pipe(
            switchMap((response: any) => {
                return of(response);
            }))
    }

    update(elemento: CronJob): Observable<any> {
        return this._http.put(this.url, elemento).pipe(
            switchMap((response: any) => {
                return of(response);
            }))
    }

    delete(id: number): Observable<any> {
        return this._http.delete(this.url +'/' + id).pipe(
            switchMap((response: any) => {
                return of(response);
            }))
    }

    generateFilters(filters?: FilterObject) {
        let params = new HttpParams();
        if (filters?.pageIndex) { params = params.set('pageIndex', (filters?.pageIndex + 1).toString()) }
        if (filters?.pageSize) { params = params.set('pageSize', filters?.pageSize.toString()) }
        return params
    }

    updateFilters(filters: FilterObject, response: any) {
        if (filters) {
            filters.updateTableLength(response.totalRecords);
        } else {
            this.filters.updateTableLength(response.totalRecords);
            if (response.data.length == 0) { this.filters.firstPageIndex(); }
        }
    }
}
