import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddStudentsGroupComponent } from './add-students-group.component';

describe('AddStudentsGroupComponent', () => {
  let component: AddStudentsGroupComponent;
  let fixture: ComponentFixture<AddStudentsGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddStudentsGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddStudentsGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
