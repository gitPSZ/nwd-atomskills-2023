import { Timestamp } from "rxjs";

export interface RequestModel
{
    id?:number;
    number?:number;
    date?: Date;
    repleaseDate?: Date;
    description?: string;
    idContractor?: number;
    stateCode?: string;
    stateCaption?:string;
    contractorName?:string;
    priority?:number;
    notificationDate?:string

    


}