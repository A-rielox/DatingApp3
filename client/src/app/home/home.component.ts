import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
   selector: 'app-home',
   templateUrl: './home.component.html',
   styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
   registerMode: boolean = false;
   users: any;
   baseUrl = environment.apiUrl;

   constructor(private http: HttpClient) {}

   ngOnInit(): void {
      this.getUsers();
   }

   registerToggle() {
      this.registerMode = !this.registerMode;
   }

   getUsers() {
      this.http.get(this.baseUrl + 'users').subscribe({
         next: (res) => (this.users = res),
         error: (err) => console.log(err),
      });
   }

   cancelRegisterMode(event: boolean) {
      this.registerMode = event;
   }
}
