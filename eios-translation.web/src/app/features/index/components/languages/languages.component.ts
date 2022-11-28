import { Component, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { LanguageService } from 'src/app/services/language.service';
import { ToasterService } from 'src/app/shared/services/toaster.service';

@Component({
  selector: 'ts-languages',
  templateUrl: './languages.component.html',
  styleUrls: ['./languages.component.css']
})
export class LanguagesComponent implements OnInit {

  languages = [];
  constructor(
    private languageService: LanguageService,
    private toastrService: ToasterService
  ) { }

  ngOnInit(): void {
    this.getLanguages();
    
  }

  getLanguages() {
    this.languageService.getLanguages().subscribe((response: any) => {
      if (response.StatusCode == 200) {
        this.languages = response.Result;
      } else {
        this.toastrService.notify('error', 'Error!', response.Message);
      }
    });
  }

}
