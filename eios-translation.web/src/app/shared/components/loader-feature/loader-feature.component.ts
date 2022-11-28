import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'eios-loader-feature',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="loader-feature-wrapper" [ngStyle]="{'display': this.showLoader === true ? 'none' : 'block' }">
      <div class="loaderSpinner featureSpinner"></div>
    </div>
  `,
  styles: [`
    .loader-feature-wrapper{
      position: fixed;
      display: block;
      -ms-flex-wrap: wrap;
      flex-wrap: wrap;
      -ms-flex-pack: center;
      justify-content: center;
      margin: 0 auto;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      z-index: 900;
      overflow-x: hidden;
      background-color: white;
      pointer-events: none;
      box-sizing: border-box;
      -moz-box-sizing: border-box;
    }
  `],
  styleUrls: [
    './../../../../assets/css/loader.spinner.css'
  ]
})
export class LoaderFeatureComponent {

  @Input() showLoader: boolean = true;

}
