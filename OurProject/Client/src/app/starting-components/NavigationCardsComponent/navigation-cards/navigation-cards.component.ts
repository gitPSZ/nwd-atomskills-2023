import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from 'src/app/base-component/base.component';
import { NavigationButton } from 'src/app/models/NavigationButton';
import { SimpleMessageService } from 'src/app/services/SimpleMessageService/simple-message.service';

@Component({
  selector: 'app-navigation-cards',
  templateUrl: './navigation-cards.component.html',
  styleUrls: ['./navigation-cards.component.css']
})
export class NavigationCardsComponent extends BaseComponent{

  constructor(public messageService: SimpleMessageService, private router: Router){
    super();
    this.messageService.observableTitle.next("Список возможностей");
  }
  ngOnInit(){

  }
  navigate(button:NavigationButton){
    if(button == null || button.routerLink == null || button.caption == null){
      return;
    }
    this.router.navigateByUrl(button.routerLink);
    this.messageService.observableTitle.next(button.caption);
  }

}
