import { createAction, props } from "@ngrx/store";
import { AuthResult } from 'src/app/models/msla.model';

export const logIn = createAction(
    '[Auth] LogIn'
)
export const loginRedirect = createAction(
    '[Auth] LogIn Redirect'
)



export const loginSignIn = createAction(
    '[Auth] LogIn - SignIn',
    props<{ auth: AuthResult}>()
)
export const loginSuccess = createAction(
    '[Auth] LogIn - Success',
    props<{auth: AuthResult;}>(),
)
export const loginFailed = createAction(
    '[Auth] LogIn - Failed',
    props<{value: AuthResult;}>(),
)
export const loginRenew = createAction(
    '[Auth] LogIn - Renew Token',
)


export const logOut = createAction(
    '[Auth] LogOut'
)
export const logOutSuccess = createAction(
    '[Auth] LogOut Success'
)






//Redux - Save Token
export const reduxSaveToken = createAction(
    '[Auth] Redux - Save Token',
    props<{auth: AuthResult}>()
)
//Redux - Check Token
export const reduxCheckToken = createAction(
    '[Auth] Redux - Check Token'
)
//Redux - Delete Token
export const reduxDeleteToken = createAction(
    '[Auth] Redux - Delete Token'
)
//Check Token Expired
export const checkTokenExpired = createAction(
    '[Auth] Token check if Expired',
    props<{auth: AuthResult;}>()
)
