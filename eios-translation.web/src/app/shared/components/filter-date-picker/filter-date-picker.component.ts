import { style } from '@angular/animations';
import { CalendarService } from './../../services/calendars.service';
import { distinctUntilChanged, debounceTime, filter, tap } from 'rxjs/operators';
import { AfterViewInit, Component, Input, OnInit, ViewChild, OnChanges, SimpleChanges, Output, EventEmitter, OnDestroy } from '@angular/core';
import { IDatePickerConfig } from 'ng2-date-picker';
import * as moment from 'moment';
//Services
import { dateTimeChecker } from './../../services/dateTimeChecker.service';
import { fromEvent } from 'rxjs';


@Component({
  selector: 'eios-filter-date-picker',
  templateUrl: './filter-date-picker.component.html',
  styleUrls: ['./filter-date-picker.component.css']
})
export class FilterDatePickerComponent implements OnInit, OnChanges, AfterViewInit, OnDestroy {

  @Input() inputName?: string;
  @Input() valueRange?: string;
  @Input() placeHolder?: string;
  @Input() inputClassName: string = 'text-field-sidebar text-field-button';
  @Input() btnClassName: string = 'btn-calendar-find';
  @Input() inputPlaceHolder?: string = "DD/MM/YYYY";
  @Input() scrollEvent?: string = null;
  @Input() alertFeedback?: string = "false";
  
  @Output() dateInput: EventEmitter<{dateTime: string, valueRange: string}> = new EventEmitter<{dateTime: string, valueRange: string}>();ù
  @Output() errorEmitter?: EventEmitter<{errorTimeStamp: string}> = new EventEmitter<{errorTimeStamp: string}>
 
  @ViewChild('datePicker') picker: any;

  //Input-Fields
  inputDate;
  btnCalendar;

  calendarObs$;
  calendarWraper;

  public config: IDatePickerConfig;
  public showDatepicker: boolean = false;
  public inputDateValue: string;
  public topPosition?: string;
  public leftPostion?: string;
  public dateValid: boolean = true;

  curDate: moment.Moment;

  constructor(
    private dateCheckerService: dateTimeChecker,
    private _calendarService: CalendarService
  ){}

  //Life Hooks
  ngOnInit() {
    if (!this.curDate) {
      this.curDate = moment.utc().startOf('day');
    }
    this.config = {
      format: 'DD/MM/YYYY',
      monthFormat: 'MMMM YYYY',
      max: moment(Date.now()).format('DD/MM/YYYY'),
      allowMultiSelect: false,
      weekDayFormat: 'dd',
      showNearMonthDays: true, //the next and previous month days are visible
      yearFormat: 'YYYY',
      showGoToCurrent: false,
      monthBtnFormat: 'MMM',
    };
  };
  ngAfterViewInit(){
    this.inputDate = document.getElementById(this.inputName) as HTMLInputElement;
    this.btnCalendar = document.getElementById('btn' + this.inputName) as HTMLButtonElement;

    fromEvent(this.inputDate, 'keyup')
      .pipe(
        filter(Boolean),
        debounceTime(1000),
        distinctUntilChanged()
    ).subscribe(() => {
        const dateTimeInput: string = this.inputDate.value;
        if(dateTimeInput){
          this.dateValid = this.dateCheckerService.dateValid(dateTimeInput, 'DD/MM/YYY');
          if(!this.dateValid){
            this.alertAdd();
            this.errorEmitter.emit({errorTimeStamp: Date.now().toString()});
          }else{
            this.alertRemove();
            this.dateInput.emit({dateTime: dateTimeInput, valueRange: this.valueRange});
            this.close();
          }
        }
    });

    this.calendarWraper = document.getElementById('calendarWrapper') as HTMLElement;
    document.body.appendChild(this.calendarWraper);
    setTimeout(() => {
      this.calendarFinder()
    }, 200);

    this.calendarObs$ = this._calendarService.show$.subscribe(x => {
      this.close();
      this.calendarWraper.style.display = 'none'
    })
  };
  ngOnChanges(changes: SimpleChanges): void {
    if(changes.scrollEvent && (!changes.scrollEvent.currentValue !== changes.scrollEvent.previousValue)){
      this.calendarFinder();
    } 
    if(changes.alertFeedback && (changes.alertFeedback.currentValue === "true")){
      this.alertAdd();
    }
    if(changes.alertFeedback && (changes.alertFeedback.currentValue === "false")){
      this.alertRemove();
    }
  };

  ngOnDestroy(): void {
    this.calendarObs$.unsubscribe();
  }

  onDateChange(event) {
    let dateTime = moment(new Date(event.date.$d)).format('DD/MM/YYYY');
    if(this.dateCheckerService.dateValid(dateTime, 'DD/MM/YYY')){
      this.inputDateValue = dateTime;
      this.dateInput.emit({dateTime, valueRange: this.valueRange});
      this.close();
    };
  }
  open() {
    this.calendarFinder();
    this.showDatepicker = true;
  }
  close() {
    this.showDatepicker = false;
  }
  toggle() {
    this.calendarFinder();
    this.showDatepicker = !this.showDatepicker;
  }

  onClickedOutside(event) {
    if (this.showDatepicker) {
      this.close();
    }
  }
  calendarFinder(){
    //console.log('finder')
    this.topPosition = (this.inputDate?.getBoundingClientRect().y + 32).toString() + 'px'
    this.leftPostion = this.inputDate?.getBoundingClientRect().x.toString() + 'px'
  }
  calendarPosition(){
    let displayVal = this.showDatepicker ? 'block' : 'none';
    return {
      top: this.topPosition,
      left: this.leftPostion,
      display: displayVal
    }
  }

  alertAdd(): void{
    if(this.inputDate && this.btnCalendar){
      this.inputDate.classList.add('alert');
      this.btnCalendar.classList.add('alert');
    }
  }
  alertRemove(): void{
    if(this.inputDate && this.btnCalendar){
      this.inputDate.classList.remove('alert')
      this.btnCalendar.classList.remove('alert');
    }
  }

}
