import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, lastValueFrom } from 'rxjs';
import { ClaimModel } from 'src/app/models/ClaimModel';
import { Person } from 'src/app/models/Person';
import { Priority } from 'src/app/models/Priority';
import { RequestModel } from 'src/app/models/RequestModel';
import { State } from 'src/app/models/State';
import { TypeRequest } from 'src/app/models/TypeRequest';
import { BaseApiService } from 'src/app/services/BaseAPIService/base-api.service';
import { ErrorService } from 'src/app/services/ErrorService/error.service';
import { SimpleMessageService } from 'src/app/services/SimpleMessageService/simple-message.service';
import { MachineModel } from '../newModels/MachineModel';

@Injectable({
  providedIn: 'root'
})
export class dictionaryService extends BaseApiService {

  constructor(private errorService: ErrorService, private http: HttpClient, private messageService : SimpleMessageService) {
    super();
  }


  getDictionary(): Promise<MachineModel[]> {

    var retValue = lastValueFrom(this.http.get<MachineModel[]>(this.APIUrl + '/machine/all')
      .pipe(catchError(this.errorService.errorHandlerList)));
    return retValue;
  }


 
}
