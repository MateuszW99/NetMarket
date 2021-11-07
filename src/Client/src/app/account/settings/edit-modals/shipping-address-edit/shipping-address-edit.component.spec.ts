import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShippingAddressEditComponent } from './shipping-address-edit.component';

describe('ShippingAddressEditComponent', () => {
  let component: ShippingAddressEditComponent;
  let fixture: ComponentFixture<ShippingAddressEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShippingAddressEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShippingAddressEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
