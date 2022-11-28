import { Component, OnInit } from '@angular/core';
import { Toast, ToastrService, ToastPackage } from 'ngx-toastr';

@Component({
  selector: 'eios-toast-custom',
  templateUrl: './toast-custom.component.html',
  styleUrls: ['./toast-custom.component.css']
})
export class ToastCustomComponent implements OnInit {

  constructor(
    protected toastrService: ToastrService,
    public toastPackage: ToastPackage,
  ) {
  }

  ngOnInit(): void {
  }

}
