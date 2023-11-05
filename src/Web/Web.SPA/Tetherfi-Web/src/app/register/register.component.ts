import { Component, OnInit } from '@angular/core';
import { UserserviceService } from '../Services/userservice.service';
import { Router } from '@angular/router';
import { RegisterModel } from '../Models/RegisterModel';
import { TokenService } from '../Services/token.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent  implements OnInit{

  constructor(private userservice: UserserviceService,private router: Router, private tokenService : TokenService) {}

  ngOnInit(): void {
    var token = this.tokenService.getToken();
    if(token != null){
      this.router.navigate(["chatRoom"]);
    }
  }


  register(formData: any){

    this.userservice.Register(new RegisterModel(formData.name, formData.email,formData.password)).subscribe(
      data => {
      var token = data["token"];
      this.tokenService.setToken(token);
      this.router.navigate(["chatRoom"]);
    },
    (error) => {

      console.error('Error:', error);
    }
    
    );
    
  }
}
