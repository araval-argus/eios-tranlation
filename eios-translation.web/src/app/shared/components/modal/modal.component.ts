import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'eios-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent {

  @Input() modalName: string;
  @Input() modalToDisplay: string;
  @Input() modalTitle: string;
  @Input() modalDescription: string;
  @Input() modalBtnConfirmLabel: string;
  @Input() modalBtnDiscardLabel: string;
  
  @Output() modalToggle: EventEmitter<{modalName: string}> = new EventEmitter<{modalName: string}>();

  //TimeFrame BTN - Modal Toggle
  btnModalClose(event: MouseEvent): void{
    event.stopPropagation();
    this.modalToggle.emit({modalName: ''});
  }

}
