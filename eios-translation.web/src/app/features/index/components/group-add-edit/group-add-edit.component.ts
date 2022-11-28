import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LabelGroup } from 'src/app/models/groups.model';
import { LabelGroupService } from 'src/app/services/label-group.service';
import { ToasterService } from 'src/app/shared/services/toaster.service';

@Component({
  selector: 'ts-group-add-edit',
  templateUrl: './group-add-edit.component.html',
  styleUrls: ['./group-add-edit.component.css']
})
export class GroupAddEditComponent implements OnInit {

  groups = [];
  submitted = false;
  groupId;
  groupForm: FormGroup = new FormGroup({
    groupName: new FormControl('', Validators.required),
    fK_ParentLableGroupId: new FormControl(),
    labelGroupId: new FormControl()
  });

  constructor(
    private labelGroupService: LabelGroupService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastrService: ToasterService
  ) {
    this.groupId = this.activatedRoute.snapshot.paramMap.get('id');
    if (this.groupId) {
      this.getGroup();
    }
  }

  ngOnInit(): void {
    this.getGroups();
  }

  getGroup() {
    this.labelGroupService.getLabelGroup(this.groupId).subscribe((response: any) => {
      delete response.Result.parentGroup;
      this.groupForm.setValue(response.Result);
    });
  }
  getGroups() {
    this.labelGroupService.getLabelGroups().subscribe((response: any) => {
      this.groups = response.Result;
    });
  }

  get f(): any { return this.groupForm.controls; }

  saveGroup() {
    this.submitted = true;
    if (this.groupForm.valid) {
      const group: LabelGroup = this.groupForm.value;
      if (this.groupId) {
        this.labelGroupService.editLabelGroups(this.groupId, group).subscribe(response => {
          if (response.StatusCode == 200) {
            this.toastrService.notify('success', 'Success!', response.Message);
            this.router.navigate(['groups']);
          } else {

          }
        })
      } else {
        this.labelGroupService.addLabelGroups(group).subscribe(response => {
          if (response.StatusCode == 200) {
            this.toastrService.notify('success', 'Success!', response.Message);
            this.router.navigate(['groups']);
          } else {
            this.toastrService.notify('error', 'Error!', response.Message);
          }
        })
      }
    }
  }
}
