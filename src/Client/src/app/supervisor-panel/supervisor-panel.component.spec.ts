import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupervisorPanelComponent } from './supervisor-panel.component';

describe('SupervisorPanelComponent', () => {
  let component: SupervisorPanelComponent;
  let fixture: ComponentFixture<SupervisorPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupervisorPanelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SupervisorPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
