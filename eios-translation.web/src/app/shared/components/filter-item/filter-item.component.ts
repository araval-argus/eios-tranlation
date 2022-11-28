import { 
  Component, 
  Input, 
  OnChanges, 
  Output, 
  EventEmitter, 
  ChangeDetectionStrategy, 
  SimpleChanges 
} from '@angular/core';
import { FilterItem } from './../../../shared/models/filter-item.model';
@Component({
  selector: 'eios-filter-item',
  template: `
    <div class="filters-dd-item" (click)="selectFilter($event, item)" *ngIf="item.visible">
      <div class="item-wrapper" >
        <div class="item-sub-wrapper" [ngStyle]="this.styleItemBar()">
          <div class="item-area">
            <div [ngClass]="this.checkerVal" style="margin-left: 1px"></div>

            <div *ngIf="showKeyRing === true" class="item-ring align-center-horizontal fl">
              <svg xmlns="http://www.w3.org/2000/svg" x="12px" y="12px" viewBox="0 0 12 12"  >
                <circle cx="6" cy="6" r="6" [ngClass]="ringStyle(item.key)"/>
              </svg>
            </div>
            <div *ngIf="showKeyBadge === true" class="item-badge align-center-horizontal fl">
              {{ item.key }}
            </div>
            <div *ngIf="showCountryFlag === true && item.country" [ngClass]="this.flagStyle(item.country)">
            </div>
            <div class="item-text align-center-horizontal">
              <span *ngIf="showKeyBracket === true" class="item-bracket">
                ({{ item.key.toLowerCase() }})    
              </span>
              <span class="item-label">
                {{ isTranslationRequired === true ? item.labelTraslated : item.label }}
              </span>
            </div>
            <div class="item-question align-center-horizontal" *ngIf="showQuestionMark === true">
                <svg width="12px" height="12px" viewBox="0 0 12 12" xmlns="http://www.w3.org/2000/svg" style="margin: 0; padding: 0">
                  <path d="M6 0C4.81331 0 3.65328 0.351894 2.66658 1.01118C1.67989 1.67047 0.910851 2.60754 0.456726 3.7039C0.00259972 4.80026 -0.11622 6.00666 0.115291 7.17054C0.346802 8.33443 0.918247 9.40353 1.75736 10.2426C2.59648 11.0818 3.66558 11.6532 4.82946 11.8847C5.99335 12.1162 7.19975 11.9974 8.2961 11.5433C9.39246 11.0892 10.3295 10.3201 10.9888 9.33342C11.6481 8.34673 12 7.18669 12 6C12 4.4087 11.3679 2.88258 10.2426 1.75736C9.11743 0.632141 7.5913 0 6 0V0ZM7 7.83V10H5V6H6C6.19778 6 6.39112 5.94135 6.55557 5.83147C6.72002 5.72159 6.84819 5.56541 6.92388 5.38268C6.99957 5.19996 7.01937 4.99889 6.98079 4.80491C6.9422 4.61093 6.84696 4.43275 6.70711 4.29289C6.56726 4.15304 6.38907 4.0578 6.19509 4.01921C6.00111 3.98063 5.80005 4.00043 5.61732 4.07612C5.43459 4.15181 5.27841 4.27998 5.16853 4.44443C5.05865 4.60888 5 4.80222 5 5H3C3.00029 4.45007 3.15173 3.9108 3.43778 3.44112C3.72383 2.97144 4.13347 2.58942 4.62195 2.33681C5.11043 2.0842 5.65894 1.97072 6.20755 2.00876C6.75616 2.04681 7.28376 2.23492 7.7327 2.55254C8.18163 2.87016 8.53462 3.30506 8.75309 3.80973C8.97157 4.3144 9.04713 4.86941 8.9715 5.41412C8.89588 5.95882 8.67199 6.47226 8.32429 6.89833C7.9766 7.32439 7.51848 7.64669 7 7.83V7.83Z"></path>
                  <clipPath id="clip0">
                      <rect width="12" height="12" fill="white"></rect>
                    </clipPath>
                </svg>
            </div>
          </div>
          <div class="item-area-numb">
            <div class="item-percentage">
              <div class="item-badge align-center-horizontal fr"  eiosTooltip="{{ item.count }}" placement="right" timeout="1000" marginLeft="4" delay='200' >
                {{ item.count | metric }}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>`,
  styles: [`
    .filters-dd-item{ 
      float: left;
      width: 100%;
      height: 33px;
      cursor: pointer;
      padding: 4px 0 4px 0;
      border-bottom: 1px solid #eaeaea;
    }
      .filters-dd-item .item-wrapper{ 
          float: left;
          width: 100%;
          height: 24px;
          border-radius: 2px;
          background-color: #f6f6f6;
      }
      .filters-dd-item .item-sub-wrapper{ 
          float: left;
          width: 100%;
          height: 24px;
          padding: 0 0 0 8px;
          border-radius: 2px;
          background-color: #f6f6f6;
          -webkit-transition: background-image 0.4s ease-in-out;
          transition: background-image 0.4s ease-in-out;
      }
      .filters-dd-item .item-wrapper .item-area{  
          float: left;
          width: calc(100% -  50px);
          height: 24px;
      }

      .filters-dd-item .item-wrapper .item-area .item-label{  
        float: left;
        width: 100%;
        max-width: 180px;
        text-overflow: ellipsis;
        white-space: nowrap;
        overflow: hidden;
      }
      .filters-dd-item .item-wrapper .item-area .item-flag{  
          float: left;
          width: 16px;
          height: 12px;
          background-size: contain;
          background-position: 50%;
          background-repeat: no-repeat;
          margin: 0 4px 0 2px;
      }
      .filters-dd-item .item-wrapper .item-area-numb{  
          float: left;
          width: 50px;
          height: 24px;
      }
      .filters-dd-item .item-wrapper .item-text{  
          float: left;
          width: auto;
          height: auto;
          line-height: 12px;
          margin-left: 4px;
          letter-spacing: 0.3px;
          font-size: 12px;
          font-family: "font-Medium", Arial, Helvetica, sans-serif;
          text-transform: capitalize;
          text-rendering: optimizeLegibility;
          transition: 0.5s;
      }
      .filters-dd-item:hover .item-wrapper .item-text{ 
          margin-left: 10px
      }
        .filters-dd-item .item-wrapper .item-text .item-bracket{  
            float: left;
            width: auto;
            height: auto;
            margin-right: 2px;
            text-transform: lowercase;
            letter-spacing: 0.3px;
        }
      .filters-dd-item .item-wrapper .item-question{  
          float: left;
          display: flex;
          fill: #6c757d8f;
          justify-content: center;
          width: 12px;
          height: 12px;
          padding: 0;
          align: top;
          margin: 0 4px 0 4px;
     }
      .filters-dd-item .item-wrapper .item-ring{  
          float: left;
          display: flex;
          justify-content: center;
          width: 12px;
          height: 12px;
          padding: 0;
          align: top;
          margin: 0 2px 0 4px;
     }
        .filters-dd-item .item-wrapper .item-percentage{  
            float: right;
            width: 100%;
            min-width: 56px;
            height: 24px;
            font-size: 12px;
            border-radius: 2px;
            padding-right: 4px;
        }


      .filters-dd-item .item-wrapper .item-badge{  
          width: 48px;
          height: 16px;
          color: #9B9B9B;
          text-align: center;
          margin: 0 4px 0 2px;
          border-radius: 4px;
          line-height: 17px;
          font-size: 11px;
          background-color: white;
          font-family: "font-Bold", Arial, Helvetica, sans-serif;
      }
      .filters-dd-item .item-checkbox {
          float: left;
          height: 14px;
          width: 14px;
          margin-right: 4px;
          border: 2px solid #BBBBBB;
          border-radius: 2px;
          background-color: #F3F3F3;
      }
      .filters-dd-item .item-checkbox.checked {
        border: 2px solid #3E8AC8;
        transition: color 250ms cubic-bezier(.4,.0,.23,1);
      }
      .filters-dd-item .item-checkbox.checked::before{
        content: '';
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        width: 100%;
        height: 100%;
        pointer-events: none;
        background-image: url('./../../../../assets/images/sidebar-item-checkbox.svg');
        background-size: contain;
        background-repeat: no-repeat;
      }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FilterItemComponent implements OnChanges {

    @Input() item: FilterItem;
    @Input() countMax: number;
    @Input() isTranslationRequired: boolean;
    @Input() isTranslationFrontend: boolean;

    @Input() showKeyBadge?: boolean;
    @Input() showCountryFlag?: boolean;
    @Input() showQuestionMark?: boolean;
    @Input() showKeyRing?: boolean;
    @Input() showKeyBracket?: boolean;

    @Output() filterClickItem: EventEmitter<FilterItem> = new EventEmitter<FilterItem>();
    
    checkerVal: string;

    ngOnChanges(changes: SimpleChanges): void {
    }

    selectFilter(event: MouseEvent, item: FilterItem): void { 
      event.stopPropagation();
      this.filterClickItem.emit(item);
    };

    flagStyle(country: string): string {
      let flagStyle: string;
      if(country){
        flagStyle = `item-flag align-center-horizontal flag-icon flag-icon-${country}`;
      }
      return flagStyle.toLowerCase();
    };

    ringStyle(key: string): string{
      return key.replace(" ", "-").toLowerCase();
    };

    styleItemBar(): Object {
      let barSize = this.item.count > 0 ? (100 * this.item.count / this.countMax).toFixed(0) + '%' : '0%';
      this.checkerVal = 'item-checkbox align-center-horizontal';
      let textColor = '#888888';
      let backgroundVal = `url(/assets/images/sidebar-item-bkg-gray.svg) bottom right / ${barSize} repeat-y`;
      
      if(this.item.selected === true){
        textColor = '#2178C0';
        backgroundVal = `url(/assets/images/sidebar-item-bkg-azure.svg) bottom right / ${barSize} repeat-y`;
        this.checkerVal = 'item-checkbox checked align-center-horizontal';
      }
      return {
        background: backgroundVal,
        color: textColor
      }
    };

}
