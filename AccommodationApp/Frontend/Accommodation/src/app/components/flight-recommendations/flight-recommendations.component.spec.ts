import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightRecommendationsComponent } from './flight-recommendations.component';

describe('FlightRecommendationsComponent', () => {
  let component: FlightRecommendationsComponent;
  let fixture: ComponentFixture<FlightRecommendationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FlightRecommendationsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FlightRecommendationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
