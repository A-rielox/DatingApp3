import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map, Observable, take } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable({
   providedIn: 'root',
})
export class AuthGuard implements CanActivate {
   constructor(
      private accountService: AccountService,
      private toastr: ToastrService
   ) {}

   canActivate(): Observable<boolean> {
      return this.accountService.currentUser$.pipe(
         take(1),
         map((user) => {
            if (user) return true;
            else {
               this.toastr.error('🧙‍♂️ You shall not pass!!! 💥⚡⚡💥');

               return false;
            }
         })
      );
   }
}
