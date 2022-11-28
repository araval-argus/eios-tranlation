import { AfterViewInit, Component, EventEmitter, Input, Output, OnDestroy } from '@angular/core';
import { fromEvent, Observable } from 'rxjs';

@Component({
  selector: 'eios-filter-search',
  template: `
      <!-- search-board -->
      <div class="filters-group">
        <div class="filters-searchHead">
          <div class="filters-SearchTitle">
            {{ searchHeadName }}
          </div>
        </div>
        <div class="filters-ctrlWrapper mg-down-medium">
            <div class="filter-areaSearch">
              <input 
                id="searchBoards"
                type="text"
                name="text"
                class="text-field-sidebar text-field-button"
                [placeholder]='searchLabel'
                autocomplete="off">
              <button 
                (click)="searchHandler($event)" 
                [ngClass]="this.crossIcon">
              </button>
            </div> 
        </div>
      </div>
  `
})
export class FilterSearchComponent implements AfterViewInit, OnDestroy{

  @Input() searchHeadName: string;
  @Input() searchLabel: string;
  
  inputChange$;
  inputSearch;

  crossIcon: string = 'btn-text-find';

    //Events Emitters
  @Output() searchBoard: EventEmitter<{ searchText: string }> = new EventEmitter<{ searchText: string }>();
  
  ngAfterViewInit(){
    this.inputSearch = document.getElementById("searchBoards") as HTMLInputElement;
    this.inputChange$ = fromEvent<InputEvent>(this.inputSearch, "keyup").subscribe(event => {
      this.crossIcon = (event.target as HTMLInputElement).value.length > 0 ? 'btn-text-cross' : 'btn-text-find';
      if(event instanceof KeyboardEvent && event.key === 'Enter'){
        this.searchBoard.emit({searchText: this.inputSearch.value});
      }
    });
  }
  ngOnDestroy(): void{
    this.inputChange$.unsubscribe();
  };

  searchHandler(event: MouseEvent): void {
    event.stopPropagation();
    if(this.inputSearch.value.length > 0){
      this.inputSearch.value = '';
      this.crossIcon = 'btn-text-find';
      this.searchBoard.emit({searchText: ''});
    }
  }


}
