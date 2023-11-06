import { Component } from '@angular/core';
import { MessageService } from './Services/message.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Tetherfi-Web';

  constructor(private messageService: MessageService){
    this.messageService.init();
  }
}
