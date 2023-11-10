import { Component } from '@angular/core';
import { HttpService } from '../Services/Http.service';
import { TokenService } from '../Services/token.service';
import { MessageService } from '../Services/message.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CallPopupComponent } from '../call-popup/call-popup.component';


@Component({
  selector: 'app-chat-room-view',
  templateUrl: './chat-room-view.component.html',
  styleUrls: ['./chat-room-view.component.css']
})
export class ChatRoomViewComponent {

  onlineUsers$: any[] = [];
  chatData$: any[] = [];
  selecteduserId$ : any;
  SelectedUsername$ : string = "Please select the  user";
  currentUserId$ : any;
  
  
 

  userMessage = '';

  constructor(public dialog: MatDialog, private httpService : HttpService, private tokenService : TokenService, private messageService : MessageService) { }

  ngOnInit(): void {

    this.httpService.GetUserList().subscribe(s=>{});
    this.httpService.userData$.subscribe(updatedData => {
      this.onlineUsers$ = updatedData;
    });

    const token = this.tokenService.getToken() ?? "";
    const payload = token.split('.')[1];
    let data = this.decode(payload);
    this.currentUserId$ = data["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]
    this.messageService.init();
  }

  decode(payload : any) {
    return JSON.parse(atob(payload));
  }


  callVideoCall(){

    console.log("Call clicked");

    const dialogRef = this.dialog.open(CallPopupComponent, {
      width: '25%',
      height: '65%',
      data : this.selecteduserId$
      
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });

  }

  UserSelected(user : any){
   
    this.selecteduserId$ = user.id;
    this.SelectedUsername$ = user.name;
    this.httpService.GetChatList(this.selecteduserId$).subscribe(s=>{});
    this.httpService.chatDatda$.subscribe(updatedData => {
      this.chatData$ = updatedData;
    });

    var chatRoomIdTemp : string = this.selecteduserId$+"_"+this.currentUserId$;
    this.tokenService.setChatRoomId(chatRoomIdTemp);
  }

  sendMessage() {

    let message : any = {
      message : this.userMessage,
      partnerId : this.selecteduserId$

    };
    this.messageService.SendMessage(message);
    this.userMessage = '';
  }

}
