import { TestBed } from '@angular/core/testing';

import { CarbookingService } from './carbooking.service';

describe('CarbookingService', () => {
  let service: CarbookingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CarbookingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
