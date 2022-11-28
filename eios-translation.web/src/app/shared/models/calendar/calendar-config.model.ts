import {IBaseCalendarConfig} from './calendar-month-config.model';
import {Moment} from 'moment';

export interface ICalendarConfig extends IBaseCalendarConfig {
  calendarsAmount?: number;
  min?: Moment | string;
  max?: Moment | string;
  allowMultiSelect?: boolean;
  format?: string;
  monthFormat?: string;
  monthFormatter?: (date: Moment) => string;
}
