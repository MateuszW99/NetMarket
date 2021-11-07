import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerServiceFaqComponent } from './customer-service-faq.component';

describe('CustomerServiceFaqComponent', () => {
  let component: CustomerServiceFaqComponent;
  let fixture: ComponentFixture<CustomerServiceFaqComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerServiceFaqComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerServiceFaqComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
