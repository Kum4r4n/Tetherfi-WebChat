import { Component } from '@angular/core';
import { UserserviceService } from '../Services/userservice.service';
import { Router } from '@angular/router';
import { RegisterModel } from '../Models/RegisterModel';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  constructor(private userservice: UserserviceService,private router: Router) {}


  register(formData: any){

    this.userservice.Register(new RegisterModel(formData.name, formData.email,formData.password)).subscribe(
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
