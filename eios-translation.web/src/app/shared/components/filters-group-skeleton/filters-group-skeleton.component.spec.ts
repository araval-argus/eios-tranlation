import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FiltersGroupSkeletonComponent } from './filters-group-skeleton.component';

describe('FiltersGroupSkeletonComponent', () => {
  let component: FiltersGroupSkeletonComponent;
  let fixture: ComponentFixture<FiltersGroupSkeletonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FiltersGroupSkeletonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FiltersGroupSkeletonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
