import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserListComponent } from './admin/user-list/user-list.component';
import { InitCheckTableComponent } from './init-check-table/init-check-table.component';
import { RequestComponent } from './request/request/request.component';
import { AuthorisationComponent } from './starting-components/authorisation/authorisation.component'; 
import { NavigationCardsComponent } from './starting-components/NavigationCardsComponent/navigation-cards/navigation-cards.component';
import { StatisticComponent } from './statistic/statistic.component';

const routes: Routes = [
  {path:'navigationCards' , component:NavigationCardsComponent},
  {path:'auth' , component:AuthorisationComponent},
  {path:'request' , component:RequestComponent},
  {path:'statistic', component:StatisticComponent},
  {path:'initCheckTable', component:InitCheckTableComponent},
  {path:'admin/userList', component:UserListComponent},
  {path:'', redirectTo: 'navigationCards', pathMatch: 'full'}

];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
