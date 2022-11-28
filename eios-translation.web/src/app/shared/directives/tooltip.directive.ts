import { Input, Directive, ElementRef, Renderer2, HostListener } from '@angular/core';
import { fromEvent, of } from 'rxjs';
import { delay, switchMap, takeUntil } from 'rxjs/operators'

@Directive({
  selector: '[eiosTooltip]'
})
export class TooltipDirective {

  @Input('eiosTooltip') tooltipTitle: string;
  @Input() placement: string;
  @Input() timeout: string;
  @Input() marginLeft?: string;
  @Input() delay?: string = '3000';

  tooltip: HTMLElement;
  offset = 10;

  constructor(
    private renderer: Renderer2, 
    private el: ElementRef) {
    }

    ngAfterViewInit(): any {
      const parentElement = this.el.nativeElement;
      const mouseleave$ = fromEvent(parentElement, 'mouseleave');
      const mouseenter$ = fromEvent(parentElement, 'mouseenter');

      mouseenter$.pipe(
                switchMap(val => of(val).pipe(
                  delay(+this.delay),
                  takeUntil(mouseleave$))
              ))
              .subscribe(res => {
                if(!this.tooltip){
                  this.show();
                } 
              });

     mouseleave$.subscribe(() =>{
       if(this.tooltip)
        this.hide();
     })
    }

    show() {
      this.create();
      this.setPosition();
      this.renderer.addClass(this.tooltip, 'ng-tooltip-show');
    }
  
    hide() {
      this.renderer.removeClass(this.tooltip, 'ng-tooltip-show');
      window.setTimeout(() => {
        this.renderer.removeChild(document.body, this.tooltip);
        this.tooltip = null;
      }, +this.delay);
    }


  create() {
    this.tooltip = this.renderer.createElement('span');

    this.renderer.appendChild(
      this.tooltip,
      this.renderer.createText(this.tooltipTitle) // textNode
    );

    this.renderer.appendChild(document.body, this.tooltip);

    this.renderer.addClass(this.tooltip, 'ng-tooltip');
    this.renderer.addClass(this.tooltip, `ng-tooltip-${this.placement}`);

    // delay
    this.renderer.setStyle(this.tooltip, '-webkit-transition', `opacity ${this.delay}ms`);
    this.renderer.setStyle(this.tooltip, '-moz-transition', `opacity ${this.delay}ms`);
    this.renderer.setStyle(this.tooltip, '-o-transition', `opacity ${this.delay}ms`);
    this.renderer.setStyle(this.tooltip, 'transition', `opacity ${this.delay}ms`);
  }

  setPosition() {
    const hostPos = this.el.nativeElement.getBoundingClientRect();
    const tooltipPos = this.tooltip.getBoundingClientRect();
    const scrollPos = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;

    let top, left;

    if (this.placement === 'top') {
      top = hostPos.top - tooltipPos.height - this.offset;
      left = hostPos.left + (hostPos.width - tooltipPos.width) / 2;
    }

    if (this.placement === 'bottom') {
      top = hostPos.bottom + this.offset;
      left = hostPos.left + (hostPos.width - tooltipPos.width) / 2;
    }

    if (this.placement === 'left') {
      top = hostPos.top + (hostPos.height - tooltipPos.height) / 2;
      left = hostPos.left - tooltipPos.width - this.offset;
    }

    if (this.placement === 'right') {
      top = hostPos.top + (hostPos.height - tooltipPos.height) / 2;
      left = hostPos.right + this.offset;
    }

    if(this.marginLeft){
      this.renderer.setStyle(this.tooltip, 'margin-left', `${this.marginLeft}px`)
    }

    this.renderer.setStyle(this.tooltip, 'top', `${top + scrollPos}px`);
    this.renderer.setStyle(this.tooltip, 'left', `${left}px`);
  }

}
