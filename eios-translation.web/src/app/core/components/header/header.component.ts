import { ChangeDetectionStrategy, Component, Input, Output, EventEmitter, AfterViewInit } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Router } from '@angular/router';

@Component({
  selector: 'eios-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [
    trigger('collapsable', [
      state('opened', style({
        height: '72px'
      })),
      state('closed', style({
        height: '0px',
        overflow: 'hidden'
      })),
      transition('opened => closed', [
        animate('0.4s cubic-bezier(0.55, 0, 1, 0.45)')
      ]),
      transition('closed => opened', [
        animate('0.4s 1s cubic-bezier(0.55, 0, 1, 0.45)')
      ]),
    ]),
  ]
})
export class HeaderComponent implements AfterViewInit {

  @Input() headerState: string;
  @Input() languageCurrent: string; 
  @Output() headerVisibility: EventEmitter<{}> = new EventEmitter<{}>();
  @Output() headerSize: EventEmitter<{width: number, height: number}> = new EventEmitter<{width: number, height: number}>();
  @Output() modalToggle: EventEmitter<{modalName: string}> = new EventEmitter<{modalName: string}>();
  
  subMenu = null;
  menuMobileVisibilty: string = 'hidden';

  constructor(
    private router: Router
  ) { }

  ngAfterViewInit(): void{
    const width: number = (<HTMLDivElement>document.getElementById("header")).offsetWidth;
    const height: number = (<HTMLDivElement>document.getElementById("header")).offsetHeight;
    this.headerSize.emit({width, height});
  }

  headerToggle(){
    this.headerVisibility.emit();
  }

  subMenuToggle(menuName: string){
    if (this.subMenu === menuName){
      this.subMenu = null
    } else {
      this.subMenu = menuName;
    }
  }
  menuMobileToggle(visibility: string){
    this.menuMobileVisibilty = visibility;
  }

  //Modal - Toggle
  modalHandlerToggle(event: MouseEvent = undefined, modalName): void {
    event.stopPropagation();
    this.modalToggle.emit({ modalName });
  }

  printMe(){
    console.log('render Header')
  }

  goToLanguage(){
    this.router.navigate(['languages']);
  }
  goToGroups() {
    this.router.navigate(['groups']);
  }
  goToLabels() {
    this.router.navigate(['labels']);
  }

}
