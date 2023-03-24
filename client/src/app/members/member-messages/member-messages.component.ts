import {
   ChangeDetectionStrategy,
   Component,
   Input,
   OnInit,
   ViewChild,
} from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
   changeDetection: ChangeDetectionStrategy.OnPush,
   selector: 'app-member-messages',
   templateUrl: './member-messages.component.html',
   styleUrls: ['./member-messages.component.css'],
})
export class MemberMessagesComponent implements OnInit {
   @ViewChild('messageForm') messageForm?: NgForm;
   @Input() username?: string;
   @Input() messages: Message[] = [];

   messageContent = '';

   constructor(private messageService: MessageService) {}

   ngOnInit(): void {
      // this.loadMessages();
   }

   // MOVIDO A MEMBER-DETAIL.COMPONENT.TS
   // loadMessages() {
   //    if (this.username) {
   //       this.messageService.getMessageThread(this.username).subscribe({
   //          next: (messages) => (this.messages = messages),
   //       });
   //    }
   // }

   sendMessage() {
      if (!this.username) return;

      this.messageService
         .sendMessage(this.username, this.messageContent)
         .subscribe({
            next: (msg) => {
               this.messages.push(msg);
               this.messageForm?.reset();
            },
         });
   }
}
