import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListAutoInsuranceComponent } from './list-auto-insurance.component';

describe('ListAutoInsuranceComponent', () => {
  let component: ListAutoInsuranceComponent;
  let fixture: ComponentFixture<ListAutoInsuranceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListAutoInsuranceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListAutoInsuranceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
