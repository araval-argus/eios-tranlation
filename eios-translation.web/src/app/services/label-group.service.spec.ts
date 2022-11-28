import { TestBed } from '@angular/core/testing';

import { LabelGroupService } from './label-group.service';

describe('LabelGroupService', () => {
  let service: LabelGroupService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LabelGroupService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
