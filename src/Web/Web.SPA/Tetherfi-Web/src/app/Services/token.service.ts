import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

    private tokenKey = 'authToken';
    private chatRoomKey = 'chatRoomKey';

    constructor() { }

    setToken(token: string): void {
        localStorage.setItem(this.tokenKey, token);
    }

    getToken(): string | null {
        return localStorage.getItem(this.tokenKey);
    }

    removeToken(): void {
        localStorage.removeItem(this.tokenKey);
    }

    setChatRoomId(chatRoomId: string): void {
        localStorage.setItem(this.chatRoomKey, chatRoomId);
    }

    getChatRoomId(): string | null {
        return localStorage.getItem(this.chatRoomKey);
    }





}
