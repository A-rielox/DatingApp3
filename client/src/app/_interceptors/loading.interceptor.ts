import { Injectable } from '@angular/core';
import {
   HttpRequest,
   HttpHandler,
   HttpEvent,
   HttpInterceptor,
} from '@angular/common/http';
import { delay, finalize, identity, Observable } from 'rxjs';
import { BusyService } from '../_services/busy.service';
import { environment } from 'src/environments/environment';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
   constructor(private busyService: BusyService) {}

   intercept(
      request: HttpRequest<unknown>,
      next: HttpHandler
   ): Observable<HttpEvent<unknown>> {
      this.busyService.busy();

      return next.handle(request).pipe(
         // delay(700),
         environment.production ? identity : delay(700),
         finalize(() => this.busyService.idle())
      );
   }
}
