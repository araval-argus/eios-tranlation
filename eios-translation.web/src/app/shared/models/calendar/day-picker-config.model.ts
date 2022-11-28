import {ICalendarConfig} from './calendar-config.model';
import {TDrops, TOpens} from './poistons.type';

export interface IDayPickerConfig extends ICalendarConfig {
  closeOnSelect?: boolean;
  closeOnSelectDelay?: number;
  onOpenDelay?: number;
  disableKeypress?: boolean;
  userValueType?: 'string' | 'object';
  appendTo?: string|HTMLElement;
  drops?: TDrops;
  opens?: TOpens;
}