import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupervisorFiltersComponent } from './supervisor-filters.component';

describe('SupervisorFiltersComponent', () => {
  let component: SupervisorFiltersComponent;
  let fixture: ComponentFixture<SupervisorFiltersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupervisorFiltersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SupervisorFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
