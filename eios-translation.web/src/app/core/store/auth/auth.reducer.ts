
import { createReducer, Action, on } from '@ngrx/store';
import { AuthResult } from './../../../models/msla.model';
import * as AuthActions from './../auth/auth.actions'

export const initialAuthState = {} as AuthResult;

const authStatusReducer = createReducer(

    //***INTIAL-STATE***//
    initialAuthState,

    //Auth Token - Save to State
    on(AuthActions.reduxSaveToken, (state, action) => {
        let authToken =  JSON.parse(JSON.stringify(action.auth)); 
        authToken.error = false;
        authToken.errorMessage = '';
        return authToken
    }),
    //Auth Token - Delete from State
    on(AuthActions.reduxDeleteToken, (state, action) =>{
        const authToken = {} as AuthResult;
        return authToken
    })

)

export function reducer(state: AuthResult | undefined, action?: Action){
    return authStatusReducer(state, action)
}