import { getAccesToken } from './auth.selectors';
import { Injectable } from "@angular/core";
import { CanActivate } from "@angular/router";
import { select, Store } from "@ngrx/store";
import { Observable } from "rxjs";
import { AppState } from "../../core.module";
import { map, tap } from 'rxjs/operators';
import { go } from '../router/router.actions';

@Injectable()
export class AuthGuard implements CanActivate{

    constructor(
        private store: Store<AppState>
    ){}

    canActivate(): Observable<boolean> {
        return this.store.pipe(
            select(getAccesToken),
            map((token) =>  !!token),
            tap(isLogged => {
                if(!isLogged){
                    this.store.dispatch(go({ path: ['login']}))
                }
            })
        );
    }

}