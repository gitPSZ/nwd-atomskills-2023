import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { AuthenticationService } from '../AuthenticationService/authentication.service';
import { SimpleMessageService } from '../SimpleMessageService/simple-message.service';

@Injectable({
  providedIn: 'root'
})
export class TokenInterceptorService implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService, private simpleMessageService : SimpleMessageService) {}
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {


    const token = this.simpleMessageService.observableToken.value;
    if (token != null) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
          token: token
        },
      });
    }
    return next.handle(request);
  }
}