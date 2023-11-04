import { Injectable } from '@angular/core';
import { LoginModel } from '../Models/LoginModel';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserserviceService {

  constructor(private http: HttpClient) { }

  DoLogin(loginModel: any) : Observable<any>{

    return this.http.post("https://localhost:44370/"+"api/User/Login", loginModel);
  }

  Register(register : any) : Observable<any>{

    return this.http.post("https://localhost:44370/"+"api/User/Register", register);
  }

}
