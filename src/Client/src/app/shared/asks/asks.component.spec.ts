import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsksComponent } from './asks.component';

describe('AsksComponent', () => {
  let component: AsksComponent;
  let fixture: ComponentFixture<AsksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsksComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
