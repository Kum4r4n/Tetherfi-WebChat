import { Component, OnInit } from '@angular/core';
import { TokenService } from '../Services/token.service';
import { Router } from '@angular/router';
import { UserserviceService } from '../Services/userservice.service';
import { map } from 'rxjs';
import { UserModel } from '../Models/UserModel';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  user: UserModel = new UserModel;

  constructor(private tokenService: TokenService, private router : Router, private userService : UserserviceService){}
  
  ngOnInit(): void {
    var token = this.tokenService.getToken();
    if(token == null){
      this.router.navigate(["login"]);
    }

    this.GetUserData();

  }

  logout(){
    this.tokenService.removeToken();
    this.router.navigate(["login"]);
  }

  updateProfile(formData: any){
    this.userService.Update(formData).subscribe(
      data => {
      this.user.id = data["id"];
      this.user.name = data["name"];
      this.user.email = data["email"];
    },
    (error) => {

      console.error('Error:', error);
    }
    
    );
    this.router.navigate(["chatRoom"]);
  }

  GetUserData() {
    this.userService.GetUser().subscribe(
      data => {
      this.user.id = data["id"];
      this.user.name = data["name"];
      this.user.email = data["email"];
    },
    (error) => {

      console.error('Error:', error);
    }
    
    );
  }

}
