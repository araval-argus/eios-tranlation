import { Action, createReducer, on } from "@ngrx/store";
//Actions
import * as FiltersActions from './filters.actions';
//Models
import { FiltersGroup } from './../models/filters-group.model';
import { FilterItem } from './../models/filter-item.model';
import { FiltersResult } from './../models/filters-result.model';

export const initialState = [] as FiltersGroup[];

const FiltersReducer = createReducer(
    //InitialState
    initialState,
    //Load Filters Success
    on(FiltersActions.loadSuccess, (state, action) => {
        const newFilters =  action.filtersResult.aggregations[action.filtersGroupSetup.id].map((filter: FilterItem) => {
            return{
                ...filter,
                selected: false,
                visible: true
            }
        });
        //New-Filters-Order
        const filtersOnDefOrder = newFilters.sort((a, b) => (a.count > b.count) ? -1 : 1);
        //New-Filters-Result
        let newfiltersResult = action.filtersResult;
        newfiltersResult = {
            ...newfiltersResult,
            aggregations:  {
                [action.filtersGroupSetup.id]: filtersOnDefOrder
            } 
        };
        const newState = [
            ...state,
            {
                id: action.filtersGroupSetup.id,
                operator: action.filtersGroupSetup.operator,
                property: action.filtersGroupSetup.property,
                filtersResult: newfiltersResult,
                filtersQuery: action.query,
                joinQueryRuleTemplate: action.filtersGroupSetup.joinQueryRuleTemplate,
                isTranslationRequired: action.filtersGroupSetup.isTranslationRequired,
                isTranslationFrontend: action.filtersGroupSetup.isTranslationFrontend,
                error: false
            } as FiltersGroup
        ];

        return newState;
    }),
    //Load Filters Failed
    on(FiltersActions.loadFailed, (state, action) => {
        return [
            ...state,
            {
                id: action.filtersID,
                filtersResult: {} as FiltersResult,
                filtersQuery: action.query
            } as FiltersGroup
        ];
    }),
    //Translation Load Success
    on(FiltersActions.translationsLoadSuccess, (state, action) => {
        const newFilters =  action.filtersResult.aggregations[action.filtersGroupSetup.id]?.map((filter: FilterItem) => {
            let labelTranslation: string = "";
            Object.keys(action.translations).forEach(key => {
                if(filter.label === key){
                    labelTranslation = action.translations[key];
                }
            });
            return{
                ...filter,
                selected: false,
                visible: true,
                labelTraslated: labelTranslation
            }
        });
        //New-Filters-Order
        let filtersOnDefOrder = newFilters.sort((a, b) => (a.count > b.count) ? -1 : 1);
        //New-Filters-Result
        let newfiltersResult = {...action.filtersResult};
        newfiltersResult = {
            ...newfiltersResult,
            aggregations:  {
                [action.filtersGroupSetup.id]: [...filtersOnDefOrder]
            } 
        };
        const newState = [
            ...state,
            {
                id: action.filtersGroupSetup.id,
                operator: action.filtersGroupSetup.operator,
                property: action.filtersGroupSetup.property,
                filtersResult: newfiltersResult,
                filtersQuery: action.query,
                joinQueryRuleTemplate: action.filtersGroupSetup.joinQueryRuleTemplate,
                isTranslationRequired: action.filtersGroupSetup.isTranslationRequired,
                isTranslationFrontend: action.filtersGroupSetup.isTranslationFrontend,
                error: false
            } as FiltersGroup
        ];
        return newState;
    }),
    on(FiltersActions.translationsUpdateSuccess, (state, action) => {
        const newFilters =  action.filtersGroup.filtersResult.aggregations[action.filtersGroup.id].map((filter: FilterItem) => {
            let labelTranslation: string = "";
            Object.keys(action.translations).forEach(key => {
                if(filter.label === key){
                    labelTranslation = action.translations[key];
                }
            });
            return{
                ...filter,
                labelTraslated: labelTranslation
            }
        });
        let newfiltersResult = {
            ...action.filtersGroup.filtersResult,
            aggregations:  {
                [action.filtersGroup.id]: newFilters
            } 
        };
        let newFiltersGroup = {
            ...action.filtersGroup,
            filtersResult: newfiltersResult
        };
        let newState = [...state].filter((filterGroup: FiltersGroup) => filterGroup !== action.filtersGroup);
        newState = [
            ...newState,
            newFiltersGroup
        ]
        return newState;
    }),


    //Filter Item Click
    on(FiltersActions.itemClick, (state, action) => {
        let newFiltersGroup: FiltersGroup[] = [...state];
        newFiltersGroup = newFiltersGroup.map((filtersGroup: FiltersGroup) => { 
            let newFiltersGroup: FiltersGroup = {...filtersGroup};
            if(newFiltersGroup.id === action.filtersGroup.id) {
                let newFilters: FilterItem[] = [...filtersGroup.filtersResult.aggregations[action.filtersGroup.id]];
                newFilters = newFilters.map((filterItem: FilterItem) => {
                    let newFilterItem: FilterItem = {...filterItem};
                    return newFilterItem.label === action.filterItem.label ? { ...newFilterItem, selected: !newFilterItem.selected} : newFilterItem;
                 }).sort(function(a, b) {
                     if(newFiltersGroup.searchKey){
                         const rgx = new RegExp(newFiltersGroup.searchKey, 'gi');
                         return ( (+b.label.match(rgx)) - (+a.label.match(rgx)) )
                         
                     }else{
                         return (+b.selected) - (+a.selected) || (b.count - a.count)
                     }
                 });

                newFiltersGroup = {
                    ...newFiltersGroup,
                    filtersResult: {
                        ...newFiltersGroup.filtersResult,
                        aggregations: {
                            [action.filtersGroup.id]: [...newFilters]
                        }
                    }
                }

                return newFiltersGroup;
            } else {
                return newFiltersGroup;
            }
        });
        return newFiltersGroup
    }),
    //Filters Select All
    on(FiltersActions.selectAllFilters, (state, action) => {
        let newFiltersGroupArray = [...state];
        newFiltersGroupArray = newFiltersGroupArray.map((filtersGroup: FiltersGroup) => {
            let newFilterGroup: FiltersGroup = { ...filtersGroup };
            if(newFilterGroup.id === action.filtersGroup.id){
                let newFilters: FilterItem[] = [...newFilterGroup.filtersResult.aggregations[action.filtersGroup.id]];
                newFilters = newFilters.map((filterItem: FilterItem) => {
                    let newFilterItem = {...filterItem}
                    newFilterItem.selected = true;
                    return newFilterItem;
                });
                newFilterGroup = {
                    ...newFilterGroup,
                    filtersResult: {
                        ...filtersGroup.filtersResult,
                        aggregations: {
                            [action.filtersGroup.id]: [...newFilters]
                        }
                    }
                };
                return newFilterGroup;
            }else{
                return {
                    ...filtersGroup
                }
            }
        });
        return newFiltersGroupArray;
    }),
    //Filters Deselect All
    on(FiltersActions.deselectAllFilters, (state, action) => {
        let newFiltersGroupArray = [...state];
        newFiltersGroupArray = newFiltersGroupArray.map((filtersGroup: FiltersGroup) => {
            let newFiltersGroup = {...filtersGroup}
            if(newFiltersGroup.id === action.filtersGroup.id){
                let newFiltersArray: FilterItem[] = [...newFiltersGroup.filtersResult.aggregations[action.filtersGroup.id]];
                newFiltersArray = newFiltersArray.map((filterItem: FilterItem) => {
                    let newFilterItem = {
                        ...filterItem,
                        selected: false
                    };
                    return newFilterItem;
                }).sort((a, b) => (b.count - a.count));

                newFiltersGroup = {
                    ...newFiltersGroup,
                    filtersResult: {
                        ...newFiltersGroup.filtersResult,
                        aggregations: {
                            [action.filtersGroup.id]: [...newFiltersArray]
                        }
                    }
                }
                return newFiltersGroup
            }else{
                return {
                    ...newFiltersGroup
                }
            }
        });
        return newFiltersGroupArray;
    
    }),

    //Filters Search
    on(FiltersActions.searchFilter, (state, action) => {
        const rgx = new RegExp(action.searchKey, 'gi');
        let newFiltersGroupArray = [...state];
        newFiltersGroupArray = newFiltersGroupArray = newFiltersGroupArray.map((filtersGroup: FiltersGroup) => {
            let newFilterGroup: FiltersGroup = { ...filtersGroup };
            if(newFilterGroup.id === action.filtersGroup.id){
                //FiltersArray
                let newFiltersArray: FilterItem[] = [...newFilterGroup.filtersResult.aggregations[action.filtersGroup.id]];
                newFiltersArray = newFiltersArray.map((filterItem: FilterItem) => {
                    let newFilter = {...filterItem};
                    if(newFilter.selected === false){
                        if(newFilterGroup.isTranslationFrontend === true){
                            if(newFilter.labelTraslated.match(rgx)){
                                return { 
                                    ...newFilter, 
                                    visible: true 
                                };
                            }else{
                                return { 
                                    ...newFilter, 
                                    visible: false 
                                };
                            }
                        }else{
                            if(newFilter.label.match(rgx)){
                                return { 
                                    ...newFilter, 
                                    visible: true 
                                };
                            }else{
                                return { 
                                    ...newFilter, 
                                    visible: false 
                                };
                            }
                        }
                    }else{
                        return newFilter;
                    }
                }).sort(function(a, b) {
                    return (+a.selected) - (+b.selected) || (b.count - a.count)
                })
                newFilterGroup = {
                    ...newFilterGroup,
                    filtersResult: {
                        ...newFilterGroup.filtersResult,
                        aggregations: {
                            [action.filtersGroup.id]: [...newFiltersArray]
                        }
                    }
                }
                return newFilterGroup;

            }else{
                return newFilterGroup;
            }
        });
        return newFiltersGroupArray;
    }),
    //Filters Search Delete
    on(FiltersActions.searchDelete, (state, action) => {
        let newFiltersGroupArray = [...state];
        newFiltersGroupArray = newFiltersGroupArray.map((filtersGroup: FiltersGroup) => {
            let newFiltersGroup = { ...filtersGroup};
            if(newFiltersGroup.id === action.filtersGroup.id){
                let newFiltersArray: FilterItem[] = [...newFiltersGroup.filtersResult.aggregations[action.filtersGroup.id]];
                newFiltersArray = newFiltersArray.map((filterItem: FilterItem) => {
                    let newFilterItem = {
                        ...filterItem,
                        visible: true
                    };
                    return newFilterItem;
                }).sort(function(a, b) {
                    return (+b.selected) - (+a.selected) || (b.count - a.count)
                });

                newFiltersGroup = {
                    ...newFiltersGroup,
                    searchKey: null,
                    filtersResult: {
                        ...newFiltersGroup.filtersResult,
                        aggregations: {
                            [action.filtersGroup.id]: [...newFiltersArray]
                        }
                    }
                }
                return newFiltersGroup;
            }else{
                return newFiltersGroup;
            }
        });

        return newFiltersGroupArray;
    }),

    //Filters DateTime Selection
    on(FiltersActions.dateTimeSelection, (state, action) => {
        let newFiltersGroupArray = [...state];

        console.log('Filters DateTime Selection')

        return newFiltersGroupArray;
    })
)

export function reducer(state: FiltersGroup[] | undefined, action?: Action){
    return FiltersReducer(state, action)
}