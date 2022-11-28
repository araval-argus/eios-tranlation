import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from 'lodash';
import { Language } from 'src/app/models/language.model';
import { LanguageService } from 'src/app/services/language.service';
import { ToasterService } from 'src/app/shared/services/toaster.service';

@Component({
  selector: 'ts-languages-add-edit',
  templateUrl: './languages-add-edit.component.html',
  styleUrls: ['./languages-add-edit.component.css']
})
export class LanguagesAddEditComponent implements OnInit {

  typeOptions: any[];
  submitted = false;
  languageId;
  languageForm: FormGroup = new FormGroup({
    name: new FormControl('', Validators.required),
    languageCode: new FormControl('', Validators.required),
    toleranceType: new FormControl(0, Validators.required),
    tolerance: new FormControl('', Validators.required),
    description: new FormControl(),
  });

  constructor(
    private languageService: LanguageService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastrService: ToasterService
  ) {
    this.typeOptions = [{ label: 'Absolute', value: 0 }, { label: 'Relative', value: 1 }];
    this.languageId = this.activatedRoute.snapshot.paramMap.get('id');
    if (this.languageId) {
      this.getLanguage();
    }
  }

  ngOnInit(): void { }

  getLanguage() {
    this.languageService.getLanguage(this.languageId).subscribe((response: any) => {
      console.log(response.Result)
      delete response.Result.languageId;
      this.languageForm.setValue(response.Result);
    });

  }
  get f(): any { return this.languageForm.controls; }

  saveLanguage() {
    this.submitted = true;
    if (this.languageForm.valid) {

      const language: Language = this.languageForm.value;
      if (this.languageId) {
        this.languageService.editLanguages(this.languageId, language).subscribe((response: any) => {
          if (response.StatusCode == 200) {
            this.toastrService.notify('success', 'Success!', response.Message);
            this.router.navigate(['languages']);
          } else {
            this.toastrService.notify('error', 'Error!', response.Message);
          }
        })
      } else {
        this.languageService.addLanguages(language).subscribe((response: any) => {
          if (response.StatusCode == 200) {
            this.toastrService.notify('success', 'Success!', response.Message);
            this.router.navigate(['languages']);
          } else {
            this.toastrService.notify('error', 'Error!', response.Message);
          }
        })
      }
    }
  }

}
