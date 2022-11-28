import { Component, OnInit } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { Store } from '@ngrx/store';
import { AppState } from './../../../../core/core.module';
import * as AuthActions from './../../../../core/store/auth/auth.actions';

@Component({
  selector: 'eios-login',
  templateUrl: './login.component.html',
  styleUrls: [
    './login.component.css',
    './../../../../../assets/css/loader.spinner.css'
  ]
})
export class LoginComponent implements OnInit {

  constructor(
    private store: Store<AppState>,
    private authService: MsalService
  ) { }

  ngOnInit(): void {
    this.store.dispatch(AuthActions.logIn());
  }

  logOut(): void{
    localStorage.removeItem('aztkn');
    this.authService.logout()
  }


}
