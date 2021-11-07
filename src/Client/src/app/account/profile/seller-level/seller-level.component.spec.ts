import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerLevelComponent } from './seller-level.component';

describe('SellerLevelComponent', () => {
  let component: SellerLevelComponent;
  let fixture: ComponentFixture<SellerLevelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellerLevelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SellerLevelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
