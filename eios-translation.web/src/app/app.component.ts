import { AppState } from './core/core.module';
import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { environment } from './../environments/environment.prod';

import { select, Store } from '@ngrx/store';
import { fromEvent, timer } from 'rxjs';
import { debounce, switchMap } from 'rxjs/operators';

import { CalendarService } from './shared/services/calendars.service';

//Redux
import * as UiStatusActions from './core/store/ui-status/ui-status.actions';
import { getUiStatus } from './core/store/ui-status/ui-status.selectors';
import { TranslocoService } from '@ngneat/transloco';

@Component({
  selector: 'eios-root',
  template: `
    <div class="wrapper-overall">
      <ng-container 
        *ngIf="mainComponents">
        <eios-header
            [headerState]="headerState"
            [languageCurrent]="languageCurrent" 
            (headerSize)="headerSize($event)"
            (headerVisibility)="headerSetVisibility()"
            (modalToggle)="modalToggleEvent($event)"
        ></eios-header>
      <div [ngStyle]="{'padding-top': paddingTop + 'px', 'transition-duration' : transitionDuration, 'padding-right' : '100px', 'padding-left': '100px' }">

      <router-outlet>
      <ts-toaster></ts-toaster>
        </router-outlet>
      </div>
        </ng-container>

      <eios-modal
        modalName="languages"
        [modalToDisplay]="modalToDisplay"
        [modalDescription]="'modal-languages.description' | transloco">
        <div header>
          <div class="jumbotron-title-wrapper">
          {{ 'modal-languages.title' | transloco }}
          </div>
          <div class="jumbotron-closeModal"
            (click)="modalHandlerToggle(undefined, '')">
          </div>
        </div>
        <div content>
          <div class="btn-modal" 
            *ngFor="let item of languageList"
            [ngClass]="{'active': this.languageCurrent === item.code}"
            (click)="langChangeBtnHandler($event, item)">
            {{ item.label }}
          </div>
        </div>
        <div footer>
          <div class="btn-modal btn-discard fr"
            (click)="modalHandlerToggle(undefined, '')">
            {{ 'modal-languages.close' | transloco }}
          </div>
        </div>
      </eios-modal>
      
    </div>
`,
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  
  //Observables
  event$;  
  uiStatus$;
  router$;

  headerState: string;
  navBarState: string;
  mainComponents: boolean = false;
  routerUrl: string;
  paddingTop: number = 0;
  transitionDuration: string = '0ms';
  title = 'EIOS - Epidemic Intelligence from Open Sources';
  windowWidth: number = 0;

  //ModalDisplay
  modalToDisplay: string;

  //Translation Feature
  languageCurrent: string = 'en';
  languageList = environment.languageList;

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private translocoService: TranslocoService,
    private _calendarService: CalendarService
  ){ 
    this.event$ = this.router.events.subscribe((navEv) => {
      if(navEv instanceof NavigationStart) {
        this.mainComponents = navEv.url.includes('/login') || !navEv.url.split('/') ? false : true;
      }
    });
  };

  ngOnInit(): void{

    let winWidth: number = window.innerWidth;
    let winHeight: number = window.innerHeight;
    this.store.dispatch(UiStatusActions.setInitialState({ winWidth, winHeight }));

    fromEvent(window,'resize')
      .pipe( 
        debounce(() => timer(400)),
        switchMap(evn => {
          winWidth = (evn.target as Window).innerWidth;
          winHeight = (evn.target as Window).innerHeight;
          return [
            this.store.dispatch(UiStatusActions.windowResize({winWidth, winHeight})),
            this.store.dispatch(UiStatusActions.navBarResize({winWidth}))
          ]
        })
    ).subscribe();

    fromEvent(window, 'click').subscribe(event => {
      const elClick = event.target as HTMLInputElement;
      const classList = elClick.classList.value.split(' ');
      let isDpCalendar = classList.filter(cls => cls.toLowerCase().includes('dp-')).length;
      if(!elClick.hasAttribute('data-click') && isDpCalendar < 1){
        let dataService = Date.now().toString();
        this._calendarService.setData(dataService);
      }
    });

    this.windowWidth = winWidth;
    this.uiStatus$ = this.store.pipe(select(getUiStatus)).subscribe(res =>{
      this.modalToDisplay = res.modalToDisplay;
      this.headerState = res.headerVisibility;
      if(res.headerVisibility === 'opened'){
        this.paddingTop = 102
      }else{
        this.paddingTop = 30
      }
      if(this.paddingTop > 0){
        setTimeout(() => {
          this.transitionDuration = "500ms"
        }, 2000);
      }
      this.navBarState = res.navBarVisibility;
      if(this.languageCurrent !== res.siteLanguage){
        this.languageCurrent = res.siteLanguage;
        this.translocoService.setActiveLang(res.siteLanguage);
      }
    });
  };

  navBarSize({width, height}): void{
    this.store.dispatch(UiStatusActions.navBarSize({ width, height }));
  };
  navBarSetVisibility(): void{
    this.store.dispatch(UiStatusActions.navBarVisibility());
  };
  
  headerSize({width, height}): void{
    this.store.dispatch(UiStatusActions.headerSize({ width, height }));
  };
  headerSetVisibility(): void{
    this.store.dispatch(UiStatusActions.headerVisibility());
  };

  //Modal - Toggle Event Emitter
  modalToggleEvent({ modalName }): void{
    this.store.dispatch(UiStatusActions.modalToShown({ modalName }));
  };
  //Modal - Toggle Btn Handler
  modalHandlerToggle(event: MouseEvent = undefined, modalName: ''): void {
    if(event !== undefined){
      event.stopPropagation();
    }
    this.store.dispatch(UiStatusActions.modalToShown({ modalName }));
  }

  //Languages - Change Btn Handler
  langChangeBtnHandler(event: MouseEvent, itemLang): void{
    event.stopPropagation();
    this.store.dispatch(UiStatusActions.languageChange({ codeLang: itemLang.code }))
    this.modalToggleEvent({ modalName: ''});
  }



}
