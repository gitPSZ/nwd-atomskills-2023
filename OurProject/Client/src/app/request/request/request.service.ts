import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, lastValueFrom } from 'rxjs';
import { ClaimModel } from 'src/app/models/ClaimModel';
import { Person } from 'src/app/models/Person';
import { Priority } from 'src/app/models/Priority';
import { RequestModel } from 'src/app/newModels/RequestModel';
import { State } from 'src/app/models/State';
import { TypeRequest } from 'src/app/models/TypeRequest';
import { BaseApiService } from 'src/app/services/BaseAPIService/base-api.service';
import { ErrorService } from 'src/app/services/ErrorService/error.service';
import { SimpleMessageService } from 'src/app/services/SimpleMessageService/simple-message.service';
import { ProductsForPosition } from 'src/app/newModels/ProductsForPosition';
import { MachineRequestModel } from 'src/app/newModels/MachineRequestModel';

@Injectable({
  providedIn: 'root'
})
export class RequestService extends BaseApiService {

  constructor(private errorService: ErrorService, private http: HttpClient, private messageService : SimpleMessageService) {
    super();
  }

  getTypeRequests(): Promise<TypeRequest[]> {

    var retValue = lastValueFrom(this.http.get<TypeRequest[]>(this.APIUrl + '/typerequest')
      .pipe(catchError(this.errorService.errorHandlerList)));
    return retValue;
  }
  getCountRequest(): Promise<number> {

    var retValue = lastValueFrom(this.http.get<number>(this.APIUrl + '/request/countRequest')
      .pipe(catchError(this.errorService.errorHandlerNumber)));
    return retValue;
  }
 

  getRequests(): Promise<RequestModel[]> {

    var retValue = lastValueFrom(this.http.get<RequestModel[]>(this.APIUrl + '/request')
      .pipe(catchError(this.errorService.errorHandlerList)));
    return retValue;
  }

  getProductsForPosition(request: RequestModel | null | undefined): Promise<ProductsForPosition[]> {

    var retValue = lastValueFrom(this.http.post<ProductsForPosition[]>(this.APIUrl + '/request/productsForPosition', request)
      .pipe(catchError(this.errorService.errorHandlerList)));
    return retValue;
  }
  
  SaveMachineRequest(idMachine?:string, idRequest?: number): Promise<MachineRequestModel[]> {
   let b = { IdMachine: idMachine, IdRequest: idRequest }
    var retValue = lastValueFrom(this.http.post<MachineRequestModel[]>(this.APIUrl + '/request/saveMachineRequest',b)
      .pipe(catchError(this.errorService.errorHandlerList)));
    return retValue;
  }

  getLastRequests(count:number): Promise<RequestModel[]> {

    let countDto = {Count:count};
    var retValue = lastValueFrom(this.http.post<RequestModel[]>(this.APIUrl + '/request/last',countDto)
      .pipe(catchError(this.errorService.errorHandlerList)));
    return retValue;
  }





  getExecutors(): Promise<Person[]> {

    var retValue = lastValueFrom(this.http.get<Person[]>(this.APIUrl + '/persons/getexecutors')
      .pipe(catchError(this.errorService.errorHandlerList)));
    console.debug("person", retValue);
    return retValue;
  }
  //принять в работу
  toWork(request: RequestModel) {

    var retValue = lastValueFrom(this.http.post<RequestModel>(this.APIUrl + '/request/toWork', request)
      .pipe(catchError(this.errorService.errorHandlerList)));
    return retValue;
  }

  //выполнено
  acceptClaim(request: RequestModel) {

    var retValue = lastValueFrom(this.http.post<RequestModel>(this.APIUrl + '/request/acceptClaim', request)
      .pipe(catchError(this.errorService.errorHandlerList)));
    return retValue;
  }

    //отказать
    cancelClaim(request: any) {

      var retValue = lastValueFrom(this.http.post<any>(this.APIUrl + '/request/cancelClaim', request)
        .pipe(catchError(this.errorService.errorHandlerList)));
      return retValue;
    }

  getPriorities(): Promise<Priority[]> {

    var retValue = lastValueFrom(this.http.get<Priority[]>(this.APIUrl + '/request/priorities')
      .pipe(catchError(this.errorService.errorHandlerList)));
    return retValue;
  }
  getStates(): Promise<State[]> {

    var retValue = lastValueFrom(this.http.get<State[]>(this.APIUrl + '/request/states')
      .pipe(catchError(this.errorService.errorHandlerList)));
    return retValue;
  }
  getClaimsWithAllAttributes(): Promise<ClaimModel[]> {

    var retValue = lastValueFrom(this.http.post<ClaimModel[]>(this.APIUrl + '/request/allClaims', this.messageService.observableUser.value)
      .pipe(catchError(this.errorService.errorHandlerList)));
    return retValue;
  }



  putRequest(newRequest: RequestModel) {
    var retValue = lastValueFrom(this.http.put(this.APIUrl + '/request/save', newRequest)
      .pipe(catchError(this.errorService.errorHandlerList)));

    return retValue;
  }

  saveExecutor(request: ClaimModel) {
    var retValue = lastValueFrom(this.http.post(this.APIUrl + '/request/saveExecutor', request)
      .pipe(catchError(this.errorService.errorHandlerList)));

    return retValue;
  }
}
