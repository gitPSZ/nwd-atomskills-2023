import { TestBed } from '@angular/core/testing';

import { AgileInterfaceService } from './agile-interface.service';

describe('AgileInterfaceService', () => {
  let service: AgileInterfaceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgileInterfaceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
