import { FilterGroupSetup } from './../models/filters-group-setup.model';
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { map, catchError, exhaustMap } from 'rxjs/operators';
import { merge, of } from 'rxjs';
//Actions
import * as FiltersActions from './filters.actions';
//Services
import { FiltersEndPointService } from './../services/filters-end-point.service';
import { FiltersTranslationsService } from './../services/filters-translations.service'
import { FiltersQueryBuilderService } from './../services/filters-query-builder.service';
@Injectable()
export class FiltersEffects{

    constructor(
        private actions$: Actions,
        private filtersEndPointService: FiltersEndPointService,
        private filtersTranslationsService: FiltersTranslationsService,
        private queryBuilderService: FiltersQueryBuilderService
    ){}
    load$ = createEffect(() => this.actions$.pipe(
            ofType(FiltersActions.load),
            exhaustMap((action) => 
                merge(
                    ...action.filtersSetup.map( (filtersGroupSetup: FilterGroupSetup) => {
                            const queryObj = this.queryBuilderService.getQuery(filtersGroupSetup.id);
                            return this.filtersEndPointService.loadFilterstPost(filtersGroupSetup.id, queryObj)
                                .pipe(
                                    map(res => {
                                            if(filtersGroupSetup.isTranslationRequired === true){
                                                return FiltersActions.translationsLoad({ filtersResult: res, filtersGroupSetup, query: queryObj})
                                            }else{
                                                return FiltersActions.loadSuccess({ filtersResult: res, filtersGroupSetup, query: queryObj})
                                            }
                                        }
                                    ),
                                    catchError(() => of(FiltersActions.translationsLoadFailed()))
                            )
                        }
                    )
                )
            )
        ),
    );
    translationsLoad$ = createEffect( () => this.actions$.pipe(
        ofType(FiltersActions.translationsLoad),
        map(action => {
            const translations = this.filtersTranslationsService.getTranslations(action.filtersGroupSetup.id);
            return FiltersActions.translationsLoadSuccess({ filtersResult: action.filtersResult, filtersGroupSetup: action.filtersGroupSetup, query: action.query, translations: translations});
        }),
        catchError((err) => {
            return of(FiltersActions.translationsLoadFailed())
        })
    ));
    translationUpdate$ = createEffect( () => this.actions$.pipe(
        ofType(FiltersActions.translationsUpdate),
        map(action => {
            const translations = this.filtersTranslationsService.getTranslations(action.filtersGroup.id);
            return FiltersActions.translationsUpdateSuccess({ filtersGroup: action.filtersGroup, translations: translations});
        }),
        catchError((err) => {
            return of(FiltersActions.translationsUpdateFailed())
        })
    ));

    dataLookUp$ = createEffect(() => this.actions$.pipe(
        ofType(FiltersActions.itemClick),
        map((test) => {
            console.log('test dataLookUP', test)
            return FiltersActions.dataLookUp();
        })
    ));

}