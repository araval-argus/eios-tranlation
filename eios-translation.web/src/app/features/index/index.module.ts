import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LanguagesComponent } from './components/languages/languages.component';
import { LanguagesAddEditComponent } from './components/languages-add-edit/languages-add-edit.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { GroupComponent } from './components/group/group.component';
import { GroupAddEditComponent } from './components/group-add-edit/group-add-edit.component';
import { LabelsComponent } from './components/labels/labels.component';
import { LabelAddComponent } from './components/label-add/label-add.component';
import { ImportComponent } from './components/import/import.component';
import { ExportComponent } from './components/export/export.component';



@NgModule({
  declarations: [
    DashboardComponent,
    LanguagesComponent,
    LanguagesAddEditComponent,
    GroupComponent,
    GroupAddEditComponent,
    LabelsComponent,
    LabelAddComponent,
    ImportComponent,
    ExportComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      { path: '', redirectTo: 'languages', pathMatch: 'full' },
      { path: 'languages', component: LanguagesComponent },
      { path: 'languages/add', component: LanguagesAddEditComponent },
      { path: 'languages/edit/:id', component: LanguagesAddEditComponent },
      { path: 'groups', component: GroupComponent },
      { path: 'groups/add', component: GroupAddEditComponent },
      { path: 'groups/edit/:id', component: GroupAddEditComponent },
      { path: 'labels', component: LabelsComponent }
    ])]
})
export class IndexModule { }
