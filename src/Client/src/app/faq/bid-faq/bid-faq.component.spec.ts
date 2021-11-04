import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BidFaqComponent } from './bid-faq.component';

describe('BidFaqComponent', () => {
  let component: BidFaqComponent;
  let fixture: ComponentFixture<BidFaqComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BidFaqComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BidFaqComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
