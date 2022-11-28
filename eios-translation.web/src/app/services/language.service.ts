import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Language } from '../models/language.model';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  apiPrefix;
  constructor(
    private http: HttpClient,
  ) {
    this.apiPrefix = environment.apiPrefix
  }

  getLanguages() {
    return this.http.get<Language[]>(`${this.apiPrefix}/Language/GetAllLanguages`);
  }
  getLanguage(languageId: number) {
    return this.http.get<Language>(`${this.apiPrefix}/Language/GetSelectedLanguage/${languageId}`);
  }

  addLanguages(language: Language) {
    return this.http.post<any>(`${this.apiPrefix}/Language/InsertLanguage`, language);
  }

  editLanguages(languageId, language) {
    return this.http.post<any>(`${this.apiPrefix}/Language/UpdateLanguage/${languageId}`, language);
  }
}
