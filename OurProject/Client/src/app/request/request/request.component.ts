import { Component } from '@angular/core';
import { TypeRequest } from 'src/app/models/TypeRequest';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { catchError, lastValueFrom, map, Observable, of, pipe } from 'rxjs';
import { ErrorService } from '../../../app/services/ErrorService/error.service';
import { Router } from '@angular/router';
import { Input } from 'postcss';
import { StatusService } from 'src/app/services/StatusService/status.service';
import { timeout } from 'rxjs';
import { BaseComponent } from 'src/app/base-component/base.component';
import { SimpleMessageService } from 'src/app/services/SimpleMessageService/simple-message.service';
import { environment } from 'src/environments/environment.prod';
import { ConfigService } from 'src/app/services/ConfigService/config.service';
import { RequestModel } from 'src/app/newModels/RequestModel';
import { RequestService } from './request.service';
import { ToastService, ToastType } from 'src/app/services/ToastService/toast.service';
import { Priority } from 'src/app/models/Priority';



@Component({
	selector: 'app-request',
	templateUrl: './request.component.html',
	styleUrls: ['./request.component.css']
})
export class RequestComponent extends BaseComponent {
	isPutRequestLoading = false;
	priorities: Priority[] = [];
	typeList: TypeRequest[] = [];
	newRequest: RequestModel = {};
	jobPlace = "";
	inputText = "";
	dateEnd:any;
	selectedRequestList: TypeRequest = {};
	selectedPriority: Priority = {};
	constructor(private http: HttpClient, private errorService: ErrorService, private configService: ConfigService,
		private messageService: SimpleMessageService, private router: Router, private statusService: StatusService,
		private requestService : RequestService, private toastService : ToastService, private simplemesService:SimpleMessageService) {
		super();
	}

	async ngOnInit() {
		this.typeList = await this.requestService.getTypeRequests();
		this.priorities = await this.requestService.getPriorities();
	}
	async sendClick() {
		// this.newRequest.placeOfService = this.jobPlace;
		// this.newRequest.idPriority = this.selectedPriority.id;
		// this.newRequest.text = this.inputText;
		// this.newRequest.idType = this.selectedRequestList.id;
		// this.newRequest.IdAuthor =this.simplemesService.observableUser.value.id;

		// this.isPutRequestLoading = true;
	 	// this.requestService.putRequest(this.newRequest); 

	
		this.toastService.show("Заявка успешно создана", undefined, ToastType.success);
		this.isPutRequestLoading = false;
	}
}


