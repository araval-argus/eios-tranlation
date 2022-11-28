import { AuthResult } from './../../../models/msla.model';
import { MsalService, MSAL_INSTANCE } from '@azure/msal-angular';
import { Injectable } from "@angular/core";

@Injectable()
export class AuthServices {

    constructor(
        private msalService: MsalService
    ){ }

    login(){
        return this.msalService.handleRedirectObservable()
    }
    loginRedirect(){
        return this.msalService.loginRedirect();
    }
    renewToken(){

        var request = {
            scopes: ["user.read"]
        };

        return this.msalService.acquireTokenSilent(request);
    }


    authSaveLocalStorage(auth: any){
        localStorage.setItem('aztkn', JSON.stringify(auth))
    }
    authDeleteLocalStorage(){
        localStorage.removeItem('aztkn');
    }
    authGetLogalStorage(): AuthResult{
        let authRes = {} as AuthResult;
        try {
            let getAuth = localStorage.getItem('aztkn');
            if(getAuth !== null){
                authRes = JSON.parse(getAuth);
            }
        }
        catch(err) {
            console.log('ERROR authGetLogalStorage', err)
        }
        return authRes;
    }
    authCheckifExpired(auth: AuthResult): boolean{
        let isAuthExpired: boolean = false;
        if (new Date(auth.expiresOn).valueOf() < Date.now()) {
            //Attivo
            isAuthExpired = true;
        }
        return isAuthExpired;
    }
    authCheckExpiredTime(auth: AuthResult): number{
        let time = new Date(auth.expiresOn).valueOf() - Date.now();
        return time;
    }

}