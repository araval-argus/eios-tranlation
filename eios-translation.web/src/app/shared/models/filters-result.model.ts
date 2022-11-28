import { FilterItem } from './filter-item.model';

export interface FiltersResult {
    Result: any[];
    Count: number;
    aggregations: Aggregations;
    IsCountAccurate: boolean;
}
export interface Aggregations {
}

