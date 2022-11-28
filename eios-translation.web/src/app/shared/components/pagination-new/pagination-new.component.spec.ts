import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaginationNewComponent } from './pagination-new.component';

describe('PaginationNewComponent', () => {
  let component: PaginationNewComponent;
  let fixture: ComponentFixture<PaginationNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PaginationNewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PaginationNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
