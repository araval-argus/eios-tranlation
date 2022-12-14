import {Moment} from 'moment';
import { WeekDays } from 'ng2-date-picker/lib/common/types/week-days.type';

export interface IBaseCalendarConfig {
  isDisabledCallback?: (date: Moment) => boolean;
  weekdayNames?: {[key: string]: string};
  showNearMonthDays?: boolean;
  showWeekNumbers?: boolean;
  firstDayOfWeek?: WeekDays;
}

export interface ICalendarMonthConfig extends IBaseCalendarConfig {
  month: Moment;
  min?: Moment;
  max?: Moment;
}