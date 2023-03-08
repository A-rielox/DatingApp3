import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
   selector: 'app-root',
   templateUrl: './app.component.html',
   styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
   title = 'client';
   users: any;
   baseUrl = environment.apiUrl;

   constructor(private http: HttpClient) {}

   ngOnInit(): void {
      this.http.get(this.baseUrl + 'users').subscribe({
         next: (res) => (this.users = res),
         error: (err) => console.log(err),
      });
   }
}
