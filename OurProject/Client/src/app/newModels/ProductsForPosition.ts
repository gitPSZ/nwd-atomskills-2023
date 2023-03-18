import { Timestamp } from "rxjs";

export interface ProductsForPosition
{
Id?:number;
RequestId?:number;
quantity?: number;
QuantityExec?: number;
ProductId?: number;
Code?: string;
Caption?: string;
millingTime?:number;
latheTime?:number;



}