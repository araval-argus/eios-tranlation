import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupAddEditComponent } from './group-add-edit.component';

describe('GroupAddEditComponent', () => {
  let component: GroupAddEditComponent;
  let fixture: ComponentFixture<GroupAddEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GroupAddEditComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GroupAddEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
