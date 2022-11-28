import { Component, OnInit } from '@angular/core';
import { LabelGroupService } from 'src/app/services/label-group.service';

@Component({
  selector: 'ts-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.css']
})
export class GroupComponent implements OnInit {

  groups = [];
  constructor(
    private labelGroupService: LabelGroupService
  ) { }

  ngOnInit(): void {
    this.getGroups();
  }
  getGroups() {
    this.labelGroupService.getLabelGroups().subscribe((response: any) => {
      if (response.StatusCode == 200) {
        this.groups = response.Result;
      }
    });
  }
}
