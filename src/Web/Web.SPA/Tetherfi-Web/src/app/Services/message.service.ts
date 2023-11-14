import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection } from '@microsoft/signalr';
import { TokenService } from './token.service';
import * as signalR from '@microsoft/signalr';
import { ChatUserModel } from '../Models/ChatUserModel';
import { Store } from '@ngrx/store';
import { BehaviorSubject, Observable } from 'rxjs';
import { SendMessage } from '../Models/SendMessage';
import { ChatModel } from '../Models/ChatModel';
import { HttpService } from './Http.service';


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

    constructor(private tokenService : TokenService,private store: Store<any>, private httpService : HttpService){
        this.headers = new HttpHeaders();
        this.headers = this.headers.set('Content-Type', 'application/json');
        this.headers = this.headers.set('Accept', 'application/json');
        //this.init();
    }

    init(): void {
        
        this.token = this.tokenService.getToken();
        this.initHub();
       
    }

  
    join(): void {
      console.log('DMS: send join');
      if (this.hubConnection) {
        this.hubConnection.invoke('Join');
      }
    }


    SendMessage(data : any){

      if (this.hubConnection) {
        this.hubConnection.invoke('SendMessage', data.message, data.partnerId);
      }

    }


   
    private initHub(): void {
       
     
          const tokenApiHeader = 'Bearer ' + this.token;
          this.headers = this.headers!.append('Authorization', tokenApiHeader);
          console.log(tokenApiHeader)
          let tokenValue = '?token=' + this.token;
     
          const url = 'http://localhost:2592/api/message-signalr/';
     
          this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${url}userchathub${tokenValue}`,{
              skipNegotiation: true,
              transport: signalR.HttpTransportType.WebSockets
            })
            .configureLogging(signalR.LogLevel.Information)
            .build();
     
          this.hubConnection.start().then(() => {
            console.log("Hub is connected");
            this.join();
          }).catch((err) => console.error(err.toString()));

        
     
          this.hubConnection.on('NewOnlineUser', (onlineUser: ChatUserModel) => {
            console.log('DMS: NewOnlineUser received');
            console.log(onlineUser);
            
          });
     
          this.hubConnection.on('ListenMessage', (chatModel: ChatModel) => {
            this.HandleIncomingChat(chatModel);
          });

          
      }

   HandleIncomingChat(chatModel: ChatModel){
      var chatroomIdTemp = this.tokenService.getChatRoomId() ?? "";
      var splits = chatroomIdTemp.split('_')
      if(chatModel.chatRoomId.includes(splits[0]) && chatModel.chatRoomId.includes(splits[1])){

        var olddata = this.httpService.chats.value;
        olddata.forEach(f=> this.chatMessages.push(f));
        this.chatMessages.push(chatModel);
        var soredChats = this.chatMessages.sort((a, b) => new Date(a.createdDateTime).getTime() - new Date(b.createdDateTime).getTime());
        this.httpService.chats.next(soredChats);
        this.chatMessages = [];

      }
   }

   decode(payload : any) {
    return JSON.parse(atob(payload));
  }
}