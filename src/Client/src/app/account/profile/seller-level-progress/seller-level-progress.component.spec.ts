import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerLevelProgressComponent } from './seller-level-progress.component';

describe('SellerLevelProgressComponent', () => {
  let component: SellerLevelProgressComponent;
  let fixture: ComponentFixture<SellerLevelProgressComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellerLevelProgressComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SellerLevelProgressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
