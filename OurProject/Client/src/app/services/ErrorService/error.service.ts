import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Toast } from 'primeng/toast';
import { BehaviorSubject, of } from 'rxjs';
import { ToastService, ToastType } from '../ToastService/toast.service';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  constructor(private toastService : ToastService, private router : Router) { }
  observableException:BehaviorSubject<HttpErrorResponse|undefined> = new BehaviorSubject<HttpErrorResponse|undefined>(undefined);
  error:HttpErrorResponse|undefined;
  
  errorHandlerList = (error: HttpErrorResponse)=> {
    this.handleError(error);

    return of([]);
  }
  errorHandlerSingleObject = (error: HttpErrorResponse)=> {
    this.handleError(error);

    return of({});
  } 
  errorHandlerString = (error: HttpErrorResponse)=> {
    this.handleError(error);

    return of("");
  }
  errorHandlerNumber = (error: HttpErrorResponse)=> {
    this.handleError(error);

    return of(-1);
  }
  errorHandlerBoolean = (error: HttpErrorResponse)=> {
    this.handleError(error);
    return of(false);
  }
  handleError(error:HttpErrorResponse){
    if(error.status == 401){
      /* this.router.navigateByUrl("auth"); */
      /* this.toastService.show("Недостаточно прав для выполнения операции",'',ToastType.error); */
      return;
  }
    console.log(error);
    this.toastService.show("Ошибка выполнения запроса",error.message,ToastType.error);
    this.observableException.next(error);
  }
}
