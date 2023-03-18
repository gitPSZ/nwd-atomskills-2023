import { Injectable } from '@angular/core';
import { stringify } from 'postcss';
import { MessageService } from 'primeng/api';
import { Toast } from 'primeng/toast';

@Injectable({
	providedIn: 'root'
})
export class ToastService {

	constructor(private messageService: MessageService) {
		
	}
	show(text:string, detail:string = "", type: ToastType = ToastType.info){
		this.messageService.add({ key: 'globalToast', severity: this.getSeverityString(type), summary: text, detail: detail });			
	}
	getSeverityString(type: ToastType): string{
		switch(type){
			case ToastType.success:
				return 'success'
			case ToastType.info:
				return 'info'
			case ToastType.warn:
				return 'warn'
			case ToastType.error:
				return 'error'
		}
	}
}
export enum ToastType{
	success, info, warn, error
}

