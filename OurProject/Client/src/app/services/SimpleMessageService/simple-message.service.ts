import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { NavigationButton } from 'src/app/models/NavigationButton';
import { Person } from 'src/app/models/Person';
import { NotificationModel } from 'src/app/newModels/NotificationModel';
import { environment } from 'src/environments/environment.prod';
import { AuthenticationService } from '../AuthenticationService/authentication.service';
import { CookieService } from '../CookieService/cookie.service';

@Injectable({
  providedIn: 'root'
})
export class SimpleMessageService {

  observableMainDesignVisibility:BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  observableToken:BehaviorSubject<string> = new BehaviorSubject<string>('');
  observableUser:BehaviorSubject<Person> = new BehaviorSubject<Person>({});
  observableNavigationButtons:BehaviorSubject<NavigationButton[]> = new BehaviorSubject<NavigationButton[]>([]);
  observableShortClientName:BehaviorSubject<string> = new BehaviorSubject<string>('');
  observableTitle:BehaviorSubject<string> = new BehaviorSubject<string>('Управление IT услугами');
  observableNotificationMessage:BehaviorSubject<NotificationModel> = new BehaviorSubject<NotificationModel>({});

  constructor(private cookieService : CookieService) {
    this.observableToken.next(this.cookieService.getCookie(environment.tokenKey));
  }

  changeMainDesignVisibility(isVisible : boolean){
    this.observableMainDesignVisibility.next(isVisible);    
  }
}
