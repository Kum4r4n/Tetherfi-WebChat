import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection } from '@microsoft/signalr';
import { TokenService } from './token.service';
import * as signalR from '@microsoft/signalr';
import { ChatUserModel } from '../Models/ChatUserModel';
import * as directMessagesAction from '../Actions/directmessage.action';
import { Store } from '@ngrx/store';


@Injectable({
  providedIn: 'root'
})
export class MessageService {

    private hubConnection: HubConnection | undefined;
    private headers: HttpHeaders | undefined;
    private token: string | null = '';

    constructor(private tokenService : TokenService,private store: Store<any>){
        this.headers = new HttpHeaders();
        this.headers = this.headers.set('Content-Type', 'application/json');
        this.headers = this.headers.set('Accept', 'application/json');
    }

    private init(): void {
        
        this.token = this.tokenService.getToken();
       
    }

    private initHub(): void {
       
     
          const tokenApiHeader = 'Bearer ' + this.token;
          this.headers = this.headers!.append('Authorization', tokenApiHeader);
          console.log(tokenApiHeader)
          let tokenValue = '?token=' + this.token;
     
          const url = 'https://localhost:7265/';
     
          this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${url}userchathub${tokenValue}`)
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
            this.store.dispatch(
              directMessagesAction.receivedOnlineUsersAction({
                payload: onlineUsers,
              })
            );
          });
     
          this.hubConnection.on('Joined', (onlineUser: ChatUserModel) => {
            console.log('DMS: Joined received');
            console.log(onlineUser);
          });
     
          this.hubConnection.on(
            'SendDM',
            (message: string, user: ChatUserModel) => {
              console.log('DMS: SendDM received');
              this.store.dispatch(
                directMessagesAction.receivedDirectMessageForUserAction({
                  payload: {  user, message },
                })
              );
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