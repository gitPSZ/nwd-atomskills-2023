import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, lastValueFrom } from 'rxjs';
import { NavigationButton } from 'src/app/models/NavigationButton';
import { BaseApiService } from '../BaseAPIService/base-api.service';
import { ErrorService } from '../ErrorService/error.service';

@Injectable({
  providedIn: 'root'
})
export class AgileInterfaceService extends BaseApiService{

  constructor(private errorService: ErrorService, private http: HttpClient) {
    super();
  }
  getNavigationButtons() : Promise<NavigationButton[]>{
    var result = lastValueFrom(this.http.get<NavigationButton[]>(this.APIUrl + '/agileInterface/navigationButtons')
    .pipe(catchError(this.errorService.errorHandlerList)));

    return result;
  }
}
