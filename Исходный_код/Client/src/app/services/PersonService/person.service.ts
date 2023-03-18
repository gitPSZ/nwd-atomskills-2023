import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
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
export class PersonService extends BaseApiService{

  constructor(private http: HttpClient, private errorService: ErrorService, private messageService : SimpleMessageService, private cookieService : CookieService) {
    super();
    
  }

  async getActualCurrentUser(): Promise<Person> {
    var observable = this.http.get<Person>(this.APIUrl + '/persons/actualCurrent')
    .pipe(catchError(this.errorService.errorHandlerSingleObject));

    var retValue = await lastValueFrom(observable);
    
    return retValue;
  }
  async getUsers(): Promise<Person[]> {
    var observable = this.http.get<Person[]>(this.APIUrl + '/admin/all')
    .pipe(catchError(this.errorService.errorHandlerList));

    var retValue = await lastValueFrom(observable);
    
    return retValue;
  }
  async getRoles(): Promise<Role[]> {
    var observable = this.http.get<Role[]>(this.APIUrl + '/admin/roles')
    .pipe(catchError(this.errorService.errorHandlerList));

    var retValue = await lastValueFrom(observable);
    
    return retValue;
  }
  async changeRole(personId: number , roleId : number): Promise<boolean> {
    var observable = this.http.post<boolean>(this.APIUrl + '/admin/changeRole/' + personId, roleId)
    .pipe(catchError(this.errorService.errorHandlerBoolean));

    var retValue = await lastValueFrom(observable);
    
    return retValue;
  }
  async changeEmail(email:string): Promise<boolean> {
    console.log("email", email)
    var observable = this.http.post<boolean>(this.APIUrl + '/persons/email', {email:email})
    .pipe(catchError(this.errorService.errorHandlerBoolean));

    var retValue = await lastValueFrom(observable);
    
    return retValue;
  }
}
