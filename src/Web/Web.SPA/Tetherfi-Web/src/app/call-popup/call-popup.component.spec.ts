import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CallPopupComponent } from './call-popup.component';

describe('CallPopupComponent', () => {
  let component: CallPopupComponent;
  let fixture: ComponentFixture<CallPopupComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CallPopupComponent]
    });
    fixture = TestBed.createComponent(CallPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
