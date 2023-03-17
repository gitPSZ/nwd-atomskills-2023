import { Timestamp } from "rxjs";

export interface RequestModel
{
Id?:number;
Number?:number;
Date?: Date;
RepleaseDate?: Date;
Description?: string;
IdContractor?: number;
StateCode?: string;
StateCaption?:string;
ContractorName?:string

}