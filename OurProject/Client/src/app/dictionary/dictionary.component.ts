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
import { dictionaryService } from './dictionary.service';

@Component({
  selector: 'app-dictionary',
  templateUrl: './dictionary.component.html',
  styleUrls: ['./dictionary.component.css']
})
export class DictionaryComponent  extends BaseComponent implements OnInit{
  loading: boolean = false;
  selectedProduct1 : MachineModel = {};
  globalText = '';
  customers: MachineModel[] = [];
  @ViewChild("dt") dataGrid : Table | undefined;
  constructor(private primengConfig: PrimeNGConfig,private dictionaryService : dictionaryService,  private http: HttpClient, private simplemesService: SimpleMessageService, private requestService : RequestService, private toastService: ToastService,) {
    super();
  }
  async ngOnInit() {
    this.customers = await this.dictionaryService.getDictionary();
    console.log(this.customers);
  }

  filterGlobal() {
    this.dataGrid?.filterGlobal(this.globalText, 'contains');
  }

  async refeshBtn(){
    this.customers = await this.dictionaryService.getDictionary();
    console.log(this.customers);
    
  }
}