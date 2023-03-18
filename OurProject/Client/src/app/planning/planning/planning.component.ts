import { Component } from '@angular/core';
import { BaseComponent } from 'src/app/base-component/base.component';
import { MachineModel } from 'src/app/newModels/MachineModel';
import { ProductsForPosition } from 'src/app/newModels/ProductsForPosition';
import { RequestModel } from 'src/app/newModels/RequestModel';
import { RequestService } from 'src/app/request/request/request.service';
import { dictionaryService } from '../../dictionary/dictionary.service';

@Component({
  selector: 'app-planning',
  templateUrl: './planning.component.html',
  styleUrls: ['./planning.component.css']
})
export class PlanningComponent extends BaseComponent{

    requests : RequestModel[] = [];
    products: ProductsForPosition[]= [];
    selectedRequest: RequestModel = {}
    selectedProduct: ProductsForPosition = {};
    machine: MachineModel[] = [];
    selectedMachine: MachineModel[] = [];
    filterValue : string = '';
    constructor(private requestService : RequestService, private dictionaryService : dictionaryService){
        super();
    }
    async ngOnInit(){
        this.requests = await this.requestService.getRequests();
        this.selectedRequest = this.requests[0];
        this.products = await this.requestService.getProductsForPosition(this.selectedRequest);
        this.machine = await this.dictionaryService.getDictionary();

        console.log(this.machine);
    }
    myResetFunction(options:any){

    }
    async onChangeRequest(event: any) {
        this.products = await this.requestService.getProductsForPosition(this.selectedRequest);
    }
}
