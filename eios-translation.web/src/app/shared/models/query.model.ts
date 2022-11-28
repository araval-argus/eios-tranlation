export interface Query {
    rules: Rule[];
    groups: any[];
    operator: string;
    start: number;
    limit: number;
    sorts: Sort[];
    page: number;
    aggregations: Aggregation[];
    types: string[];
}
export interface Rule {
    name: string;
    operator: string;
    property: string;
    value: string | string[];
}
export interface Sort {
    property: string;
    direction: string;
}
export interface Aggregation {
    name: string;
    property: string;
    type: string;
    filter: Filter;
}
export interface Filter {
    operator: string;
    groups: any[];
    rules: Rule[];
}