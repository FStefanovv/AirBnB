import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BuyFlightTicketsComponent } from './buy-flight-tickets.component';

describe('BuyFlightTicketsComponent', () => {
  let component: BuyFlightTicketsComponent;
  let fixture: ComponentFixture<BuyFlightTicketsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BuyFlightTicketsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BuyFlightTicketsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
