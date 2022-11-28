import { 
  Component, 
  Input, 
  Output, 
  EventEmitter, 
  ChangeDetectionStrategy, 
  OnChanges, 
  SimpleChanges } from '@angular/core';


@Component({
  selector: 'eios-pagination',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
      <div id="paginationWrapper">
        <div class="paginationLeft">
          <div class="paginationButton pageActive paginationInfo" style="margin-left: 0">
              {{ 'pagination.showing' | transloco }} 
            <span class="font-semibold">{{ this.current }} </span> 
              {{ 'pagination.of' | transloco }} 
            <span class="font-semibold">
              {{ this.totalPages }}
            </span>
          </div>
        </div>

        <div class="paginationRight">
          <div class="loaderSpace">
            <div class="loaderSpinner paginationSpinner" *ngIf="this.isLoading"></div>
          </div>

          <div
            (click)="this.handlerPrev()" 
            [ngClass]="(this.current > 1) && (this.totalPages > 1) ? 'pageArrow paginationButton icoBack' : 'pageArrow paginationButton icoBackDisable'">
          </div>

          <div *ngFor="let page of pages; index as idx">

            <div 
              *ngIf="page > 0"
              (click)="this.handlerGoTo(page)" 
              [ngClass]="this.current === page ? 'paginationButton pageActive' : 'paginationButton pageNumber'">
              {{ page }}
            </div>

            <div 
              (click)="this.handlerMore(page, idx)" 
              class="paginationMore"
              *ngIf="page < 0">
              ...
            </div>

          </div>

          <div
          (click)="this.handlerNext()" 
          [ngClass]="(this.current < this.totalPages) && (this.totalPages > 1) ? 'paginationButton icoForward pageArrow' : 'paginationButton icoForwardDisable pageArrow'">
          </div>

        </div>
      </div>
  `,
  styles: [`
     #paginationWrapper{
        float: left;
        width: 100%;
        height: auto;
        overflow: hidden;
        box-sizing: border-box;
        padding: 0 24px 0 25px;
        margin: 18px 0 18px 0;
      }
        .paginationLeft{
            float: left;
            width: 30%;
            height: auto;
            margin: 0 0 0 0;
            box-sizing: border-box;
            display: flex;
            justify-content: left;
            align-items: center;
        }
        .loaderSpace{
          float: left;
          width: 32px;
          height: 24px;
          background-repeat:no-repeat;
          background-position: center center;
          -webkit-background-size: contain;
          -moz-background-size: contain;
          -o-background-size: contain;
          background-size: contain;
        }
        .paginationRight{
            float: left;
            width: 70%;
            height: auto;
            margin: 0 0 0 0;
            box-sizing: border-box;
            display: flex;
            justify-content: right;
            align-items: center;
        }
    .paginationMore{
      float: left;
      color: #5E6366;
      margin-left: 4px;
      border-radius: 6px;
      line-height: 24px;
      cursor: pointer;
      box-sizing: border-box;
      font-size: 11px;
      font-family: "font-Medium", Arial, Helvetica, sans-serif;
      text-align: center;
      text-rendering: optimizeLegibility;
    }
    .paginationButton{
        float: left;
        color: #5E6366;
        margin-left: 4px;
        border-radius: 6px;
        line-height: 24px;
        cursor: pointer;
        box-sizing: border-box;
        font-size: 11px;
        font-family: "font-Medium", Arial, Helvetica, sans-serif;
        text-align: center;
        text-rendering: optimizeLegibility;
        background-repeat: no-repeat;
        background-position: center center;
        border: 1px solid #E2E2E2;
    }
      .pageNumber{
        height: 24px;
        padding: 0 10px 0 10px;
        background-color: rgba(230, 230, 230, 0.1);
      }
      .pageActive{
        height: 24px;
        padding: 0 10px 0 10px;
        pointer-events: none;
        background-color: rgba(62, 138, 200, 0.055);
      }
      .pageArrow{
        height: 24px;
        padding: 0 16px 0 16px;
        border: 1px solid #E2E2E2;
      }
      .icoForward{
        background-color: #ffff;
        background-image: url(data:image/svg+xml,%3Csvg%20width%3D%228%22%20height%3D%2210%22%20viewBox%3D%220%200%208%2010%22%20fill%3D%22none%22%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%3E%0A%20%20%3Cpath%20id%3D%22c5e3cf29071c0f7cf0327623880450e3%22%20d%3D%22M0%2010L1.74846e-06%200L8%205L0%2010Z%22%20fill%3D%22%233D5467%22%3E%3C%2Fpath%3E%0A%3C%2Fsvg%3E);
      }
      .icoForwardDisable{
        background-color: #ffff;
        pointer-events: none;
        background-image: url(data:image/svg+xml,%3Csvg%20width%3D%228%22%20height%3D%2210%22%20viewBox%3D%220%200%208%2010%22%20fill%3D%22none%22%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%3E%0A%20%20%3Cpath%20id%3D%22c5e3cf29071c0f7cf0327623880450e3%22%20d%3D%22M0%2010L1.74846e-06%200L8%205L0%2010Z%22%20fill%3D%22%23CFDDE4%22%3E%3C%2Fpath%3E%0A%3C%2Fsvg%3E);
      }
      .icoBack{
        transform: rotate(180deg);
        background-color: #ffff;
        background-image: url(data:image/svg+xml,%3Csvg%20width%3D%228%22%20height%3D%2210%22%20viewBox%3D%220%200%208%2010%22%20fill%3D%22none%22%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%3E%0A%20%20%3Cpath%20id%3D%22c5e3cf29071c0f7cf0327623880450e3%22%20d%3D%22M0%2010L1.74846e-06%200L8%205L0%2010Z%22%20fill%3D%22%233D5467%22%3E%3C%2Fpath%3E%0A%3C%2Fsvg%3E);
      }
      .icoBackDisable{
        background-color: #ffff;
        pointer-events: none;
        transform: rotate(180deg);
        background-image: url(data:image/svg+xml,%3Csvg%20width%3D%228%22%20height%3D%2210%22%20viewBox%3D%220%200%208%2010%22%20fill%3D%22none%22%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%3E%0A%20%20%3Cpath%20id%3D%22c5e3cf29071c0f7cf0327623880450e3%22%20d%3D%22M0%2010L1.74846e-06%200L8%205L0%2010Z%22%20fill%3D%22%23CFDDE4%22%3E%3C%2Fpath%3E%0A%3C%2Fsvg%3E);
      }
    .paginationInfo{
      font-size: 11px;
      letter-spacing: 0.6px;
      font-family: "font-Regular", Arial, Helvetica, sans-serif;
      padding: 0 16px 0 16px;
    }
  `],
  styleUrls: [
    './../../../../assets/css/loader.spinner.css'
  ]
})
export class PaginationComponent implements OnChanges{

  @Input() id: string;
  @Input() total: number = 0;
  @Input() current: number = 0;
  @Input() isLoading: boolean = false;
  @Input() totalItems: number = 0;
  @Input() totalPages: number = 0;
  @Input() itemsPerPage: number = 0;

  @Output() goTo: EventEmitter<number> = new EventEmitter<number>();
  @Output() next: EventEmitter<number> = new EventEmitter<number>();
  @Output() prev: EventEmitter<number> = new EventEmitter<number>();
  
  public pages: number[] = [];

  public ngOnChanges(changes: SimpleChanges): void {
    if((changes.current && changes.current.currentValue) ||
        (changes.totalPages && changes.totalPages.currentValue)){
          this.pages = this.getPages(this.current, this.totalPages);
        }
  }
  private getPages(current: number, total: number): number[] {
    if (total <= 7) {
      return [...Array(total).keys()].map((x) => ++x);
    }
    if (current > 5) {
      if (current >= total - 4) {
        return [1, -1, total - 4, total - 3, total - 2, total - 1, total];
      } else {
        return [1, -1, current - 1, current, current + 1, -2, total];
      }
    }
    return [1, 2, 3, 4, 5, -2, total];
  }


  //Handlers
  public handlerGoTo(page: number): void{
    if(!this.isLoading && this.current !== page){
      this.goTo.emit(page);
    }
  }
  public handlerNext(): void{
    if(!this.isLoading && this.current < this.totalPages){
      this.next.emit(this.current);
    }
  }
  public handlerPrev(): void{
    if(!this.isLoading && this.current > 1){
      this.prev.emit(this.current);
    }
  }
  public handlerMore(page, idx): void{
    if(!this.isLoading){
      if(page <= -2){
        let numPage = this.pages[idx - 1];
        this.next.emit(numPage);
      }else{
        let numPage = this.pages[idx + 1];
        this.prev.emit(numPage);
      }
    }
  }


}
