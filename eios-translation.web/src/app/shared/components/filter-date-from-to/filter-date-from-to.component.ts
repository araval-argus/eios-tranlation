import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { dateTimeChecker } from './../../services/dateTimeChecker.service';

@Component({
  selector: 'eios-filter-date-from-to',
  templateUrl: './filter-date-from-to.component.html',
  styleUrls: ['./filter-date-from-to.component.css']
})
export class FilterDateFromToComponent{

  @Input() scrollEvent?: string = null;
  @Input() dateFromToFeedback?: string[] = [];
  @Output() dateInputAction: EventEmitter<{dateFrom: string, dateTo: string}> = new EventEmitter<{dateFrom: string, dateTo: string}>();

  //To
  dateToValue: string;
  alertTo: string = "false";
  //From
  dateFromValue: string;
  alertFrom: string = "false";

  constructor(
    private dateCheckerService: dateTimeChecker,
    private toastr: ToastrService
  ){}

  dateInputChecker({ dateTime, valueRange}): void{
    let dateToTemp;
    let dateFromTemp;
    //DateFrom
    if(valueRange === 'dateFrom'){
      dateFromTemp = dateTime;
      let dateValid = this.dateCheckerService.dateNoGreaterThan(dateFromTemp, this.dateToValue);
      if(dateValid){
        this.alertFrom = "false";
        this.dateFromValue = dateFromTemp;
        this.dateInputAction.emit({dateFrom: this.dateFromValue, dateTo: this.dateToValue});
      }else{
        this.alertFrom = "true";
        if(this.dateFromToFeedback.length > 0){
          this.toastr.error(this.dateFromToFeedback[1]);
          this.dateFromValue = null;
        }
      }
    };
    //DateTo
    if(valueRange === 'dateTo'){
      dateToTemp = dateTime;
      let dateValid = this.dateCheckerService.dateNoLessThan(dateToTemp, this.dateFromValue);
      if(dateValid){
        this.alertTo = "false";
        this.dateToValue = dateToTemp;
      }else{
        this.alertTo = "true";
        if(this.dateFromToFeedback.length > 0){
          this.toastr.error(this.dateFromToFeedback[2])
          this.dateToValue = null;
        }
      }
    };
  }

  errorEmitterInput({ errorTimeStamp }): void{
    this.toastr.error(this.dateFromToFeedback[0]);
  }



}
