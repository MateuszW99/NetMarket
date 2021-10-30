import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAskEditComponent } from './user-ask-edit.component';

describe('UserAskEditComponent', () => {
  let component: UserAskEditComponent;
  let fixture: ComponentFixture<UserAskEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserAskEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAskEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
