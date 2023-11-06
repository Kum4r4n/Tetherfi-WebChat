import { Component, OnInit  } from '@angular/core';
import { TokenService } from '../Services/token.service';
import { Router } from '@angular/router';
import { ChatUserModel } from '../Models/ChatUserModel';
import { Observable } from 'rxjs';
import { DirectMessage } from '../Models/DirectMessage';
import { Store, select } from '@ngrx/store';
import * as fromSelectorsStore from '../Actions/directmessages.selectors';
import * as directMessagesAction from '../Actions/directmessage.action';
import { MessageService } from '../Services/message.service';
import { SendMessage } from '../Models/SendMessage';

@Component({
  selector: 'app-chatroom',
  templateUrl: './chatroom.component.html',
  styleUrls: ['./chatroom.component.css']
})
export class ChatroomComponent implements OnInit  {

  public async: any;
  onlineUser: ChatUserModel = { connectionId: '', id: '', name:'' };
  selectedOnlineUserId = '';
  selectedOnlineUserName = '';
  isAuthorized = false;
  message = '';
  onlineUsers$: any[] = [];
  chatMessages$ : any[] = [];
  directMessages$: Observable<DirectMessage[]> = new Observable<DirectMessage[]>();
  // connected$: Observable<boolean>;
  connected = false;

  constructor(private tokenService: TokenService, private router : Router, private store : Store<any>, private messageService : MessageService){

      
      //this.onlineUsers$ = this.store.pipe(select(fromSelectorsStore.selectOnlineUsers));
      this.directMessages$ = this.store.pipe(select(fromSelectorsStore.selectDirectMessages));
      this.store
      .pipe(select(fromSelectorsStore.selectConnected))
      .subscribe((data) => {
        console.log(data);
        this.connected = data;
      });
  }


  Join(){
    this.messageService.join();
  }
  
  ngOnInit(): void {
    var token = this.tokenService.getToken();
    if(token == null){
      this.router.navigate(["login"]);
    }

    this.messageService.data$.subscribe(updatedData => {
      this.onlineUsers$ = updatedData;
    });

    this.messageService.chats$.subscribe(updatedData => {
      this.chatMessages$ = updatedData;
    });
   
  }

  selectChat(onlineuserUser : any): void {
    
    this.selectedOnlineUserId = onlineuserUser.id;
    this.selectedOnlineUserName = onlineuserUser.name;

    //clear chat box
    //load all chats
  }

  sendMessage(): void {
  
    var model = new SendMessage;
    model.targetUserId = this.selectedOnlineUserId;
    model.message = this.message;

    this.message = "";
   
    this.messageService.SendDirectMessage(model);
  }

  getUserInfoName(directMessage: DirectMessage): string {
    if (directMessage.fromOnlineUser) {
      return directMessage.fromOnlineUser.name;
    }
 
    return '';
  }

  disconnect(): void {
    this.store.dispatch(directMessagesAction.leaveAction());
  }
 
  connect(): void {
    this.store.dispatch(directMessagesAction.joinAction());
  }

}
