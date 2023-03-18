import { Timestamp } from "rxjs";
import { MachineModel } from "./MachineModel";
import { RequestModel } from "./RequestModel";

export interface RequestSharedModel
{
    request?: RequestModel;

    machines: MachineModel[];
    selectedMachines: MachineModel[];


}