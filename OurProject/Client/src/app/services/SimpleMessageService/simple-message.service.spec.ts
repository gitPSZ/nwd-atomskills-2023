import { TestBed } from '@angular/core/testing';

import { SimpleMessageService } from './simple-message.service';

describe('SimpleMessageService', () => {
  let service: SimpleMessageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SimpleMessageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
