import { Injectable } from "@angular/core";
import { BehaviorSubject } from 'rxjs';

import { FilterObject } from "../models/filter.model";

@Injectable()
export class CommonService {
    onElementosChanged: BehaviorSubject<any> = new BehaviorSubject([]);
    onElementoChanged: BehaviorSubject<any> = new BehaviorSubject([]);

    isLoading: boolean = false;

    filters: FilterObject = new FilterObject();
}
