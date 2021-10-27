import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BidsComponent } from './bids.component';

describe('BuyingComponent', () => {
  let component: BidsComponent;
  let fixture: ComponentFixture<BidsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BidsComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BidsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
