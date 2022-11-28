
import { RouterReducerState } from "@ngrx/router-store";
import { createFeatureSelector, createSelector } from "@ngrx/store";
import { AppState } from "../../core.module";


export const getRouter = (state: AppState) => state.router;



