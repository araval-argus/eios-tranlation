import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'eios-filter-item-skeleton',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="filters-dd-item">
      <div class="item-wrapper skeleton" >
        
      </div>
    </div>`,
  styles: [`
    .filters-dd-item{ 
      float: left;
      width: 100%;
      height: 33px;
      cursor: pointer;
      padding: 4px 0 4px 0;
    }
      .filters-dd-item .item-wrapper{ 
          float: left;
          width: 100%;
          height: 24px;
          border-radius: 2px;
          border: 1px solid #E8E8E8;
      }
  `],
  styleUrls: [
    './../../../../assets/css/skeleton.css'
  ],
})
export class FilterItemSkeletonComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
