import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { interval, Observable, timer } from 'rxjs';
import { BaseComponent } from 'src/app/base-component/base.component';
import { PersonService } from 'src/app/services/PersonService/person.service';
import { SimpleMessageService } from 'src/app/services/SimpleMessageService/simple-message.service';
import { ToastService } from 'src/app/services/ToastService/toast.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent extends BaseComponent {
    firstEmail = '';
    formGroup : FormGroup
    constructor(public messageService : SimpleMessageService, private personService : PersonService, private toastService : ToastService){
        super();
        this.formGroup = new FormGroup({
            email: new FormControl("123", Validators.required),
        })
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
}
