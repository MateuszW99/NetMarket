import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteSupervisorComponent } from './delete-supervisor.component';

describe('DeleteSupervisorComponent', () => {
  let component: DeleteSupervisorComponent;
  let fixture: ComponentFixture<DeleteSupervisorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteSupervisorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteSupervisorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
