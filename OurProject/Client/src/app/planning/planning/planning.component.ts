import { Component, ViewChild, ViewEncapsulation } from '@angular/core';
import { BaseComponent } from 'src/app/base-component/base.component';
import { MachineModel } from 'src/app/newModels/MachineModel';
import { ProductsForPosition } from 'src/app/newModels/ProductsForPosition';
import { RequestSharedModel } from 'src/app/newModels/RequestSharedModel';
import { RequestModel } from 'src/app/newModels/RequestModel';
import { MachineRequestModel } from 'src/app/newModels/MachineRequestModel';
import { RequestService } from 'src/app/request/request/request.service';
import { dictionaryService } from '../../dictionary/dictionary.service';
import { Listbox } from 'primeng/listbox';
import { ToastService, ToastType } from 'src/app/services/ToastService/toast.service';

@Component({
  selector: 'app-planning',
  templateUrl: './planning.component.html',
  styleUrls: ['./planning.component.css'],
})
export class PlanningComponent extends BaseComponent{

    @ViewChild("requestListBox") requestListBox : Listbox | undefined;
    requests : RequestModel[] = [];
    requestsShared: RequestSharedModel[] = [];
    products: ProductsForPosition[]= [];
    machine: MachineModel[] = [];
    // selectedRequest: RequestSharedModel = {
    //     machine: [],
    //     selectedMachine: []
    // }
    selectedRequest: RequestSharedModel = {
        machines: [],
        selectedMachines: []
    };
    selectedProduct: ProductsForPosition = {};
    
    filterValue : string = '';
    constructor(private requestService : RequestService, private dictionaryService : dictionaryService, private toastService : ToastService){
        super();
    }
    async ngOnInit(){
        this.requests = await this.requestService.getRequests();
  
      this.machine = await this.dictionaryService.getDictionary().finally(()=>{
        
      });
      
      let requestsSharedLocal : RequestSharedModel[] = []
      this.requests.forEach(xx=>{ 
        if (xx.stateCode=="DRAFT")
        {
        requestsSharedLocal.push({
                 request: xx,
                 machines: this.machine,
                 selectedMachines: []
             })
          }
      })
      this.requestsShared = requestsSharedLocal
          this.selectedRequest = this.requestsShared[0];
          
      //  this.products = await this.requestService.getProductsForPosition(this.selectedRequest.request);
       
     
    }
    myResetFunction(options:any){

    }
    async onChangeRequest(event: any) {
        this.products = await this.requestService.getProductsForPosition(this.selectedRequest.request);

    }
    async acceptClick(){


        let requestLocal = this.selectedRequest;
        let frezCount = 0;
        let tokarnCount = 0;
        this.selectedRequest.selectedMachines.forEach(x=>{
            if (x.machineTypeCaption == "токарный станок")
            {
                tokarnCount++;
            }
            if (x.machineTypeCaption == "фрезеровальный станок")
            {
                frezCount++;
            }
        })
        console.log(frezCount, tokarnCount);
        if (frezCount<1||tokarnCount<1)
        {
            this.toastService.show("Должен быть назначен хотя бы один токарный и фрезерный станок", "", ToastType.warn);
            return;
        }

		if (this.selectedRequest.selectedMachines != null) {
		
            this.selectedRequest.selectedMachines.forEach(x=>
                {
                      this.requestService.SaveMachineRequest(x.id,this.selectedRequest.request?.id)
                })
                this.toastService.show("Заявки для данной заявки распределены", "", ToastType.success)


                let position = this.requestsShared.findIndex(value => value.request?.id == this.selectedRequest.request?.id);
                this.requestsShared.splice(position,1);
        
                this.selectedRequest = this.requestsShared[0];
                
		}
        if(requestLocal.request != null){
            this.requestService.startMonitoringRequest(requestLocal.request);

        }



 
    }
}
