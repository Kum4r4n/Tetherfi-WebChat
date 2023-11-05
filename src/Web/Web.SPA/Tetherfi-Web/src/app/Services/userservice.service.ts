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

    return this.http.post("https://localhost:44370/"+"api/User/Login", loginModel);
  }

  Register(register : any) : Observable<any>{

    return this.http.post("https://localhost:44370/"+"api/User/Register", register);
  }


  GetUser() : Observable<any>  {

    var token = this.tokenService.getToken();

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    const requestOptions = {
      headers: headers
    };
    
    return this.http.get("https://localhost:44370/"+"api/User/Profile",requestOptions);
  }

  Update(data : any) : Observable<any>  {
    var token = this.tokenService.getToken();

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    const requestOptions = {
      headers: headers
    };

    return this.http.put("https://localhost:44370/"+"api/User/Update",data,requestOptions);

  }

}
