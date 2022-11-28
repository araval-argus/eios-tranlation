import { AfterViewInit, ChangeDetectionStrategy, Component, EventEmitter, Input, Output, OnDestroy, OnChanges, SimpleChanges } from '@angular/core';
import { fromEvent } from 'rxjs';

@Component({
  selector: 'eios-pagination-new',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl:'./pagination-new.component.html',
  styleUrls: [
    './pagination-new.component.css',
    './../../../../assets/css/loader.spinner.css'
  ],
})
export class PaginationNewComponent implements AfterViewInit, OnDestroy {

  @Input() id: string;
  @Input() isLoading: boolean = false;
  @Input() totalItems: number = 0;
  @Input() totalPages: number = 100;
  @Input() currentPage: number = 0;
  @Input() itemsPerPage: number = 0;

  @Output() goTo: EventEmitter<number> = new EventEmitter<number>();
  @Output() itemsChanger: EventEmitter<number> = new EventEmitter<number>();

  inputPage;
  inputPageChange$;

  ngAfterViewInit(): void {
    this.inputPage = document.getElementById("inputPage") as HTMLInputElement;
    this.inputPageChange$ = fromEvent<InputEvent>(this.inputPage, "keyup").subscribe(event => {
      let result = false;
      if(event instanceof KeyboardEvent){
        let pattern = /^([0-9])$/;
        result = pattern.test(event.key);
        let sanitized = this.inputPage.value;
        if(!result){
          sanitized = sanitized.replace(event.key, '');
          this.inputPage.value = sanitized;
        }
        if(+sanitized > this.totalPages){
          this.inputPage.value = this.totalPages;
        }
        if(event instanceof KeyboardEvent && event.key === 'Enter'){
          this.goTo.emit(+this.inputPage.value);
        }
      }
    });
  }
  ngOnDestroy(): void{
    this.inputPageChange$.unsubscribe();
  };
  

  //NEXT
  nextHandler(event: MouseEvent): void{
    event.stopPropagation();
    let nextPage = this.currentPage + 1;
    if(!this.isLoading && nextPage <= this.totalPages){
      this.inputPage.value = nextPage;
      this.currentPage = nextPage;
      this.goTo.emit(nextPage);
    }
  }
  lastHandler(event: MouseEvent): void{
    event.stopPropagation();
    if(!this.isLoading && this.currentPage !== this.totalPages){
      this.inputPage.value = this.totalPages;
      this.currentPage = this.totalPages;
      this.goTo.emit(this.totalPages);
    }
  }
  //PREV
  prevHandler(event: MouseEvent): void{
    event.stopPropagation();
    let nextPage = this.currentPage - 1;
    if(!this.isLoading && nextPage >= 1){
      this.inputPage.value = nextPage;
      this.currentPage = nextPage;
      this.goTo.emit(nextPage);
    }
  }
  //FIRST
  firstHanlder(event: MouseEvent): void{
    event.preventDefault();
    if(!this.isLoading && this.currentPage !== 1){
      this.inputPage.value = 1;
      this.currentPage = 1;
      this.goTo.emit(1);
    }
  }
  //RELOAD
  reloadHandler(event: MouseEvent): void{
    event.stopPropagation();
    if(!this.isLoading){
      this.goTo.emit(this.currentPage);
    }
  }
  //ITEMS-CHANGE
  itemsChange(event): void{
    this.itemsChanger.emit(event.target.value);
  }



}
