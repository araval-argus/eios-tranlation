import { Component, OnInit } from '@angular/core';
import { Message, MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { ToasterService } from '../toaster.service';

@Component({
  selector: 'ts-toaster',
  templateUrl: './toaster.component.html',
  styleUrls: ['./toaster.component.css'],
  providers: [MessageService]
})
export class ToasterComponent implements OnInit {

  subscription: Subscription;
  constructor(
    private toasterService: ToasterService,
    private messageService: MessageService
  ) { }

  ngOnInit() {
    this.subscribeToNotifications();
  }
  subscribeToNotifications() {
    this.subscription = this.toasterService.notificationChange
      .subscribe(notification => {
        this.messageService.add(notification);
      });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
