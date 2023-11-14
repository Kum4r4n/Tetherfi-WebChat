import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, map } from 'rxjs';
import { TokenService } from './token.service';

@Injectable({
    providedIn: 'root'
  })
  export class HttpService {

    private users = new BehaviorSubject<any[]>([]);
    userData$ = this.users.asObservable();

    chats = new BehaviorSubject<any[]>([]);
    chatDatda$ = this.chats.asObservable();

    constructor(private http: HttpClient, private tokenService : TokenService) { }


    AddMessage(data : any){

      var token = this.tokenService.getToken();

      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      });

      const requestOptions = {
        headers: headers
      };

      return this.http.post<any>("http://localhost:2592/api/message/"+"api/chat" , data ,requestOptions).pipe(map(p=>{
        
      }));

    }


    GetChatList(partnerId : any){

      var token = this.tokenService.getToken();

      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      });

      const requestOptions = {
        headers: headers
      };

      var chaArra : any[] = [];

      return this.http.get<any>("http://localhost:2592/api/message/"+"api/Users/Chatroom/"+ partnerId ,requestOptions).pipe(map(p=>{
        Object.keys(p.chats).forEach(key=> {
          chaArra.push(p.chats[key]);
        });

        var soredChats = chaArra.sort((a, b) => new Date(a.createdDateTime).getTime() - new Date(b.createdDateTime).getTime());
        this.chats.next(soredChats);
        return chaArra;
      }));



    }

    GetUserList(){
      var token = this.tokenService.getToken();

      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      });

      const requestOptions = {
        headers: headers
      };

      var usArra : any[] = [];

      return this.http.get<any>("http://localhost:2592/api/message/"+"api/Users",requestOptions).pipe(map(p=>{
        Object.keys(p).forEach(key=> {
          usArra.push(p[key]);
        });

        this.users.next(usArra);
        return usArra;
      }));
    }

  }