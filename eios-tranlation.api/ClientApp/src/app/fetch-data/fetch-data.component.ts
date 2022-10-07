import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public labels: Label[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Label[]>(baseUrl + 'api/labels/getAll').subscribe(result => {
      this.labels = result;
    }, error => console.error(error));
  }
}

interface Label {
  LabelId?: number,
  ResourceId?: string,
  FK_LabelGroupId?: number,
  FK_LanguageId?: number,
  LabelValue?: string,
  LabelType?: number,
  LabelDescription?:string ,
  LabelSnapshotPath?: string,
  MachineTranslation?: string ,
  Scope?: string
  TranslationStatus?: number
  Version?: number
  IsActive?: boolean
  FK_PrevVersionLabelId?: string
}
