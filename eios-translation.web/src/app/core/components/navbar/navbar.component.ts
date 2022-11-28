import { animate, state, style, transition, trigger } from '@angular/animations';
import { ChangeDetectionStrategy, Component, Input, Output, EventEmitter, AfterViewInit } from '@angular/core';

@Component({
  selector: 'eios-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [
    trigger('hiddenable', [
      state('visible', style({
        transform: 'translateX(0px)'
      })),
      state('hidden', style({
        transform: 'translateX(-64px)'
      })),
      transition('visible => hidden', [
        animate('0.6s cubic-bezier(0, 0.55, 0.45, 1)')
      ]),
      transition('hidden => visible', [
        animate('0.6s 1s cubic-bezier(0, 0.55, 0.45, 1)')
      ]),
    ]),
  ]
})
export class NavbarComponent implements AfterViewInit {

  navBarStatus$;
  
  @Input() navBarState: string;
  @Output() navBarVisibility: EventEmitter<{}> = new EventEmitter<{}>();
  @Output() navBarSize: EventEmitter<{width: number, height: number}> = new EventEmitter<{width: number, height: number}>();

  ngAfterViewInit(): void{
    const width: number = (<HTMLDivElement>document.getElementById("navbarContainer")).offsetWidth;
    const height: number = (<HTMLDivElement>document.getElementById("navbar")).offsetHeight;
    this.navBarSize.emit({width, height});
  };
  navBarToggle(): void{
    this.navBarVisibility.emit();
  };
  printMe(): void {
    console.log('NavBar Print');
  };

}
