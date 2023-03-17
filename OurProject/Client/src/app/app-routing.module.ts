import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserListComponent } from './admin/user-list/user-list.component';
import { DictionaryComponent } from './dictionary/dictionary.component';
import { InitCheckTableComponent } from './init-check-table/init-check-table.component';
import { PlanningComponent } from './planning/planning/planning.component';
import { RequestComponent } from './request/request/request.component';
import { AuthorisationComponent } from './starting-components/authorisation/authorisation.component'; 
import { NavigationCardsComponent } from './starting-components/NavigationCardsComponent/navigation-cards/navigation-cards.component';
import { ProfileComponent } from './starting-components/ProfileComponent/profile/profile.component';
import { StatisticComponent } from './statistic/statistic.component';

const routes: Routes = [
  {path:'navigationCards' , component:NavigationCardsComponent},
  {path:'profile' , component:ProfileComponent},
  {path:'auth' , component:AuthorisationComponent},
  {path:'request' , component:RequestComponent},
  {path:'statistic', component:StatisticComponent},
  {path:'initCheckTable', component:InitCheckTableComponent},
  {path:'dictionary', component:DictionaryComponent},
  {path:'planning', component:PlanningComponent},
  {path:'admin/userList', component:UserListComponent},
  {path:'', redirectTo: 'navigationCards', pathMatch: 'full'}

];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
