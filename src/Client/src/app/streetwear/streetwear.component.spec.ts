import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StreetwearComponent } from './streetwear.component';

describe('StreetwearComponent', () => {
  let component: StreetwearComponent;
  let fixture: ComponentFixture<StreetwearComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StreetwearComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StreetwearComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
