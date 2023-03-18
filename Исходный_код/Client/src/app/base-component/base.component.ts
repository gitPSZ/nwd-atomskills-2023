import { Component } from '@angular/core';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-base',
  templateUrl: './base.component.html',
  styleUrls: ['./base.component.css']
})
export class BaseComponent {
  subscriptions:Subscription[] = []; 

  public constructor(){
    console.debug("ComponentShown: " + this.constructor.name);
  }
  
  ngOnDestroy(){
    this.subscriptions.forEach(element => {
      element.unsubscribe();
    });
  }
}

