import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { catchError, interval, lastValueFrom, Observable, timer } from 'rxjs';
import { BaseComponent } from 'src/app/base-component/base.component';
import { Person } from 'src/app/models/Person';
import { ErrorService } from 'src/app/services/ErrorService/error.service';
import { PersonService } from 'src/app/services/PersonService/person.service';
import { SimpleMessageService } from 'src/app/services/SimpleMessageService/simple-message.service';
import { ToastService, ToastType } from 'src/app/services/ToastService/toast.service';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent extends BaseComponent {
    firstEmail = '';
    newPerson: Person;
    APIUrl = environment.apiURL
    formGroup : FormGroup; 	visible = false;
    formGroupRegister: FormGroup;
    isLoadingRegistration = false;
    constructor(public messageService : SimpleMessageService, private personService : PersonService, private errorService: ErrorService, private http: HttpClient, private toastService : ToastService){
        super();
        this.newPerson = {};
        this.formGroupRegister = new FormGroup({
			nameClient: new FormControl("", Validators.required),
			password: new FormControl("", Validators.required),
			login: new FormControl("", Validators.required),
		});
        this.formGroup = new FormGroup({
            email: new FormControl("123", Validators.required),
        });
        this.messageService.observableUser.subscribe(async user => {
            console.log(user)
            if(user == null){
                return;
            }
            let email = (await this.personService.getActualCurrentUser()).email;
            if(email == null){
                return;
            }
            this.formGroup = new FormGroup({
                email: new FormControl(email, Validators.pattern("^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")),
                
            })
            this.firstEmail = email;
        })
    }
    async updateEmail(){
        if(await this.personService.changeEmail(this.formGroup.value.email)){
            this.firstEmail = this.formGroup.value.email;
            console.log(this.formGroup.value.email)
            this.toastService.show("Email успешно обновлён");
        }
    }
    showRegisterWindow() {
        this.formGroupRegister = new FormGroup({
			nameClient: new FormControl("", Validators.required),
			password: new FormControl("", Validators.required),
			login: new FormControl("", Validators.required),
		});
		this.visible = true;
	}
    shouldShow(id: string) {
		return this.formGroupRegister.get(id)?.dirty && this.formGroupRegister.get(id)?.invalid;
	}

    async registrationClick() {
		this.isLoadingRegistration = true;

		var checkLogin = await lastValueFrom(this.http.post<boolean>(this.APIUrl + '/persons/checkReply', this.formGroupRegister.value)
			.pipe(catchError(this.errorService.errorHandlerString)));
		console.debug("result", checkLogin);
		if (checkLogin == false) {
			this.isLoadingRegistration = false;
			this.toastService.show("Пользователь с данным логином уже зарегистрирован", "", ToastType.success)
			return;
		}

		var retValue = await lastValueFrom(this.http.put<number>(this.APIUrl + '/persons/registration', this.formGroupRegister.value)
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

	}
}
