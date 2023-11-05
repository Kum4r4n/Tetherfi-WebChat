import { Component, OnInit  } from '@angular/core';
import { TokenService } from '../Services/token.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-chatroom',
  templateUrl: './chatroom.component.html',
  styleUrls: ['./chatroom.component.css']
})
export class ChatroomComponent implements OnInit  {

  constructor(private tokenService: TokenService, private router : Router){}
  
  ngOnInit(): void {
    var token = this.tokenService.getToken();
    if(token == null){
      this.router.navigate(["login"]);
    }
  }

}
