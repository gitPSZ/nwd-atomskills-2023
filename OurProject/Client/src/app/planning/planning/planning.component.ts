import { Component } from '@angular/core';
import { BaseComponent } from 'src/app/base-component/base.component';
import { RequestModel } from 'src/app/models/RequestModel';
import { RequestService } from 'src/app/request/request/request.service';

@Component({
  selector: 'app-planning',
  templateUrl: './planning.component.html',
  styleUrls: ['./planning.component.css']
})
export class PlanningComponent extends BaseComponent{

    requests : RequestModel[] = []
    selectedRequest: RequestModel = {}
    filterValue : string = '';
    constructor(private requestService : RequestService){
        super();
    }
    async ngOnInit(){
        this.requests = await this.requestService.getRequests();
    }
    myResetFunction(options:any){

    }
}
