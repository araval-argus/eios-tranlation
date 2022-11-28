import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

//Toast
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

//Resize Event
import { AngularResizeEventModule } from 'angular-resize-event';

//Redux
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { UiStatus } from './models/ui-status.model';

//Auth
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './store/auth/auth.interceptor';
import { AuthResult } from 'src/app/models/msla.model';
import { AuthGuard } from './store/auth/auth.guard';
import { AuthServices } from './store/auth/auth.services';

//Modules Custom
import { SharedModule } from './../shared/shared.module';
//Components Custom
import { NavbarComponent } from './components/navbar/navbar.component';
import { HeaderComponent } from './components/header/header.component';
//Router-Reducer
import { routerReducer, RouterReducerState, StoreRouterConnectingModule } from '@ngrx/router-store';
import { reducer as uiStatusReducer } from './store/ui-status/ui-status.reducer';
import { reducer as authStatusReducer} from './store/auth/auth.reducer'
import { RouterSerializer } from './store/router/router.serialized';
import { ToastCustomComponent } from './components/toast-custom/toast-custom.component';

export interface AppState{
  router: RouterReducerState,
  uiStatus: UiStatus,
  authData: AuthResult
}

@NgModule({
  declarations: [
    NavbarComponent,
    HeaderComponent,
    ToastCustomComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    AngularResizeEventModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    StoreModule.forRoot({
      router: routerReducer,
      uiStatus: uiStatusReducer,
      authData: authStatusReducer
    },
    {
      runtimeChecks: {
        strictActionImmutability: true,
        strictActionSerializability: true,
        strictStateImmutability: true,
        strictStateSerializability: true
      }
    }),
    StoreDevtoolsModule.instrument({
      maxAge: 25
    }),
    StoreRouterConnectingModule.forRoot({
      serializer: RouterSerializer
    }),
  
  ],
  exports: [
    NavbarComponent,
    HeaderComponent
  ],
  providers: [
    AuthServices,
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ]
})
export class CoreModule {
}
