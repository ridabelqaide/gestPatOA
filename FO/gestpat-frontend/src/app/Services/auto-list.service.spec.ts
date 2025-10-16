import { TestBed } from '@angular/core/testing';

import { AutoListService } from './auto-list.service';

describe('AutoListService', () => {
  let service: AutoListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AutoListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
