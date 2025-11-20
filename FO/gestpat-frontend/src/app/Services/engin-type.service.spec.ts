import { TestBed } from '@angular/core/testing';

import { EnginTypeService } from './engin-type.service';

describe('EnginTypeService', () => {
  let service: EnginTypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EnginTypeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
