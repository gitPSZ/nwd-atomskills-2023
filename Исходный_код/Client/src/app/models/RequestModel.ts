import { Timestamp } from "rxjs";

export interface RequestModel
{
    id?:number;
    idType?:number;
    placeOfService?:string;
    idPriority?:number;
    text?:string;
    id_executor?:number;
    CreatedAt?:Date;
    IdAuthor?:number;
}