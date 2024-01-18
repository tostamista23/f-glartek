import { Injectable } from "@angular/core";
import { HttpClient,  } from '@angular/common/http';
import { switchMap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable()
export class TimeZoneService  {

    private url: string = "api/timezones";

    constructor(
        private readonly _http: HttpClient,
    ) { }


    list(): Observable<any> {
        return this._http.get(this.url + '/list').pipe(
            switchMap((response: any) => {
                return of(response);
            }))
    }

    
}
