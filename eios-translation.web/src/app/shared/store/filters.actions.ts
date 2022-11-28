import { FilterGroupSetup } from './../models/filters-group-setup.model';


import { createAction, props } from "@ngrx/store";
//Models
import { Query } from "./../models/query.model";
import { FiltersGroup } from './../models/filters-group.model';
import { FiltersResult } from './../models/filters-result.model';
import { FilterItem } from './../models/filter-item.model';

//Filters - Load
export const load = createAction(
    '[Filters] - Load',
    props<{filtersSetup: FilterGroupSetup[]}>()
);
//Filters - Load Success
export const loadSuccess = createAction(
    '[Filters] - Load Success',
    props<{filtersResult: FiltersResult, filtersGroupSetup: FilterGroupSetup, query: Query}>()
);
//Filters - Load Failed
export const loadFailed = createAction(
    '[Filters] - Load Failed',
    props<{filtersID: string, query: Query}>()
);


//Filters - Translations Load
export const translationsLoad = createAction(
    '[Filters] - Load Translation',
    props<{filtersResult: FiltersResult, filtersGroupSetup: FilterGroupSetup, query: Query}>()
);
//Filters - Translation Load Success
export const translationsLoadSuccess = createAction(
    '[Filters] - Load Translation Success',
    props<{filtersResult: FiltersResult, filtersGroupSetup: FilterGroupSetup, query: Query, translations: string[]}>()
);
//Filters - Translations Load Failed
export const translationsLoadFailed = createAction(
    '[Filters] - Load Translation Failed'
);


//Filters - Translations Update
export const translationsUpdate = createAction(
    '[Filters] - Translation Update',
    props<{filtersGroup: FiltersGroup}>()
);
//Filters - Translations Update Success
export const translationsUpdateSuccess = createAction(
    '[Filters] - Translation Update Success',
    props<{filtersGroup: FiltersGroup, translations: string[]}>()
);
//Filters - Translations Update Failed
export const translationsUpdateFailed = createAction(
    '[Filters] - Translation Update Failed'
);


//Filter Item Click
export const itemClick = createAction(
    '[Filter] - Item Click',
    props<{ filterItem: FilterItem, filtersGroup: FiltersGroup }>()
);

export const dateTimeSelection = createAction(
    '[Filter] - Datetime Selection',
    props<{ dateFrom: string, dateTo: string }>()
);


//Filters Select All
export const selectAllFilters = createAction(
    '[Filters] Select All',
    props<{ filtersGroup: FiltersGroup }>()
);
//Filters Delesect All
export const deselectAllFilters = createAction(
    '[Filters] Deselet All',
    props<{ filtersGroup: FiltersGroup }>()
);


//Filter Search
export const searchFilter = createAction(
    '[Filters] Search',
    props<{searchKey: string, filtersGroup: FiltersGroup }>()
);
//Filter Delete Search
export const searchDelete = createAction(
    '[Filters] Search Delete',
    props<{filtersGroup: FiltersGroup }>()
);


export const dataLookUp = createAction(
    '[Filters] DataLookUp',
)