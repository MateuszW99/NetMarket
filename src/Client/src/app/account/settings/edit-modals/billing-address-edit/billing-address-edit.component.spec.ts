import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BillingAddressEditComponent } from './billing-address-edit.component';

describe('BillingAddressEditComponent', () => {
  let component: BillingAddressEditComponent;
  let fixture: ComponentFixture<BillingAddressEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BillingAddressEditComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BillingAddressEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
