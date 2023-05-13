import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserInfoChangeComponent } from './user-info-change.component';

describe('UserInfoChangeComponent', () => {
  let component: UserInfoChangeComponent;
  let fixture: ComponentFixture<UserInfoChangeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserInfoChangeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserInfoChangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
