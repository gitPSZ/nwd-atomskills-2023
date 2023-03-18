import { Timestamp } from "rxjs";

export interface MachineModel
{
    id?:string;
    machineType?:string;
    machineTypeCaption?:string;
    port?:number;
    isDeleted?:string;
    idState?:number;
    state?:string;
    idRequest?:number;
}