import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TooltipDirective } from './directives/tooltip.directive';
import { TranslocoRootModule } from './../transloco/transloco-root.module';

//CUSTOM-COMPONENTS
import { JumbotronComponent } from './components/jumbotron/jumbotron.component';
import { ContainerFeatureComponent } from './components/container-feature/container-feature.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { ModalComponent } from './components/modal/modal.component';
//SERVICES
import { uiJumbotronService } from './services/ui-jumbotron.service';
import { FiltersEndPointService } from './services/filters-end-point.service';
import { FiltersQueryBuilderService } from './services/filters-query-builder.service';
import { FiltersTranslationsService } from './services/filters-translations.service';
import { dateTimeChecker } from './services/dateTimeChecker.service';

//Filters-Groups
import { FilterSearchComponent } from './components/filter-search/filter-search.component';
//Filters-Components
import { WrapperFeatureComponent } from './components/wrapper-feature/wrapper-feature.component';
import { FilterItemComponent } from './components/filter-item/filter-item.component';
import { FilterPanelHeadComponent } from './components/filter-panel-head/filter-panel-head.component';
import { MetricPipe } from './pipes/metric.pipe';
import { FiltersGroupComponent } from './components/filters-group/filters-group.component';
import { LoaderFeatureComponent } from './components/loader-feature/loader-feature.component';
import { FilterItemSkeletonComponent } from './components/filter-item-skeleton/filter-item-skeleton.component';
import { FiltersGroupSkeletonComponent } from './components/filters-group-skeleton/filters-group-skeleton.component';
import { PaginationNewComponent } from './components/pagination-new/pagination-new.component';
import { FilterDatePickerComponent } from './components/filter-date-picker/filter-date-picker.component';
//
import { DpDatePickerModule } from 'ng2-date-picker';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { FilterDateFromToComponent } from './components/filter-date-from-to/filter-date-from-to.component';
import { CardModule } from 'primeng/card';
import { InputTextareaModule } from "primeng/inputtextarea";
import { InputTextModule } from "primeng/inputtext";
import { ButtonModule } from 'primeng/button';
import { PanelModule } from 'primeng/panel';
import { DropdownModule } from 'primeng/dropdown';
import { TableModule } from 'primeng/table';
import { SelectButtonModule } from 'primeng/selectbutton';
import { TabViewModule } from 'primeng/tabview';
import { MultiSelectModule } from 'primeng/multiselect';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { ToastModule } from 'primeng/toast';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { FileUploadModule } from 'primeng/fileupload';

const maskConfig: Partial<IConfig> = {
  validation: false,
};

@NgModule({
  declarations: [
    TooltipDirective,
    JumbotronComponent,
    WrapperFeatureComponent,
    ContainerFeatureComponent,
    PaginationComponent,
    ModalComponent,
    LoaderFeatureComponent,
    //Filters-Groups
    FiltersGroupComponent,
    FilterSearchComponent,
    //Filters-Components
    FilterItemComponent,
    FilterPanelHeadComponent,
    //Pipes
    MetricPipe,
    FilterItemSkeletonComponent,
    FiltersGroupSkeletonComponent,
    PaginationNewComponent,
    FilterDatePickerComponent,
    FilterDateFromToComponent,
  ],
  imports: [
    CommonModule,
    TranslocoRootModule,
    FormsModule,
    DpDatePickerModule,
    NgxMaskModule.forRoot(maskConfig),
    CardModule,
    InputTextareaModule,
    InputTextModule,
    ButtonModule,
    PanelModule,
    DropdownModule,
    TableModule,
    SelectButtonModule,
    TabViewModule,
    MultiSelectModule,
    DynamicDialogModule,
    ToastModule,
    BreadcrumbModule,
    FileUploadModule
  ],
  exports: [
    TooltipDirective,
    TranslocoRootModule,
    ModalComponent,
    LoaderFeatureComponent,
    DpDatePickerModule,
    //Components
    JumbotronComponent,
    WrapperFeatureComponent,
    ContainerFeatureComponent,
    PaginationComponent,
    PaginationNewComponent,
    //Filter-Groups
    FiltersGroupComponent,
    FilterSearchComponent,
    //Filter-Components
    FilterItemComponent,
    FilterPanelHeadComponent,
    CardModule,
    InputTextareaModule,
    InputTextModule,
    ButtonModule,
    PanelModule,
    DropdownModule,
    TableModule,
    SelectButtonModule,
    TabViewModule,
    MultiSelectModule,
    DynamicDialogModule,
    ToastModule,
    FormsModule,
    BreadcrumbModule,
    FileUploadModule
  ],
  providers: [
    dateTimeChecker,
    FiltersEndPointService,
    FiltersTranslationsService,
    FiltersQueryBuilderService
  ]
})
export class SharedModule {

  static forRoot() {
    return {
      ngModule: SharedModule,
      providers: [
        uiJumbotronService
      ],
    };
  }

}
