import { AuthResult } from 'src/app/models/msla.model';
import { AuthenticationResult } from '@azure/msal-browser';
import { selectAuthToken } from './auth.selectors';
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, mergeMap, switchMap, tap, withLatestFrom, } from "rxjs/operators";
import { AuthServices } from './auth.services';
import * as AuthActions from './auth.actions';
import * as RouterActions from './../router/router.actions';
import { throwError } from "rxjs/internal/observable/throwError";
import { AuthToken } from '@azure/msal-common';
import { select, Store } from '@ngrx/store';
import { AppState } from '../../core.module';

@Injectable()
export class AuthEffects{
    constructor(
        private actions$: Actions,
        private authservices: AuthServices,
        private store: Store<AppState>
    ){ }

  
    loginEffect$ = createEffect(() => this.actions$.pipe(
          ofType(AuthActions.logIn),
          switchMap(() => this.authservices.login()),
          map(res  => {
            if(res === null){
              //Not-Result
              console.log('No-Result', res);
              //return AuthActions.authCheckTokenPresent();
              return AuthActions.reduxCheckToken();
            }else{
              //Get-Result
              return AuthActions.loginSuccess({auth: JSON.parse(JSON.stringify(res))})
            }
          }),
          catchError( error => {
            console.log('loginEffect error and rethrowing', error);
            return throwError(error);
          })
      )
    );
    loginSuccessEffect$ = createEffect(() => this.actions$.pipe(
        ofType(AuthActions.loginSuccess),
        switchMap((res) => [
          AuthActions.reduxSaveToken({auth: res.auth}),
          RouterActions.go({ path: ['/dashboard']})
        ]),
        catchError( error => {
          console.log('loginSignInSuccessEffect error and rethrowing', error);
          return throwError(error);
        })
      ),
    );
    loginRedirectEffect$ = createEffect(() => this.actions$.pipe(
          ofType(AuthActions.loginRedirect),
          map(() => {
            this.authservices.loginRedirect();
          })
      ), { dispatch: false}
    );
    loginRenewEffect$ = createEffect(() => this.actions$.pipe(
        ofType(AuthActions.loginRenew),
        map(() => this.authservices.renewToken()),
        switchMap(res => {
          console.log('loginRenew', res)
          return [
            AuthActions.reduxSaveToken({auth: JSON.parse(JSON.stringify(res))}),
            RouterActions.go({ path: ['/dashboard']})
          ]
        })
      )
    )


    //Redux State - Save Token
    reduxSaveTokenEffect$ = createEffect(() => this.actions$.pipe(
        ofType(AuthActions.reduxSaveToken)
      ), { dispatch: false}
    );
    //Redux State - Check Token Present
    reduxChekTokenPresent = createEffect(() => this.actions$.pipe(
      ofType(AuthActions.reduxCheckToken),
      mergeMap(() => this.store.pipe(
        select(selectAuthToken),
        map((authToken: AuthResult ) => {
          console.log('authToken', authToken)
          if(Object.keys(authToken).length <= 0 || authToken === null){
            //No Access Token
            console.log('No Access Token - Go to LoginRedicet')
            return AuthActions.loginRedirect()
          }else{
            //Yes Access Token
            console.log('Yes Access Token')
            return AuthActions.checkTokenExpired({auth: authToken});
          }
        })
      )
      
      )
    )
    );

    //Token Check Expired
    authCheckTokenExpiredEffect$ = createEffect(() => this.actions$.pipe(
      ofType(AuthActions.checkTokenExpired),
      switchMap((res) => {
          const isAuthExpired = this.authservices.authCheckifExpired(res.auth);
          console.log('isAuthExpired', isAuthExpired)
          if(isAuthExpired){
            console.log('Token Expired')
            return [
              AuthActions.loginRedirect()
            ]
          }else{
            const time = this.authservices.authCheckExpiredTime(res.auth);
            if(time < 300000){
              //Token Running Out - Renew Token
              console.log('Token Running Out ')
              return [
                AuthActions.loginRenew()
              ]
            }else{
              //Token Valid
              console.log('Token Running Out ')
              return [
                AuthActions.reduxSaveToken({auth: res.auth}),
                RouterActions.go({ path: ['/dashboard']})
              ]
            }
          }
        }
      )
    )
  );



  













}