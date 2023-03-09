import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
   selector: 'app-server-error',
   templateUrl: './server-error.component.html',
   styleUrls: ['./server-error.component.css'],
})
export class ServerErrorComponent implements OnInit {
   error: any;

   // SOLO SE PUEDE ACCEDER A LOS RUTER-STATES DENTRO DEL CONSTRUCTOR

   /* 
   const navigationExtras: NavigationExtras = {
         state: { error: error.error },
      };

      this.router.navigateByUrl(
         '/server-error',
         navigationExtras
      );
   */
   constructor(private router: Router) {
      const navigation = this.router.getCurrentNavigation();
      this.error = navigation?.extras?.state?.['error'];
   }

   ngOnInit(): void {}
}
