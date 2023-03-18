import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, lastValueFrom, Observable, of } from 'rxjs';
import { Person } from 'src/app/models/Person';
import { Role } from 'src/app/models/Role';
import { environment } from 'src/environments/environment';
import { BaseApiService } from '../BaseAPIService/base-api.service';
import { CookieService } from '../CookieService/cookie.service';
import { ErrorService } from '../ErrorService/error.service';
import { SimpleMessageService } from '../SimpleMessageService/simple-message.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService extends BaseApiService {

  constructor(private http: HttpClient, private errorService: ErrorService, private messageService: SimpleMessageService, private cookieService: CookieService,
    private router : Router) {
    super();

  }

  async getToken(person: Person): Promise<string> {
    var observable = this.http.post<string>(this.APIUrl + '/persons/token', person)
      .pipe(catchError(this.errorService.errorHandlerString));

    //caching token
    observable.subscribe((value) => {
      console.debug("tokenSet");
      this.messageService.observableToken.next(value)
      this.cookieService.setCookie(environment.tokenKey, value, 1, "")
    });

    var retValue = lastValueFrom(observable);

    return retValue;
  }
  logOut() {
    
    this.cookieService.deleteCookie(environment.tokenKey)
    this.router.navigateByUrl('auth');
  }
  async getCurrentUser(): Promise<Person> {
    var observable = this.http.get<Person>(this.APIUrl + '/persons/current')
      .pipe(catchError(this.errorService.errorHandlerSingleObject));

    var retValue = await lastValueFrom(observable);

    return retValue;
  }
}
