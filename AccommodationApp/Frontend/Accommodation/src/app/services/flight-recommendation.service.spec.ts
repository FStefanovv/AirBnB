import { TestBed } from '@angular/core/testing';

import { FlightRecommendationService } from './flight-recommendation.service';

describe('FlightRecommendationService', () => {
  let service: FlightRecommendationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FlightRecommendationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
