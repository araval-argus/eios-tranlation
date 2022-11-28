import { Component, Input, SimpleChanges, Output, EventEmitter, ChangeDetectionStrategy, OnChanges, AfterViewInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { trigger, style, state, transition, animate } from '@angular/animations';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter } from 'rxjs/operators';
import { NgForm } from '@angular/forms';
import { FiltersGroup } from './../../../shared/models/filters-group.model';
import { FilterItem } from './../../../shared/models/filter-item.model';

//Services
import { TranslocoService } from '@ngneat/transloco';

@Component({
  selector: 'eios-filters-group',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './filters-group.component.html',
  styleUrls: [
    './filters-group.component.css',
    './../../../../assets/css/skeleton.css'
  ],
  animations: [
    trigger('panelVisibility', [
      state('opened', style({
        height: '*'
      })),
      state('closed', style({
        height: '0',
        overflow: 'hidden'
      })),
      transition('opened <=> closed', [
        animate('0.4s cubic-bezier(0.55, 0, 1, 0.45)')
      ])
    ])
  ]
})
export class FiltersGroupComponent implements OnChanges, AfterViewInit{

  @Input() filtersGroupId: string;
  @Input() panelHeadName: string;
  @Input() groupHelpFeature: boolean = false;    
  //Search Properties
  @Input() searchFeature: boolean;
  @Input() searchPlaceHolder?: string;
  @Input() searchLabel?: string;
  @Input() searchLabelVisible?: boolean = false;
  @Input() isLoading: boolean = true;
  //TimeFrame Proprieties
  @Input() timeFrameFeature: boolean = false;  
  @Input() timeFrameFeatureFeedback?: string[] = [];
  //FiltersGroup
  @Input() filtersGroup: FiltersGroup;
  //Filter Scroll Properties
  @Input() showAllItems?: boolean = false; 
  @Input() showItemsNumb?: number = 0;
  //Filters Properties
  @Input() filterKeyBadge?: boolean = false;
  @Input() filterKeyRing?: boolean = false;
  @Input() filterCountryFlag?: boolean = false;
  @Input() filterQuestionMark?: boolean = false;
  @Input() filterKeyBracket?: boolean = false;
  //ScrollEvent
  @Input() scrollEvent?: string = null;
  //Events Emitters
  @Output() filtersSelectAllGroup: EventEmitter<{ filtersGroup: FiltersGroup}> = new EventEmitter<{ filtersGroup: FiltersGroup}>();
  @Output() filtersDeselectAllGroup: EventEmitter<{ filtersGroup: FiltersGroup}> = new EventEmitter<{ filtersGroup: FiltersGroup}>();
  @Output() filterItemClickGroup: EventEmitter<{filterItem: FilterItem, filtersGroup: FiltersGroup}> = new EventEmitter<{filterItem: FilterItem, filtersGroup: FiltersGroup}>();
  @Output() filterSearch: EventEmitter<{searchKey: string, filtersGroup: FiltersGroup}> = new EventEmitter<{searchKey: string, filtersGroup: FiltersGroup}>();
  @Output() filterSearchDelete: EventEmitter<{filtersGroup: FiltersGroup}> = new EventEmitter<{filtersGroup: FiltersGroup}>();
  @Output() filterDateInputAction: EventEmitter<{dateFrom: string, dateTo: string}> = new EventEmitter<{dateFrom: string, dateTo: string}>();

  @ViewChild('searchInput', {static: false}) input: ElementRef;

  itemsList: FilterItem[];
  itemsLenght: number = 0;
  itemsSteps: number = 0;
  itemsStepsList: number[] = [];
  itemsHeight: number = 0;
  itemsCount: number = 0;
  
  panelVisibility: string = 'opened';


  //Life-Cycle
  ngOnChanges(changes: SimpleChanges): void {
    if(changes.filtersGroup){
      const curentFiltersGroup: FiltersGroup = changes.filtersGroup.currentValue;
      let visibleItemsPresent = 0;
      if(curentFiltersGroup){
        visibleItemsPresent = curentFiltersGroup.filtersResult.aggregations[curentFiltersGroup.id]?.filter(item => item.visible).length;
        this.filtersUpdate(curentFiltersGroup);
      }else{
        visibleItemsPresent = 0;
      }
      this.updateScrollSpace(visibleItemsPresent);
    }
  }
  ngAfterViewInit(){
    if(this.searchFeature){
      fromEvent(this.input.nativeElement, 'keyup')
        .pipe(
          filter(Boolean),
          debounceTime(1000),
          distinctUntilChanged()
      ).subscribe(() => {
          const searchKey: string = this.input.nativeElement.value;
          if(searchKey){
            this.filterSearch.emit({searchKey, filtersGroup: this.filtersGroup});
          }
      });
    }
  }

