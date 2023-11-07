import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

import { BrowserModule } from '@angular/platform-browser';
import { FlexLayoutModule } from "@angular/flex-layout";
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ChatroomComponent } from './chatroom/chatroom.component';
import { HttpClientModule } from '@angular/common/http';
import { ProfileComponent } from './profile/profile.component';
import { StoreModule } from '@ngrx/store';
import { directMessageStoreName } from './Actions/directmessages.selectors';
import { directMessagesReducer } from './Actions/directmessages.reducer';
import { MessageService } from './Services/message.service';
import { ChatRoomViewComponent } from './chat-room-view/chat-room-view.component';




@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    ChatroomComponent,
    ProfileComponent,
    ChatRoomViewComponent

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FlexLayoutModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatCardModule,
    HttpClientModule,
    StoreModule.forRoot({}),
    StoreModule.forFeature(directMessageStoreName, directMessagesReducer),
  ],
  providers: [MessageService],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  exports : [ChatroomComponent]
})
export class AppModule { }
