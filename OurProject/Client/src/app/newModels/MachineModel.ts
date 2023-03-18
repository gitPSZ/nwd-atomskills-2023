import { Timestamp } from "rxjs";

export interface MachineModel
{
    id?:number;
    machineType?:string;
    machineTypeCaption?:string;
    port?:number;
    isDeleted?:string;
    idState?:number;
    state?:string;
}