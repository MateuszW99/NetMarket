import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupervisorSettingsComponent } from './supervisor-settings.component';

describe('SupervisorSettingsComponent', () => {
  let component: SupervisorSettingsComponent;
  let fixture: ComponentFixture<SupervisorSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupervisorSettingsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SupervisorSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
