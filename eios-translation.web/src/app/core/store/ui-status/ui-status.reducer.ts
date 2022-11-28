import { act } from '@ngrx/effects';
import { Action, createReducer, on } from '@ngrx/store';
import { UiStatus } from './../../models/ui-status.model';
import * as UiStatusActions from './ui-status.actions';

export const initialUIState = {
    windowWidth: 0,
    windowHeight: 0,
    navBarVisibility: 'visible',
    navBarWidth: 0,
    navBarHeight: 0,
    headerVisibility: 'opened',
    headerWidth: 0,
    headerHeight: 0,
    siderBarVisible: 'visible',
    sideBarWidth: 0,
    sideBarHeight: 0,
    sideHeadWidth: 0,
    sideHeadHeight: 0,
    jumbotronWidth: 0,
    jumbotronHeight: 0,
    modalToDisplay: '',
    isLoading: [] as string[],
    siteLanguage: 'en',
    dataLoaded: false
} as UiStatus;

const UiStatusReducer = createReducer(

    //***INTIAL-STATE***//
    //InitialState
    initialUIState,
    //InitialState-Set
    on(UiStatusActions.setInitialState, (state, action) => {
        return{
            ...state,
            windowWidth: action.winWidth,
            windowHeight: action.winHeight
        }
    }),
    on(UiStatusActions.dataLoaded, (state, action) =>{
        return{
            ...state,
            dataLoaded: true
        }
    }),

    on(UiStatusActions.windowResize, (state, action) => {
        return{
            ...state,
            windowWidth: action.winWidth,
            windowHeight: action.winHeight
        }
    }),

    //***NAVBAR***//
    //NavBar Visbility Change OnClick
    on(UiStatusActions.navBarVisibility, (state, action) => {
        const navBarState: string = state.navBarVisibility === 'visible' ? 'hidden' : 'visible';
        return{
            ...state,
            navBarVisibility: navBarState
        }   
    }),
    //NavBar Visbility Change Resize
    on(UiStatusActions.navBarResize, (state, action) => {
        let visibility = state.navBarVisibility;
        if (action.winWidth <= 987 && state.navBarVisibility == 'visible'){
            visibility = 'hidden';
        }
        if (action.winWidth > 987 && state.navBarVisibility === 'hidden'){
            visibility = 'visible';
        }
        return{
            ...state,
            navBarVisibility: visibility
        }
    }),
    //NavBar Size
    on(UiStatusActions.navBarSize, (state, action) => {
        return{
            ...state,
            navBarWidth: action.width,
            navBarHeight: action.height
        }  
    }),

    //***HEADER***//
    //Header Visbility Change OnClick
    on(UiStatusActions.headerVisibility, (state, action) => {
        const headerState: string = state.headerVisibility === 'opened' ? 'closed' : 'opened';
        return{
            ...state,
            headerVisibility: headerState
        }   
    }),
    //Header Size
    on(UiStatusActions.headerSize, (state, action) => {
        return{
            ...state,
            headerWidth: action.width,
            headerHeight: action.height
        }   
    }),
    
    //***SIDEBAR***//
    //SideBar Set Size
    on(UiStatusActions.sideBarSetSize, (state, action) => {
        return{
            ...state,
            sideBarWidth: action.sideWidth,
            sideBarHeight: action.sideHeight,
        }   
    }),
    //SideBar Toggle
    on(UiStatusActions.sideBarToggle, (state, action) => {
        const sideBarState: string = state.siderBarVisible === 'visible' ? 'hidden' : 'visible';
        return{
            ...state,
            siderBarVisible: sideBarState
        }
    }),
    //SideBar Scroll
    on(UiStatusActions.sideBarScroll, (state, action) => {
        return{
            ...state,
            sideBarScroll: action.scrollStamp
        }
    }),

    //***JUMBOTRON***//
    //Jumbotron Set Size
    on(UiStatusActions.jumbotronSetSize, (state, action) => {
        return{
            ...state,
            jumbotronWidth: action.width,
            jumbotronHeight: action.height
        }   
    }),

    //***MODAL***//
    on(UiStatusActions.modalToShown, (state, action) => {
        return{
            ...state,
            modalToDisplay: action.modalName
        }   
    }),

    ///***LOADING***//
    //Add-Item
    on(UiStatusActions.isLoadingAdd, (state, action) => {
        let newArr: string[] = [...state.isLoading];
        newArr.push(action.queryName);
        return{
            ...state,
            isLoading: newArr
        } as UiStatus
    }),
    //Remove-Item
    on(UiStatusActions.isLoadingRemove, (state, action) => {
        let isLoadingArrray: string[] = [...state.isLoading];
        const index = isLoadingArrray.indexOf(action.queryName, 0);

        if(index > -1){
            isLoadingArrray.splice(index, 1);
        }
        return{
            ...state,
            isLoading: isLoadingArrray
        }
    }),

    ///***LOADING***//
    //Language Change
    on(UiStatusActions.languageChange, (state, action) => {
        return{
            ...state,
            siteLanguage: action.codeLang
        }
    })
)

export function reducer(state: UiStatus | undefined, action?: Action){
    return UiStatusReducer(state, action)
}

