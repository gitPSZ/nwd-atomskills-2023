import { Component, ViewChild } from '@angular/core';
import { Table } from 'primeng/table';
import { timeout } from 'rxjs';
import { Person } from 'src/app/models/Person';
import { Role } from 'src/app/models/Role';
import { PersonService } from 'src/app/services/PersonService/person.service';
import { SimpleMessageService } from 'src/app/services/SimpleMessageService/simple-message.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent {

  @ViewChild("dt1") dt1!: Table;
  loading = false;
  filterValue = '';
  people : Person[] = []
  selectedPerson : Person = {}
  roles : Role[] = []
  constructor(private personService : PersonService, private simpleMessageService : SimpleMessageService){
    console.debug(this.dt1);
  }
  async ngOnInit(){
    this.loading = true;
    this.simpleMessageService.observableUser.subscribe(async (user)=>{
      console.debug("adminUser", user)

      if(user == null || user.id == null){
        return;
      }

      this.people = await this.personService.getUsers();
      this.roles = await this.personService.getRoles();
  
      setTimeout(()=>{
        this.people.forEach((person)=>{
          person.role = this.roles.find((role)=>role.id == person.roleId);
        })
      },0);
      
      
      console.debug(this.people);
      this.loading = false;    
    })
    
  }
  clear(){
    this.filterValue = '';
    this.filter();
  }
  filter(){
    this.dt1.filterGlobal(this.filterValue, 'contains');
  }
  async changeRole(person: Person){
    if(person.id == null || person.role == null || person.role.id == null){
      return;
    }
    var result = await this.personService.changeRole(person.id, person.role.id)
    if(result){
      person.roleId = person.role.id
    }
  }
}
