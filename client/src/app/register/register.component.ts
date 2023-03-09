import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
   selector: 'app-register',
   templateUrl: './register.component.html',
   styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
   @Output() cancelRegister = new EventEmitter();
   model: any = {};

   constructor(
      private accountService: AccountService,
      private toastr: ToastrService
   ) {}

   ngOnInit(): void {}

   register() {
      this.accountService.register(this.model).subscribe({
         next: (res) => {
            // console.log(res); 🌟
            this.cancel(); // cierro el register form
         },
         error: (err) => {
            console.log(err);

            this.toastr.error(err.error + '  💩');
         },
      });
   }

   cancel() {
      this.cancelRegister.emit(false);
   }
}

// para ver el user ( la res ) necesito return en el map del accountService.register
//
// map((user) => {
//    if (user) {
//       localStorage.setItem('user', JSON.stringify(user));
//       this.currentUserSource.next(user);
//    }
//
//    return user;   🌟
// })
