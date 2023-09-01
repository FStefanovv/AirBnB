import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccommodationRecommendationsComponent } from './accommodation-recommendations.component';

describe('AccommodationRecommendationsComponent', () => {
  let component: AccommodationRecommendationsComponent;
  let fixture: ComponentFixture<AccommodationRecommendationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccommodationRecommendationsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AccommodationRecommendationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
