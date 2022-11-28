import { NavigationExtras } from "@angular/router";
import { createAction, props } from "@ngrx/store";

export const go = createAction(
    '[Router] Go',
    props<{
        path: string[],
        extras?: NavigationExtras
    }>()
)

export const back = createAction(
    '[Router] Back'
);
export const forward = createAction(
    '[Router] Forward'
);
