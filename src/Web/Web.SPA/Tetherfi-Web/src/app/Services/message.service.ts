import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection } from '@microsoft/signalr';
import { TokenService } from './token.service';
import * as signalR from '@microsoft/signalr';
import { ChatUserModel } from '../Models/ChatUserModel';
import * as directMessagesAction from '../Actions/directmessage.action';
import { Store } from '@ngrx/store';
import { BehaviorSubject, Observable } from 'rxjs';
import { SendMessage } from '../Models/SendMessage';


@Injectable({
  providedIn: 'root'
})
export class MessageService {

    private hubConnection: HubConnection | undefined;
    private headers: HttpHeaders | undefined;
    private token: string | null = '';

    
    private users = new BehaviorSubject<any[]>([]);
    data$ = this.users.asObservable();

    private chatsB = new BehaviorSubject<any[]>([]);
    chats$ = this.chatsB.asObservable();

    chatMessages : any[] = [];

    constructor(private tokenService : TokenService,private store: Store<any>){
        this.headers = new HttpHeaders();
        this.headers = this.headers.set('Content-Type', 'application/json');
        this.headers = this.headers.set('Accept', 'application/json');
        //this.init();
    }

    init(): void {
        
        this.token = this.tokenService.getToken();
        this.initHub();
       
    }

    leave(): void {
      if (this.hubConnection) {
        this.hubConnection.invoke('Leave');
      }
    }
  
    join(): void {
      console.log('DMS: send join');
      if (this.hubConnection) {
        this.hubConnection.invoke('Join');
      }
    }

    SendDirectMessage(model : SendMessage): void {

      this.chatMessages.push(this.chatsB.value)
      this.chatMessages.push(model.message);

      this.chatsB.next(this.chatMessages );
      this.chatMessages = [];

      console.log('DMS: send join');
      if (this.hubConnection) {
        this.hubConnection.invoke('SendDirectMessage', model.message, model.targetUserId);
      }
    }

   
    private initHub(): void {
       
     
          const tokenApiHeader = 'Bearer ' + this.token;
          this.headers = this.headers!.append('Authorization', tokenApiHeader);
          console.log(tokenApiHeader)
          let tokenValue = '?token=' + this.token;
     
          const url = 'https://localhost:7265/';
     
          this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${url}userchathub${tokenValue}`,{
              skipNegotiation: true,
              transport: signalR.HttpTransportType.WebSockets
            })
            .configureLogging(signalR.LogLevel.Information)
            .build();
     
          this.hubConnection.start().catch((err) => console.error(err.toString()));
     
          this.hubConnection.on('NewOnlineUser', (onlineUser: ChatUserModel) => {
            console.log('DMS: NewOnlineUser received');
            console.log(onlineUser);
            this.store.dispatch(
              directMessagesAction.receivedNewOnlineUserAction({
                payload: onlineUser,
              })
            );
          });
     
          this.hubConnection.on('OnlineUsers', (onlineUsers: ChatUserModel[]) => {
            console.log('DMS: OnlineUsers received');
            console.log(onlineUsers);
            this.users.next(onlineUsers);
            // this.store.dispatch(
            //   directMessagesAction.receivedOnlineUsersAction({
            //     payload: onlineUsers,
            //   })
            // );
          });
     
          this.hubConnection.on('Joined', (onlineUser: ChatUserModel) => {
            console.log('DMS: Joined received');
            console.log(onlineUser);
          });
     
          this.hubConnection.on(
            'SendDM',
            (message: string, user: ChatUserModel) => {
              console.log('DMS: SendDM received');
              this.chatMessages.push(this.chatsB.value)
              this.chatMessages.push(message);
              this.chatsB.next(this.chatMessages );
              this.chatMessages = [];
            }
          );
     
          this.hubConnection.on('UserLeft', (name: string) => {
            console.log('DMS: UserLeft received');
            this.store.dispatch(
              directMessagesAction.receivedUserLeftAction({ payload: name })
            );
          });
        
      }
}