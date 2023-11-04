import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { UserserviceService } from '../Services/userservice.service';
import { LoginModel } from '../Models/LoginModel';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  
  constructor(private userservice: UserserviceService,private router: Router) {}

  login(formData: any){

    this.userservice.DoLogin(new LoginModel(formData.email,formData.password)).subscribe(
      data => {
      var token = data["token"];
      this.router.navigate(["chatRoom"]);
    },
    (error) => {

      console.error('Error:', error);
    }
    
    );
   
  }

}
