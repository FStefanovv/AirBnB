import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DisplayRecommendationsComponent } from './display-recommendations.component';

describe('DisplayRecommendationsComponent', () => {
  let component: DisplayRecommendationsComponent;
  let fixture: ComponentFixture<DisplayRecommendationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DisplayRecommendationsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DisplayRecommendationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
