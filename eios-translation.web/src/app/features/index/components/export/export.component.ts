import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { LabelService } from 'src/app/services/label.service';
import { LanguageService } from 'src/app/services/language.service';
import { ToasterService } from 'src/app/shared/services/toaster.service';
import * as FileSaver from 'file-saver';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'ts-export',
  templateUrl: './export.component.html',
  styleUrls: ['./export.component.css'],
})
export class ExportComponent implements OnInit {
  languages = [];
  languageId;
  submitted = false;
  inProgress = false;
  responseDate = [];
  fileUrl;
  urlNew = 'http://192.168.1.15:8080';

  newFileURL = 'http://192.168.1.15:8080/resources/1669210241121.json';

  backupType: string; // 'bookmarks' | 'snippets';
  blobUrl: any;
  sanitizedBlobUrl: any;
  filename: string;
  backendURL;
  languageForm: FormGroup = new FormGroup({
    languageId: new FormControl('', [Validators.required]),
  });
  constructor(
    private labelService: LabelService,
    private toastrService: ToasterService,
    public ref: DynamicDialogRef,
    private languageService: LanguageService,
  ) {
    this.backendURL = environment.backendURL

  }

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

  get l(): any {
    return this.languageForm.controls;
  }

  exportExcel() {
    this.submitted = true;
    if (this.languageForm.valid) {
      this.labelService
        .exportLabel(this.languageForm.get('languageId').value)
        .subscribe((response: any) => {
          var exportFile =  response.Result;
          FileSaver.saveAs(`${this.backendURL}${exportFile}`, exportFile.split("\\").pop());
        });
    }
  }

}