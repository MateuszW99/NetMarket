import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupervisorsTableComponent } from './supervisors-table.component';

describe('SupervisorsTableComponent', () => {
  let component: SupervisorsTableComponent;
  let fixture: ComponentFixture<SupervisorsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupervisorsTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SupervisorsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
