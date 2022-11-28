import { ChangeDetectionStrategy, Component, Input, OnChanges, SimpleChanges, OnInit } from '@angular/core';

@Component({
  selector: 'eios-wrapper-feature',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="wrapperFeature"
    [ngStyle]="styleWrapper()" 
    >
      <ng-content></ng-content>
    </div>
`,
  styles: [`
    .wrapperFeature{
      position: fixed;
      border-bottom: 1px solid #eeeeee;
      box-sizing: border-box;
      transition: 500ms cubic-bezier(0.250, 0.250, 0.750, 0.750);
      z-index: 10;
      background-color: red
    }
  `]
})
export class WrapperFeatureComponent implements OnChanges, OnInit {

  @Input() navBarState: string;  
  @Input() headerState: string;
  @Input() windowWidth: number;
  @Input() windowHeight: number;
 
  topValue: string = "";
  heightValue: string = "";
  widthValue: string = "";
  transitionValue: string = "";
  leftValue: string = "";

  ngOnInit(): void {
    this.styleMaker();
  }

  ngOnChanges(changes: SimpleChanges): void {
      this.styleMaker();
  }

  styleMaker() {
    let winWith =  window.innerWidth;
    let winHeight = window.innerHeight;

    if (
      this.headerState === 'opened'
    ){
      this.topValue = '92px';
      this.transitionValue = '0';
      this.heightValue = `calc(${winHeight}px - 92px`;
    } else {
      this.topValue = '24px';
      this.transitionValue = '0';
      this.heightValue = `calc(${winHeight}px - 24px`;
    }

    if(
      this.navBarState === 'hidden'
    ){ 
      this.leftValue = '2px';
      this.transitionValue = '0';
      this.widthValue = `calc(${winWith}px - 2px`;
    } else {
      this.leftValue = '66px' ,
      this.transitionValue = '0',
      this.widthValue = `calc(${winWith}px - 66px`;
    }
  }

  styleWrapper(): Object {
    return {
      height: this.heightValue,
      width: this.widthValue,
      top: this.topValue,
      left: this.leftValue,
      transitionDelay: this.transitionValue
    }
  }

}
