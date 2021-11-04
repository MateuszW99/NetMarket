import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AskFaqComponent } from './ask-faq.component';

describe('AskFaqComponent', () => {
  let component: AskFaqComponent;
  let fixture: ComponentFixture<AskFaqComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AskFaqComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AskFaqComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
