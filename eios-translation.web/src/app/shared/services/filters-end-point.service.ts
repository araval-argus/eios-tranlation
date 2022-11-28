import { Observable } from 'rxjs';

import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment.prod';
//Models
import { Query } from "../models/query.model";
import { FilterItem } from '../models/filter-item.model';
import { FiltersResult } from "../models/filters-result.model";

@Injectable()
export class FiltersEndPointService{

    constructor(
        private http: HttpClient
    ){}

    loadFilterstPost(filterID: string, query: Query){
        let obsFiltersRes: Observable<FiltersResult>;
        switch(filterID){
            case 'filtersActivities':
                obsFiltersRes = this.http.post<FiltersResult>('/Service/FilterActivities', query);
            break;
            default:
                obsFiltersRes = this.http.post<FiltersResult>('/Service/FilterBoards?baseView=true', query);
            break;
        }
        return obsFiltersRes;
    }

}