import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection } from '@microsoft/signalr';
import { TokenService } from './token.service';
import * as signalR from '@microsoft/signalr';
import { Message } from '../Models/Message';
import { ChatUserModel } from '../Models/ChatUserModel';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
  })
  export class SignalingService {

    private hubConnection: HubConnection | undefined;
    private headers: HttpHeaders | undefined;
    private token: string | null = '';

    private messagesSubject = new Subject<Message>();
    public messages$ = this.messagesSubject.asObservable();

    constructor(private tokenService : TokenService){
        this.headers = new HttpHeaders();
        this.headers = this.headers.set('Content-Type', 'application/json');
        this.headers = this.headers.set('Accept', 'application/json');
        
    }

    init(): void {
        
        this.token = this.tokenService.getToken();
        this.initHub();
       
    }

    sendMessage(message : Message, partnerId : any){
        if (this.hubConnection) {
            this.hubConnection.invoke('SendSignal', message, partnerId);
          }
    }


    private initHub(): void {
       
     
        const tokenApiHeader = 'Bearer ' + this.token;
        this.headers = this.headers!.append('Authorization', tokenApiHeader);
        console.log(tokenApiHeader)
        let tokenValue = '?token=' + this.token;
   
        const url = 'http://localhost:2592/api/signal-signalr/';
   
        this.hubConnection = new signalR.HubConnectionBuilder()
          .withUrl(`${url}signalhub${tokenValue}`,{
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
          })
          .configureLogging(signalR.LogLevel.Information)
          .build();
   
        this.hubConnection.start().then(() => {

            console.log("Connected sucessfull")

            //apply initial run
         
        }).catch((err) => console.error(err.toString()));

        this.hubConnection.on('ReceiveSignal', (message: Message, chatUserModel : ChatUserModel) => {
            console.log('DMS: Signal received : ' + message.type);
            this.messagesSubject.next(message);
          });
    }
    
  }