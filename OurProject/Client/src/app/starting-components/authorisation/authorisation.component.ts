import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, interval, lastValueFrom, Observable, of, pipe } from 'rxjs';
import { timeout } from 'rxjs';
import { BaseComponent } from 'src/app/base-component/base.component';
import { SimpleMessageService } from 'src/app/services/SimpleMessageService/simple-message.service';
import { Person } from '../../models/Person';
import { ErrorService } from '../../../app/services/ErrorService/error.service';
import { Router } from '@angular/router';
import { Input } from 'postcss';
import { StatusService } from 'src/app/services/StatusService/status.service';
import { environment } from 'src/environments/environment.prod';
import { ServerStatus } from 'src/app/models/ServerStatus';
import { ToastService, ToastType } from 'src/app/services/ToastService/toast.service';
import { Toast } from 'primeng/toast';
import { AuthenticationService } from 'src/app/services/AuthenticationService/authentication.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';


@Component({
	selector: 'app-authorisation',
	templateUrl: './authorisation.component.html',
	styleUrls: ['./authorisation.component.css']
})
export class AuthorisationComponent extends BaseComponent {
	person: Person;

	visible = false;
	APIUrl = environment.apiURL
	isLoading = false; newPerson: Person; isLoadingRegistration = false;

	formGroup: FormGroup;

	constructor(private http: HttpClient,
		private errorService: ErrorService,
		private messageService: SimpleMessageService,
		private router: Router,
		private statusService: StatusService,
		private toastService: ToastService,
		private authenticationService: AuthenticationService) {
		super();
		this.person = {}; this.newPerson = {};
		messageService.changeMainDesignVisibility(false);

		this.formGroup = new FormGroup({
			nameClient: new FormControl("", Validators.required),
			password: new FormControl("", Validators.required),
			login: new FormControl("", Validators.required),
		})
	}

	shouldShow(id: string) {
		return this.formGroup.get(id)?.dirty && this.formGroup.get(id)?.invalid;
	}

	ngOnInit(): void {


	}



	override ngOnDestroy() {
		super.ngOnDestroy();
		this.messageService.changeMainDesignVisibility(true);
	}
	onKeyDown(event:KeyboardEvent){
		if(event.key == "Enter"){
			this.authoriseClick();
		}
	}
	async authoriseClick() {
		this.isLoading = true;

		let token = await this.authenticationService.getToken(this.person);

		if (token != null && token != "") {
			this.router.navigateByUrl("navigationCards");
		}
		else {
			this.toastService.show("Неверный логин или пароль", undefined, ToastType.warn)

		}


		this.isLoading = false;
	}
	showRegisterWindow() {
		this.visible = true;
	}
	async registrationClick() {
		this.isLoadingRegistration = true;

		var checkLogin = await lastValueFrom(this.http.post<boolean>(this.APIUrl + '/persons/checkReply', this.formGroup.value)
			.pipe(catchError(this.errorService.errorHandlerString)));
		console.debug("result", checkLogin);
		if (checkLogin == false) {
			this.isLoadingRegistration = false;
			this.toastService.show("Пользователь с данным логином уже зарегистрирован", "", ToastType.success)
			return;
		}

		var retValue = await lastValueFrom(this.http.put<number>(this.APIUrl + '/persons/registration', this.formGroup.value)
			.pipe(catchError(this.errorService.errorHandlerString)));
		console.debug("result", retValue);

		this.isLoadingRegistration = false;

		if (retValue != null) {
			this.toastService.show("Удачная регистрация", "", ToastType.success)
		}
		else {
			this.toastService.show("Неудачная регистрация", "", ToastType.warn)
		}
		this.newPerson = {};
		this.visible = false;

		//todo поменять на всплывающее
	}

}
