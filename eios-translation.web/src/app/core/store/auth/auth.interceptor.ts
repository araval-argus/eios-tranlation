
import { switchMap, catchError } from 'rxjs/operators';
import { getAccesToken } from './auth.selectors';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { select, Store } from "@ngrx/store";
import { Observable, throwError } from "rxjs";
import { AppState } from "../../core.module";

@Injectable()
export class AuthInterceptor implements HttpInterceptor{
 
    constructor(private store: Store<AppState>){
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        return this.store.select(getAccesToken)
                .pipe(
                    switchMap(token => {
                        const newReq = !!token ? req.clone({
                            setHeaders: { 
                                'Access-Control-Allow-Origin': '*',
                                'Content-Type':  'application/json',
                                Authorization: 'Bearer ' + token
                            }
                        }) :  req;
                        return next.handle(newReq).pipe(
                            catchError(err => {
                                if(err instanceof HttpErrorResponse){
                                    switch(err.status){
                                        case 401:
                                            console.log('401 error', err.status)
                                        break;
                                        case 404:
                                            //AuthTimeOut
                                        break;
                                    }
                                }
                                return(throwError(err));
                            })
                        );
                    })
                );
    }

}