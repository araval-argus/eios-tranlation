import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LanguagesAddEditComponent } from './languages-add-edit.component';

describe('LanguagesAddEditComponent', () => {
  let component: LanguagesAddEditComponent;
  let fixture: ComponentFixture<LanguagesAddEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LanguagesAddEditComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LanguagesAddEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
