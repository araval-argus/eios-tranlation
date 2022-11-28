
export interface UiStatus{
    //Window
    windowWidth: number;
    windowHeight: number;
    //NavBar
    navBarVisibility: string,
    navBarWidth: number,
    navBarHeight: number,
    //Header
    headerVisibility: string,
    headerWidth: number,
    headerHeight: number,
    //SideBar
    siderBarVisible: string,
    sideBarWidth: number;
    sideBarHeight: number;
    sideHeadWidth: number;
    sideHeadHeight: number;
    sideBarScroll?: string;
    //JumboTron
    jumbotronWidth: number;
    jumbotronHeight: number;
    //MODAL
    modalToDisplay: string
    //LOADING ARRAY
    isLoading: string[];
    //Site Language
    siteLanguage: string;
    //Data-Loaded
    dataLoaded: boolean;
}