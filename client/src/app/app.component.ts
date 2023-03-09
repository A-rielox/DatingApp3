import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
   selector: 'app-root',
   templateUrl: './app.component.html',
   styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
   title = 'client';

   constructor(private accountService: AccountService) {}

   ngOnInit(): void {
      this.setCurrentUser();
   }

   setCurrentUser() {
      const userStr = localStorage.getItem('user');

      if (!userStr) return;

      const user: User = JSON.parse(userStr);
      this.accountService.setCurrentUser(user);
   }
}
