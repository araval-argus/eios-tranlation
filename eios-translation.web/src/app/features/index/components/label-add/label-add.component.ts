import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { Label } from 'src/app/models/label.model';
import { LabelGroupService } from 'src/app/services/label-group.service';
import { LabelService } from 'src/app/services/label.service';
import { ToasterService } from 'src/app/shared/services/toaster.service';

@Component({
  selector: 'ts-label-add',
  templateUrl: './label-add.component.html',
  styleUrls: ['./label-add.component.css']
})
export class LabelAddComponent implements OnInit {
  groups = [];
  submitted = false;
  inProgress = false;
  labelForm: FormGroup = new FormGroup({
    resourceId: new FormControl('', Validators.required),
    labelValue: new FormControl('', Validators.required),
    fK_LabelGroupId: new FormControl('', Validators.required)
  });

  constructor(
    private labelGroupService: LabelGroupService,
    private labelService: LabelService,
    private toastrService: ToasterService,
    public ref: DynamicDialogRef
  ) {

  }

  ngOnInit(): void {
    this.getGroups();
  }

  getGroups() {
    this.labelGroupService.getLabelGroups().subscribe((response: any) => {
      this.groups = response.Result;
    });
  }

  get f(): any { return this.labelForm.controls; }
  
  saveLabel() {
    
    this.submitted = true;
    if (this.labelForm.valid) {
      this.inProgress = true;
      const group: Label = this.labelForm.value;

      this.labelService.addLabel(group).subscribe(response => {
        if (response.StatusCode == 200) {
          this.toastrService.notify('success', 'Success!', response.Message);
          this.inProgress = false;
          this.ref.close();
        } else {
          this.toastrService.notify('error', 'Error!', response.Message);
          this.inProgress = false;
        }
      })
    }
  }
}

