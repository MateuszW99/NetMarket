import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupervisorTransactionsComponent } from './supervisor-transactions.component';

describe('SupervisorTransactionsComponent', () => {
  let component: SupervisorTransactionsComponent;
  let fixture: ComponentFixture<SupervisorTransactionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupervisorTransactionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SupervisorTransactionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
