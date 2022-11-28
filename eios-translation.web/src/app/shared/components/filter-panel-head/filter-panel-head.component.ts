import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'eios-filter-panel-head',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="filters-dd-head">
      <div class="filters-dd-head-title">
        <div class="head-text">
          {{ panelHeadName }}
        </div>
        <div class="head-help" *ngIf="groupHelpFeature">
          <svg width="13" height="13" viewBox="0 0 13 13" xmlns="http://www.w3.org/2000/svg" class="helpIco">
            <circle  cx="6.5" cy="6.5" r="6"/>
            <path d="M4.5,5.2c0-1.1,0.9-2,2-2s2,0.9,2,2c0,1.1-0.9,2-2,2l0,2.5"/>
          </svg>
        </div>
      </div>
      <div [ngClass]="panelVisibility === 'opened' ? 'filters-dd-arrow-frame ico-chevron-big' : 'filters-dd-arrow-frame ico-chevron-big filters-dd-arrow-rotate' ">
      </div>
    </div>`,
  styles: [`
    .filters-dd-head{
        float: left;
        width: 100%;
        height: 40px;
        padding: 0 8px 0 8px;
        cursor: pointer;
        box-sizing: border-box;
    }
      .filters-dd-head .filters-dd-head-title{
          float: left;
          width: calc(100% - 12px);
          height: auto;
          cursor: pointer;
      }

      .filters-dd-head .filters-dd-head-title .head-text{
        float: left;
        height: auto;
        width: auto;
        color: #4C789E;
        line-height: 40px;
        font-size: 15px;
        font-family: "font-Semibold", Arial, Helvetica, sans-serif;
        box-sizing: border-box;
      }     
      .filters-dd-head .filters-dd-head-title .head-help{
        float: left;        
        height: 13px;
        width: 13px;
        box-sizing: border-box;
        line-height: 44px;
        margin-left: 3px
      } 
      .filters-dd-head .filters-dd-head-title .helpIco{
        fill:none;
        stroke: #4C789E;
        stroke-miterlimit:10;
      }  
      .filters-dd-head .filters-dd-head-title .helpIco:hover{
        stroke: #FCC00E;
      }

      .filters-dd-head .filters-dd-arrow-frame{
          float: left;
          width: 12px;
          height: 8px;
          transition: transform 1s ease;
          margin-top: 16px
      }
      .filters-dd-head .filters-dd-arrow-rotate{
        transform: rotateZ(-180deg) translateY(50%);
      }
  `]
})
export class FilterPanelHeadComponent {
  @Input() groupHelpFeature: boolean; 
  @Input() panelHeadName: string;
  @Input() panelVisibility: string  = 'opened';
}
