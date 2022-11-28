import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LabelGroupService {

  apiPrefix;
  constructor(
    private http: HttpClient,
  ) {
    this.apiPrefix = environment.apiPrefix
  }

  getLabelGroups() {
    return this.http.get<any[]>(`${this.apiPrefix}/LabelGroup/GetAllLabelGroups`);
  }

  getLabelGroup(labelGroupId: number) {
    return this.http.get<any>(`${this.apiPrefix}/LabelGroup/GetSelectedLabelGroup/${labelGroupId}`);
  }

  getLabelByGroupDetails(groupId) {
    return this.http.get<any>(`${this.apiPrefix}/LabelGroup/GetLabelGroupDetailsById/${groupId}`);
  }

  saveLabelGroupDetailsById(data) {
    return this.http.post<any>(`${this.apiPrefix}/LabelGroup/SaveLabelGroupDetailsById`, data);
  }

  getParentLabelGroups() {
    return this.http.get<any[]>(`${this.apiPrefix}/LabelGroup/GetParentLabelGroups`);
  }
  addLabelGroups(LabelGroup: any) {
    return this.http.post<any>(`${this.apiPrefix}/LabelGroup/InsertLabelGroup`, LabelGroup);
  }

  editLabelGroups(labelGroupId, labelGroup) {
    return this.http.post<any>(`${this.apiPrefix}/LabelGroup/UpdateLabelGroup/${labelGroupId}`, labelGroup);
  }
}
