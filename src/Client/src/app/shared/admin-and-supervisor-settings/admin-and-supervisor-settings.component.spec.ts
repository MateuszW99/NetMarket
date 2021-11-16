import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAndSupervisorSettingsComponent } from './admin-and-supervisor-settings.component';

describe('AdminAndSupervisorSettingsComponent', () => {
  let component: AdminAndSupervisorSettingsComponent;
  let fixture: ComponentFixture<AdminAndSupervisorSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminAndSupervisorSettingsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAndSupervisorSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
