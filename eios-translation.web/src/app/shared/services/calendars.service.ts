import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({ providedIn: 'root'})
export class CalendarService{
initialValue: string = null;
behaviorSubjectSource: BehaviorSubject<string> =  new BehaviorSubject<string>(this.initialValue);
observableForBehaviorSubject: Observable<string> =  this.behaviorSubjectSource.asObservable();

public show$ = this.behaviorSubjectSource.asObservable()

setData(data: string) {
    this.behaviorSubjectSource.next(data);
}
  
}