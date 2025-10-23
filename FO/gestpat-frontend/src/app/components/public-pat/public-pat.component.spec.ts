import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicPatComponent } from './public-pat.component';

describe('PublicPatComponent', () => {
  let component: PublicPatComponent;
  let fixture: ComponentFixture<PublicPatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PublicPatComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PublicPatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
