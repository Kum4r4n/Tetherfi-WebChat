import { Injectable } from '@angular/core';
import { LoginModel } from '../Models/LoginModel';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { UserModel } from '../Models/UserModel';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class UserserviceService {

  constructor(private http: HttpClient, private tokenService : TokenService) { }

  DoLogin(loginModel: any) : Observable<any>{

    return this.http.post("http://localhost:2592/api/identity/"+"api/User/Login", loginModel);
  }

  Register(register : any) : Observable<any>{

    console.log("Register");

    return this.http.post("http://localhost:2592/api/identity/"+"api/User/Register", register);
  }


  GetUser() : Observable<any>  {

    var token = this.tokenService.getToken();

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    const requestOptions = {
      headers: headers
    };
    
    return this.http.get("http://localhost:2592/api/identity/"+"api/User/Profile",requestOptions);
  }

  Update(data : any) : Observable<any>  {
    var token = this.tokenService.getToken();

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    const requestOptions = {
      headers: headers
    };

    return this.http.put("http://localhost:2592/api/identity/"+"api/User/Update",data,requestOptions);

  }

}
