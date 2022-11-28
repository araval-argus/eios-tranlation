import { Component, AfterViewInit } from '@angular/core';

@Component({
  selector: 'eios-container-feature',
  template: `
    <div class="containerFeature">
      <ng-content></ng-content>
    </div>
`,
  styles: [`
    .containerFeature{    
      float: left;
      display: inline-block;
      top: 86px;
      height: auto;
      min-height: 0;
      right: 0px;
      left: 0;
      bottom: 0;
    }
  `]
})
export class ContainerFeatureComponent implements AfterViewInit {
  windowWidth: number;
  navBarService: any;

  constructor() { }

  ngAfterViewInit() : void {
  }

}
