import { createAction, props } from "@ngrx/store";

//***INITIAL-STATE***//
//InitialState-Setting
export const setInitialState = createAction(
    '[UI-Status] Initial-State',
    props<{winWidth: number, winHeight: number}>()
)
export const dataLoaded = createAction(
    '[UI-Status] Data-Loaded',
)

export const windowResize = createAction(
    '[UI-Status] Window Resize',
    props<{winWidth: number, winHeight: number}>()
)

//***NAVBAR***//
//NavBar Visibility
export const navBarVisibility = createAction(
    '[UI-Status] NavBar Visibility'
)
//NavBar Visibility on Resize
export const navBarResize = createAction(
    '[UI-Status] NavBar Resize',
    props<{winWidth: number}>()
)
//NavBar Size
export const navBarSize = createAction(
    '[UI-Status] NavBar Size',
    props<{width: number, height: number}>()
)

//***HEADER***//
//Header Visibility
export const headerVisibility = createAction(
    '[UI-Status] Header Visibility'
)
//Header Size
export const headerSize = createAction(
    '[UI-Status] Header Size',
    props<{width: number, height: number}>()
)

//***SIDEBAR***//
//SideBar Set Size
export const sideBarSetSize = createAction(
    '[UI-Status] SideBar Set Size',
    props<{sideWidth: number, sideHeight: number}>()
)
export const sideBarToggle = createAction(
    '[UI-Status] SideBar Toggle'
)
export const sideBarScroll = createAction(
    '[UI-Status] SideBar Scroll',
    props<{scrollStamp: string}>()
)

//***JUMBOTRON***//
//Jumbotron Set Size
export const jumbotronSetSize = createAction(
    '[UI-Status] Jumbotron Set Size',
    props<{width: number, height: number}>()
)

//***MODAL***//
export const modalToShown = createAction(
    '[UI-Status] Modal Display',
    props<{modalName: string}>()
)

//***IS-LOADING***//
//Loading Add
export const isLoadingAdd = createAction(
    '[UI-Status] IsLoading - Add',
    props<{queryName: string}>()
)
//Loading Remove
export const isLoadingRemove = createAction(
    '[UI-Status] IsLoading - Remove',
    props<{queryName: string}>()
)

//***LANGUAGE***//
//Language Change
export const languageChange = createAction(
    '[UI-Status] Language - Change',
    props<{codeLang: string}>()
)