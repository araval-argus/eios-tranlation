
import { Injectable } from "@angular/core";
import { TranslocoService } from "@ngneat/transloco";
import { FilterItem } from "./../models/filter-item.model";

@Injectable()
export class FiltersTranslationsService{

 constructor(
    private translocoService: TranslocoService,
 ) {}

 getTranslations(filtersID: string): string[]{

    let filtersTranslations: string[] = [];

    switch(filtersID){
        case 'filtersActivities':
            const filtersActivitiesTranslations = this.translocoService.translateObject('filtersActivities.labels');
            filtersTranslations = filtersActivitiesTranslations;    
        break;
        case 'filtersArchives':
            const filtersArchives = this.translocoService.translateObject('filtersArchives.labels');
            filtersTranslations = filtersArchives;    
        break;
    }

    return filtersTranslations;
 }

 getLabel(filterItem: FilterItem, filtersTranslation: string[]): string{
    let itemLabel: string;
    Object.keys(filtersTranslation).forEach(key => {
        if(filterItem.label === key){
            itemLabel = filtersTranslation[key];
        }
    });
    return itemLabel;
  }

}