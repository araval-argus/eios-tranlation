import { Component, Input, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { LabelGroupService } from 'src/app/services/label-group.service';
import { LanguageService } from 'src/app/services/language.service';
import { ToasterService } from 'src/app/shared/services/toaster.service';
import { LabelAddComponent } from '../label-add/label-add.component';
import { MenuItem } from 'primeng/api';
import { ImportComponent } from '../import/import.component';
import { ExportComponent } from '../export/export.component';
@Component({
  selector: 'ts-labels',
  templateUrl: './labels.component.html',
  styleUrls: ['./labels.component.css'],
  providers: [DialogService]
})
export class LabelsComponent implements OnInit {

  languages = [];
  groups = [];
  labelsMaster: any;
  labels = [];
  totalRecords: number;
  loading: boolean;
  selectAll: boolean = false;
  ref: DynamicDialogRef;
  selectedCustomers = [];
  activeGroupId;
  cols = [];
  items: MenuItem[] = [];
  home: MenuItem;
  _selectedColumns: any[];

  newDemo:any[];

  constructor(
    private languageService: LanguageService,
    private labelGroupService: LabelGroupService,
    public dialogService: DialogService,
    private toastrService: ToasterService
  ) { }

  ngOnInit(): void {
    this.getLanguages();
    this.getLabelGroups();

  }
  getLanguages() {
    this.languageService.getLanguages().subscribe((response: any) => {
      this.languages = response.Result;
      this.languages.forEach(lang => {
        console.log("lang",lang);
        
        if (!lang.isDefault) {
          this.cols.push({name:lang.name});
          this._selectedColumns = this.cols; 
        }
      })
    });
  }

  getLabelGroups() {
    this.labelGroupService.getParentLabelGroups().subscribe((response: any) => {
      this.groups = response.Result;
      this.activeGroupId = this.groups[0]?.labelGroupId;
      this.loadLabelsByGroup();
    });
  }

  changeGroup(e) {
    const groupId = this.groups[e.index]?.labelGroupId;
    if (groupId) {
      this.activeGroupId = groupId;
      this.loadLabelsByGroup();
    }
  }
  changeChildGroup(e) {
    const groupId = this.labelsMaster.childGroups[--e.index]?.labelGroupId;
    if (groupId) {
      this.groups = this.labelsMaster.childGroups;
      this.activeGroupId = groupId;
      this.loadLabelsByGroup();
    }
  }
  goToParent(e) {
    this.activeGroupId = e.item.id;
    this.loadLabelsByGroup();
    // console.log(e.item)
  }

  @Input() get selectedColumns(): any[] {
    return this._selectedColumns;
  }

  set selectedColumns(val: any[]) {
    //restore original order
    this._selectedColumns = this.cols.filter((col) => val.includes(col));
  }

  loadLabelsByGroup() {
    this.labelGroupService.getLabelByGroupDetails(this.activeGroupId).subscribe((response: any) => {
      this.labelsMaster = response.Result;
      this.groups = this.labelsMaster.siblingGroups;
      if (this.labelsMaster.parentGroup) {
        const activeGroupIndex = this.items.findIndex(item => item.id == this.labelsMaster.parentGroup.labelGroupId);
        if(activeGroupIndex != -1) {
          this.items = this.items.slice(0, activeGroupIndex+1)
        } else {
          this.items.push({ label: this.labelsMaster.parentGroup.groupName, id: this.labelsMaster.parentGroup.labelGroupId })
        }
        this.items = [...this.items];
      }
      const labelsMaster = _.cloneDeep(this.labelsMaster.labels);

      this.labels = labelsMaster.map(label => {
        let labelTransformed = {};
        label.translatedLabels.forEach(translated => {
          labelTransformed[translated.languageName] = translated;
        })
        label.translatedLabels = labelTransformed;
        return label;
      })
      // console.log(this.labelsMaster, labelsMaster)

    })
  }

  saveLabels() {
    const labelSave = _.cloneDeep(this.labels);
    labelSave.map(label => {
      label.translatedLabels = Object.values(label.translatedLabels)
    })
    this.labelGroupService.saveLabelGroupDetailsById({ 'labelGroupId': this.activeGroupId, 'labels': labelSave }).subscribe((response: any) => {
      if (response.StatusCode == 200) {
        this.toastrService.notify('success', 'Success!', response.Message);
      } else {
        this.toastrService.notify('error', 'Error!', response.Message);
      }
    })
  }
  showAddLabel() {
    this.ref = this.dialogService.open(LabelAddComponent, {
      header: 'Add Label',
      width: '70%',
      contentStyle: { "overflow": "auto" },
      baseZIndex: 10000,
      maximizable: true,

    });
    this.ref.onClose.subscribe((res) => {
      this.loadLabelsByGroup();
    })
  }

  importLabel(){
    this.ref = this.dialogService.open(ImportComponent, {
      header: 'Add Label',
      width: '70%',
      contentStyle: { "overflow": "auto" },
      baseZIndex: 10000,
      maximizable: true,
    });
    }

  exportLabel(){
    this.ref = this.dialogService.open(ExportComponent, {
      header: 'Add Label',
      width: '70%',
      contentStyle: { "overflow": "auto" },
      baseZIndex: 10000,
      maximizable: true,

    });
  }
}
