import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowAccommodationComponent } from './show-accommodation.component';

describe('ShowAccommodationComponent', () => {
  let component: ShowAccommodationComponent;
  let fixture: ComponentFixture<ShowAccommodationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowAccommodationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShowAccommodationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
