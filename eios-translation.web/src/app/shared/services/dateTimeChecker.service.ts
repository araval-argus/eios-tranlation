import {  Injectable } from '@angular/core';
import * as moment from 'moment';

@Injectable()
export class dateTimeChecker {
  constructor() {
  }
  dateValid(date: string, format: string): boolean {
    return moment(date, format,true).isValid();
  }
  dateNoGreaterThan(dateVal: string, dateCompare?: string): boolean{
    let validDate: boolean = true;
    if(dateCompare){
      let dateInput = moment(dateVal, 'DD/MM/YYYY');
      let dateInputCompare = moment(dateCompare, 'DD/MM/YYYY');
      validDate = dateInput.isBefore(dateInputCompare);
    }
    return validDate;
  }
  dateNoLessThan(dateVal: string, dateCompare?: string): boolean{
    let validDate: boolean = true;
    if(dateCompare){
      let dateInput = moment(dateVal, 'DD/MM/YYYY');
      let dateInputCompare = moment(dateCompare, 'DD/MM/YYYY');
      validDate = dateInput.isAfter(dateInputCompare);
    }
    return validDate;
  }
}