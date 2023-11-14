import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { UserserviceService } from '../Services/userservice.service';
import { LoginModel } from '../Models/LoginModel';
import { Router } from '@angular/router';
import { TokenService } from '../Services/token.service';
import { MessageService } from '../Services/message.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit  {
  
  constructor(private toastr: ToastrService, private userservice: UserserviceService,private router: Router, private tokenService : TokenService, private messageService : MessageService) {}

  ngOnInit(): void {
    var token = this.tokenService.getToken();
    if(token != null){
      this.router.navigate(["chatRoom"]);
    }
  }

  login(formData: any){

    this.userservice.DoLogin(new LoginModel(formData.email,formData.password)).subscribe(
      data => {
      var token = data["token"];
      this.tokenService.setToken(token);
      this.messageService.init();
      this.router.navigate(["chatRoom"]);
    },
    (errors) => {

      console.error('Error:', errors);
      this.toastr.info(JSON.stringify(errors));
    }
    
    );
   
  }

}
