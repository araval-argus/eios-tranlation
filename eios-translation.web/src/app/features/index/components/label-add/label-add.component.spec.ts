import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LabelAddComponent } from './label-add.component';

describe('LabelAddComponent', () => {
  let component: LabelAddComponent;
  let fixture: ComponentFixture<LabelAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LabelAddComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LabelAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
