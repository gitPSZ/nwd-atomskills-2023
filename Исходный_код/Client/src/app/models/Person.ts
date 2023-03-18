import { Role } from "./Role";

export interface Person
{
    id?:number;
    nameClient?:string;    
    
    roleName?: string;
    roleId?:number;
    password?:string;
    email?:string;
    login?:string;

    initialRole?:Role
    role?: Role;
}

