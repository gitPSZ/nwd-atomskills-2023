import { Timestamp } from "rxjs";

export interface NotificationModel
{
    id?:number;
    text?:string;
    header?:string;
    isRead?:boolean;
    notificationDate? : Date;
}