import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { catchError, lastValueFrom, of } from 'rxjs';
import { environment } from 'src/environments/environment.prod';
import { ServerStatus } from 'src/app/models/ServerStatus';
import { Person } from 'src/app/models/Person';
import { ErrorService } from '../ErrorService/error.service';

@Injectable({
	providedIn: 'root'
})
export class StatusService {

	person: Person;
	status: ServerStatus;
	APIUrl = environment.apiURL
	constructor(private http: HttpClient, private errorService: ErrorService) {
		this.status = {};
		this.person = {}
	}

	async getServerStatus(): Promise<ServerStatus> {
		var retValue = await lastValueFrom(this.http.post<ServerStatus>(this.APIUrl + '/status', {})
			.pipe(catchError(this.errorService.errorHandlerSingleObject)));

		return retValue;
	}
	getToken(): Promise<string> {
		var retValue = lastValueFrom(this.http.post<string>(this.APIUrl + '/persons/token', {})
			.pipe(catchError(this.errorService.errorHandlerString)));
		return retValue;
	}
	getAnauthorized(token: string): Promise<string> {
		let headers = new HttpHeaders();
		headers = headers.append("token", token);

		var retValue = lastValueFrom(this.http.get<string>(this.APIUrl + '/status/unauthorized', { headers })
			.pipe(catchError(this.errorService.errorHandlerString)));
		return retValue
	}

	sendToREST(): Promise<string> {
		let headers = new HttpHeaders();
		var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lX3VzZXIiOiJVc2VyMSIsIm5hbWVfdGVhbSI6IlRlYW0xIiwicGVybWlzc2lvbl9sZXZlbHMiOjEsImlhdCI6MTY3ODQyOTE2N30.L_2A4uz0IBOLfIatQSb3Qj6Ihnhv14bWHUAoRVa9DCU";
		headers = headers.append("Authorization", `Bearer ${token}`);

		var retValue = lastValueFrom(this.http.get<string>('http://172.23.48.61:3000/refs/time_sla', { headers })
			.pipe(catchError(this.errorService.errorHandlerString)));
		return retValue
	}

}
