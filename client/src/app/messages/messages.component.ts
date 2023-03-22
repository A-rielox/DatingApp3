import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MessageService } from '../_services/message.service';

@Component({
   selector: 'app-messages',
   templateUrl: './messages.component.html',
   styleUrls: ['./messages.component.css'],
})
export class MessagesComponent implements OnInit {
   messages?: Message[];
   pagination?: Pagination;
   container = 'Unread';
   pageNumber = 1;
   pageSize = 5;

   loading = false;

   constructor(private messageService: MessageService) {}

   ngOnInit(): void {
      this.loadMessages();
   }

   loadMessages() {
      this.loading = true;

      this.messageService
         .getMessages(this.pageNumber, this.pageSize, this.container)
         .subscribe({
            next: (res) => {
               this.messages = res.result;
               this.pagination = res.pagination;

               this.loading = false;
            },
         });
   }

   deleteMessage(id: number) {
      this.messageService.deleteMessage(id).subscribe({
         next: () =>
            this.messages?.splice(
               this.messages.findIndex((m) => m.id === id),
               1
            ),
      });
   }

   pageChanged(e: any) {
      if (this.pageNumber !== e.page) {
         this.pageNumber = e.page;

         this.loadMessages();
      }
   }
}
