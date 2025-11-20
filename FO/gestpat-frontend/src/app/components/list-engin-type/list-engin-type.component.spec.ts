import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListEnginTypeComponent } from './list-engin-type.component';

describe('ListEnginTypeComponent', () => {
  let component: ListEnginTypeComponent;
  let fixture: ComponentFixture<ListEnginTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListEnginTypeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListEnginTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
