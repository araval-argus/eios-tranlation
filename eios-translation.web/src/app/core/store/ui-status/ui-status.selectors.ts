import { AppState } from './../../core.module';

export const getNavBarStatus = (state: AppState) => state.uiStatus.navBarVisibility;
export const getHeaderStatus = (state: AppState) => state.uiStatus.headerVisibility;
export const getModalStauts = (state: AppState) => state.uiStatus.modalToDisplay;
export const getSiteLanguage = (state: AppState) => state.uiStatus.siteLanguage;

export const getSideBarScroll = (state: AppState) => state.uiStatus.sideBarScroll;

export const getUiStatus = (state: AppState) => state.uiStatus;
