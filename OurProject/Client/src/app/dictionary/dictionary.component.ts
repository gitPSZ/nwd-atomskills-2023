import { Component, OnInit, ViewChild } from '@angular/core';
import { Table } from 'primeng/table';
import { BaseComponent } from '../base-component/base.component';
import { MachineModel } from '../newModels/MachineModel';
import { PrimeNGConfig } from 'primeng/api';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';
import { ClaimModel } from '../models/ClaimModel';
import { Person } from '../models/Person';
import { SimpleMessageService } from '../services/SimpleMessageService/simple-message.service';
import { RequestService } from '../request/request/request.service';
import { ToastService } from '../services/ToastService/toast.service';
import * as FileSaver from 'file-saver';
import { timer } from 'rxjs';
import { RequestModel } from '../models/RequestModel';
import { DictionaryService } from './dictionary.service';

@Component({
  selector: 'app-dictionary',
  templateUrl: './dictionary.component.html',
  styleUrls: ['./dictionary.component.css']
})
export class DictionaryComponent  extends BaseComponent implements OnInit{
  interval: any;
  loading: boolean = false;
  selectedProduct1 : MachineModel = {};
  visible:boolean = false;
  globalText = '';
  dialogInfo:MachineModel = {};
  customers: MachineModel[] = [];
  @ViewChild("dt") dataGrid : Table | undefined;
  constructor(private primengConfig: PrimeNGConfig,private dictionaryService : DictionaryService,  private http: HttpClient, private simplemesService: SimpleMessageService, private requestService : RequestService, private toastService: ToastService,) {
    super();
  }
  async ngOnInit() {
    this.customers = await this.dictionaryService.getDictionary();
    console.log(this.customers);
    this.startTimer();
  }

  filterGlobal() {
    this.dataGrid?.filterGlobal(this.globalText, 'contains');
  }
 async repairBtnDialog(info:MachineModel){
    this.dialogInfo = {};
    this.dialogInfo = info;

this.visible = true;
}
async startTimer() {
  this.interval = setInterval(async () => {
      await this.refeshBtn();

  },7000)
  }


async repairBtnClick(){
  let repair = await this.dictionaryService.setRepair(this.dialogInfo).finally(()=>{
  });
  this.customers = await this.dictionaryService.getDictionary();
  this.visible = false;
  this.refeshBtn();
}
  async refeshBtn(){
    this.customers = await this.dictionaryService.getDictionary();
    
    console.log(this.customers);
    
  }
}