  //Filters-Update
  filtersUpdate(curentFiltersGroup: FiltersGroup){
    this.itemsCount = Math.max.apply(Math, curentFiltersGroup.filtersResult.aggregations[curentFiltersGroup.id].map(function(o) { return o.count; }))
    this.itemsList = [...curentFiltersGroup.filtersResult.aggregations[curentFiltersGroup.id].filter(item => item.visible)];
  }
  //Filters-Track
  filtersTrack(index: number, item: FilterItem) {
    return `${item.key}-${index}`;
  }

  dateInputAction({dateFrom, dateTo}):void {
    console.log('FILTER-GROUP')
    console.log('dateFrom', dateFrom)
    console.log('dateTo', dateTo);


    this.filterDateInputAction.emit({dateFrom, dateTo});
  };

  updateScrollSpace(visibleItemsPresent: number){
    if(this.showAllItems){
      this.itemsHeight = visibleItemsPresent * 33;
      this.itemsLenght = visibleItemsPresent;
    }else{
      if(this.showItemsNumb > 0){
        this.itemsSteps = visibleItemsPresent < this.showItemsNumb ? visibleItemsPresent : this.showItemsNumb;
        this.itemsHeight = this.itemsSteps * 33;
        this.itemsLenght = visibleItemsPresent;
        this.itemsStepsList = [];
        this.itemsStepsList.push(this.itemsSteps);
      }else{
        if(visibleItemsPresent != this.itemsLenght){
          this.itemsSteps = visibleItemsPresent < 5 ? visibleItemsPresent : 5;
          this.itemsHeight = this.itemsSteps * 33;
          this.itemsLenght = visibleItemsPresent;
          this.itemsStepsList = [];
          this.itemsStepsList.push(this.itemsSteps);
          if(visibleItemsPresent === 0)
            this.itemsHeight = 1;
        }
      }
    }
  }

  //Toggle-Panel-Container
  toggleFilter(): void {  
    this.panelVisibility = this.panelVisibility === 'opened' ? 'closed' : 'opened';
  }

  //Filter-Item-Click
  itemClick(filterItem: FilterItem){
    this.filterItemClickGroup.emit({filterItem, filtersGroup: this.filtersGroup});
  }

  //Search-Delete
  searchDelete(f: NgForm){
    f.reset();
    this.filterSearchDelete.emit({filtersGroup: this.filtersGroup});
  }

  //Show-Less
  showMore(event: MouseEvent): void {
    event.stopPropagation();
    let stopValue: number = 5;
    if(this.showItemsNumb){
      stopValue: this.showItemsNumb;
    }
    let nextVal = this.itemsSteps + stopValue;
    if( nextVal > this.itemsList.length){
      nextVal = this.itemsSteps + (this.itemsList.length - this.itemsSteps);
    }
    this.itemsSteps = nextVal;
    this.itemsHeight = this.itemsSteps * 33;
    this.itemsStepsList.push(nextVal);
  }
  showLess(event: MouseEvent): void {
    event.stopPropagation();
    this.itemsStepsList.pop();
    let itemPresent = this.itemsStepsList[this.itemsStepsList.length - 1];
    this.itemsHeight = itemPresent * 33;
    this.itemsSteps = itemPresent;
  }
  lessMoreButtons(): boolean{
    let showbuttons: boolean = true;
    if(this.showAllItems){
      showbuttons = false
    }else{
      if(this.showItemsNumb > 0)
        showbuttons = false
    }
    return showbuttons;
  }

  //Select-Deselect
  selectAll(event: MouseEvent): void {
    event.stopPropagation();
    this.filtersSelectAllGroup.emit({filtersGroup: this.filtersGroup});
  }
  selectAllStyle(): string{
    let styleVal = 'filters-btn align-center-horizontal';
    if( this.itemsList.every((item: FilterItem) => item.selected === true)){
      styleVal = 'filters-btn-disabled align-center-horizontal';
    }
    return styleVal;
  }
  deselectAl(event: MouseEvent): void {
    event.stopPropagation();
    this.filtersDeselectAllGroup.emit({filtersGroup: this.filtersGroup});
  }
  deselectAllStyle(): string{
    let styleVal = 'filters-btn-disabled  align-center-horizontal fr';
    if( this.itemsList.some((item: FilterItem) => item.selected === true)){
      styleVal = 'filters-btn align-center-horizontal fr';
    }
    return styleVal;
  }

}

