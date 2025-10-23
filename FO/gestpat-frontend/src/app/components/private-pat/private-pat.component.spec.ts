import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivatePatComponent } from './private-pat.component';

describe('PrivatePatComponent', () => {
  let component: PrivatePatComponent;
  let fixture: ComponentFixture<PrivatePatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PrivatePatComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PrivatePatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
