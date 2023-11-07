import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatRoomViewComponent } from './chat-room-view.component';

describe('ChatRoomViewComponent', () => {
  let component: ChatRoomViewComponent;
  let fixture: ComponentFixture<ChatRoomViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ChatRoomViewComponent]
    });
    fixture = TestBed.createComponent(ChatRoomViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
