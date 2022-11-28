
import { createSelector } from '@ngrx/store';
import { AuthResult } from './../../../models/msla.model';
import { AppState } from './../../core.module';

export const selectAuthToken = (state: AppState) => state.authData;

export const getAccesToken = createSelector(
    selectAuthToken,
    (state: AuthResult) => state.idToken
)
