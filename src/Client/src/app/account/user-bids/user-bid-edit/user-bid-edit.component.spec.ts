import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserBidEditComponent } from './user-bid-edit.component';

describe('BidEditComponent', () => {
  let component: UserBidEditComponent;
  let fixture: ComponentFixture<UserBidEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserBidEditComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBidEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
