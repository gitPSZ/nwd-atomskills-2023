import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Table } from 'primeng/table';
import { PrimeNGConfig } from 'primeng/api';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';
import { ClaimModel } from '../models/ClaimModel';
import { Person } from '../models/Person';
import { SimpleMessageService } from '../services/SimpleMessageService/simple-message.service';
import { RequestService } from '../request/request/request.service';
import { ToastService } from '../services/ToastService/toast.service';
import { BaseComponent } from '../base-component/base.component';
import * as FileSaver from 'file-saver';
import { timer } from 'rxjs';
import { RequestModel } from '../newModels/RequestModel';



@Component({
  selector: 'app-init-check-table',
  templateUrl: './init-check-table.component.html',
  styleUrls: ['./init-check-table.component.css'],
})

export class InitCheckTableComponent  extends BaseComponent implements OnInit {
  executorsList: Person[] = [];
  selectedProduct1 : ClaimModel = {};
  selectedExecutor: Person = {};
  customers: RequestModel[] = [];
  executorSaveModel: ClaimModel | undefined;
  roleUser: number | undefined
  claim: any = {};
  statuses: any[] = [];
  visible = false;
  visibleSelectExecutor = false;
  visibleCancelClaim = false;
  loading: boolean = false;
  APIUrl = environment.apiURL;
  globalText = '';
  roleid: number | undefined;
  @ViewChild("dt") dataGrid : Table | undefined;


  constructor(private primengConfig: PrimeNGConfig, private http: HttpClient, private simplemesService: SimpleMessageService, private requestService : RequestService, private toastService: ToastService,) {
    super();
  }

  ngOnInit() {
    //this.roleid = this.simplemesService.observableUser.value.roleId;
    //let person = this.simplemesService.observableUser.value;
   
 
    var userSubscription = this.simplemesService.observableUser.subscribe(async (user) => {
      if(user.id == null){
        return;
      }
    this.customers = await this.requestService.getRequests();
    })
    this.subscriptions.push(userSubscription);
    this.primengConfig.ripple = true;

  }
onSelected(){
  console.debug("selected")
}
  openDialogView(customer: ClaimModel) {
if (customer.id!=null || customer.id!=undefined)
{
    this.claim = customer;
    console.debug("Открытие", customer);
    this.visible = true;
}
else
{
  this.toastService.show("Не выбрана заявка", "", ToastType.success);
}
  }

  async openDialogSelectExecutor(customer: any) {
    this.executorsList = await this.requestService.getExecutors();
    this.claim = customer;
    this.selectedExecutor = this.executorsList.filter(x => x.id == this.claim.idExecutor)[0];
    this.visibleSelectExecutor = true;

  }

   inWork(customer: any) {
    this.requestService.toWork(customer).finally(()=>{
      this.refeshBtn();
      });
    this.toastService.show("Заявка успешно принята в работу", "", ToastType.success);
  }
   acceptClaim(customer: any) {
    this.requestService.acceptClaim(customer).finally(()=>{
    this.refeshBtn();
    });
    this.toastService.show("Заявка успешно выполнена", "", ToastType.success);
  }

  showDialogCancelClaim(customer: any) {
   
    this.claim = customer;
    console.log(this.claim);
    this.visibleCancelClaim = true;
  }
  commandCancelClaim(){
    console.debug(this.claim.id);

    this.requestService.cancelClaim(this.claim).finally(()=>{
      this.refeshBtn();
      });
  }

  async refeshBtn(){
    this.customers = await this.requestService.getRequests();
    console.log(this.customers);
    
  }
  async commandExecutorSave() {

    this.executorSaveModel = { id: this.claim.id, idExecutor: this.selectedExecutor.id };
    this.requestService.saveExecutor(this.executorSaveModel);
    this.claim.executor = this.selectedExecutor.nameClient;
    console.debug(this.claim);
    this.visibleSelectExecutor = false;
    let person = this.simplemesService.observableUser.value;
    console.debug("role", this.roleUser);
  //  this.customers = await this.requestService.getClaimsWithAllAttributes();


  }
  // onDateSelect(value: any) {
  //     this.table.filter(this.formatDate(value), 'date', 'equals')
  // }

  filterGlobal() {
    this.dataGrid?.filterGlobal(this.globalText, 'contains');
  }

  formatDate(date: { getMonth: () => number; getDate: () => any; getFullYear: () => string; }) {
    let month = date.getMonth() + 1;
    let day = date.getDate();

    if (month < 10) {
      //  month = '0' + month;
    }

    if (day < 10) {
      day = '0' + day;
    }

    return date.getFullYear() + '-' + month + '-' + day;
  }



}

export enum ToastType {
  success, info, warn, error
}

