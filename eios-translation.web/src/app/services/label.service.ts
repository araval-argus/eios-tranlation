import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Label } from '../models/label.model';

@Injectable({
  providedIn: 'root'
})
export class LabelService {
  backendURL;
  apiPrefix;
  constructor(
    private http: HttpClient,
  ) {
    this.apiPrefix = environment.apiPrefix
  }

  getLabels() {
    return this.http.get<Label[]>(`${this.apiPrefix}/Label/GetAllLabels`);
  }

  getLabel(labelId: number) {
    return this.http.get<Label>(`${this.apiPrefix}/Label/GetSelectedLabel?LabelId=${labelId}`);
  }

  addLabel(label: Label) {
    return this.http.post<any>(`${this.apiPrefix}/Label/InsertLabel`, label);
  }

  exportLabel(id) {
    return this.http.get<any>(`${this.apiPrefix}/Label/ExportLabelsByLanguageId?languageId=${id}`);
  }

  downloadExportFile(response) {
    return this.http.get<any>(`${this.backendURL}/${response}`);
  }
}
