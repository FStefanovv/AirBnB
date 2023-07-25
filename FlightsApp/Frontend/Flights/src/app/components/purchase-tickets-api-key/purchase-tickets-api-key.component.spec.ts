import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseTicketsApiKeyComponent } from './purchase-tickets-api-key.component';

describe('PurchaseTicketsApiKeyComponent', () => {
  let component: PurchaseTicketsApiKeyComponent;
  let fixture: ComponentFixture<PurchaseTicketsApiKeyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PurchaseTicketsApiKeyComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PurchaseTicketsApiKeyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
