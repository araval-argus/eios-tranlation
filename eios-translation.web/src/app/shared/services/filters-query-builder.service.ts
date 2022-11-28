import { Query } from '../models/query.model';
import { Injectable } from "@angular/core";

@Injectable()
export class FiltersQueryBuilderService{

    getQuery(filterName: string): Query {

        let queryPost = {} as Query;

        switch(filterName){
            case 'filtersActivities':
                queryPost = {
                    rules: [],
                    groups: [],
                    operator: "and",
                    start: 0,
                    limit: 0,
                    sorts: [
                    ],
                    page: 0,
                    aggregations: [
                        {
                            name: filterName,
                            property: "entityType",
                            type: "terms"
                        }
                    ],
                    types: [
                        "createquerycommentactivity",
                        "createqueryactivity",
                        "updatequeryactivity",
                        "restorequeryactivity",
                        "pinitemtoqueryactivity"
                    ]
                } as Query;
            break;
            case 'filterTeams':
                queryPost = {
                    rules: [],
                    groups: [],
                    operator: "and",
                    start: 0,
                    limit: 0,
                    sorts: [],
                    page: 0,
                    aggregations: [
                        {
                            name: filterName,
                            property: "roleId",
                            type: "terms"
                        }
                    ],
                    types: [
                    ]
                } as Query;
            break;
            case 'filtersArchives':
                queryPost = {
                    rules: [],
                    groups: [],
                    operator: "and",
                    start: 0,
                    limit: 0,
                    aggregations: [
                        {
                            name: filterName,
                            property: "isDeleted",
                            type: "terms"
                        }
                    ],
                    sorts: []
                } as Query
            break
        }

        return queryPost;
    }
    
}