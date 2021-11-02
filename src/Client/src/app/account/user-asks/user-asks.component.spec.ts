import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAsksComponent } from './user-asks.component';

describe('UserAsksComponent', () => {
  let component: UserAsksComponent;
  let fixture: ComponentFixture<UserAsksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserAsksComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAsksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
