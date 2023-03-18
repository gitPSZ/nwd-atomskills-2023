import { Timestamp } from "rxjs";

export interface ProductsForPosition
{
Id?:number;
RequestId?:number;
Quantity?: number;
QuantityExec?: number;
ProductId?: number;
Code?: string;
Caption?: string;
MillingTime?:number;
LatheTime?:number


}