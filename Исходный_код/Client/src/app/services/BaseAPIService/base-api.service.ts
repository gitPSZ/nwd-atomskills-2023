import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ErrorService } from '../ErrorService/error.service';

@Injectable({
  providedIn: 'root'
})
export class BaseApiService {

  
	APIUrl = environment.apiURL
	
}
