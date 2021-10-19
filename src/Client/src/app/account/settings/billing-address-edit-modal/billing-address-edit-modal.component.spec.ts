import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BillingAddressEditModalComponent } from './billing-address-edit-modal.component';

describe('BillingAddressEditComponent', () => {
  let component: BillingAddressEditModalComponent;
  let fixture: ComponentFixture<BillingAddressEditModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BillingAddressEditModalComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BillingAddressEditModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
