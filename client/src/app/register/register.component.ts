import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import {
   AbstractControl,
   FormBuilder,
   FormControl,
   FormGroup,
   ValidatorFn,
   Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
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
   registerForm: FormGroup = new FormGroup({});

   // maxDate: Date = new Date(); // inicializa con fecha y hora actual
   // validationErrors: string[] | undefined;

   constructor(
      private accountService: AccountService,
      private toastr: ToastrService,
      private fb: FormBuilder,
      private router: Router
   ) {}

   ngOnInit(): void {
      this.initializeForm();
   }

   initializeForm() {
      this.registerForm = this.fb.group({
         gender: ['male'],
         username: ['', Validators.required],
         knownAs: ['', Validators.required],
         // dateOfBirth: ['', Validators.required],
         city: ['', Validators.required],
         country: ['', Validators.required],
         password: [
            '',
            [
               Validators.required,
               Validators.minLength(4),
               Validators.maxLength(8),
            ],
         ],
         confirmPassword: [
            '',
            [Validators.required, this.matchValues('password')],
         ],
      });

      // por si cambia el password despues de poner el confirmPassword y pasar la validacion
      this.registerForm.controls['password'].valueChanges.subscribe(() => {
         this.registerForm.controls['confirmPassword'].updateValueAndValidity();
      });
   }

   matchValues(matchTo: string): ValidatorFn {
      return (control: AbstractControl) => {
         return control.value === control.parent?.get(matchTo)?.value
            ? null
            : { notMatching: true };
      };
   }

   register() {
      console.log(this.registerForm?.value);

      // this.accountService.register(this.model).subscribe({
      //    next: (res) => {
      //       this.cancel(); // cierro el register form
      //    },
      //    error: (err) => {
      //       console.log(err);
      //       this.toastr.error(err.error + '  💩');
      //    },
      // });
   }

   cancel() {
      this.cancelRegister.emit(false);
   }

   private getDateOnly(dob: string | undefined) {
      if (!dob) return;

      let theDob = new Date(dob);

      return new Date(
         theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset())
      )
         .toISOString()
         .slice(0, 10);
   }
}
