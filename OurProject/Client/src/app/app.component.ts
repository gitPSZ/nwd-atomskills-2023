import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Calendar } from 'primeng/calendar';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment.prod';
import { BaseComponent } from './base-component/base.component';
import { SimpleMessageService } from './services/SimpleMessageService/simple-message.service';
import { PrimeNGConfig } from 'primeng/api';
import { ConfigService } from './services/ConfigService/config.service';
import { StatusService } from './services/StatusService/status.service';
import { ToastService } from './services/ToastService/toast.service';
import { CookieService } from './services/CookieService/cookie.service';
import { AuthenticationService } from './services/AuthenticationService/authentication.service';
import { IfStmt } from '@angular/compiler';
import { NavigationButton } from './models/NavigationButton';
import { AgileInterfaceService } from './services/AgileInterfaceService/agile-interface.service';
@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent extends BaseComponent {
	title = 'projectNew';
	isOpen = true;
	isToolbarAndMenuVisible = true;
	ru: any;

	constructor(public messageService: SimpleMessageService,
		public router: Router, public configService: ConfigService, private statusService: StatusService, private config: PrimeNGConfig,
		toastService: ToastService, private cookieService: CookieService, private authenticationService: AuthenticationService, private agileInterfaceService : AgileInterfaceService) {
		super();

		this.config.setTranslation({
			accept: 'Accept',
			reject: 'Cancel',
			//translations
		});
		this.ru = configService.ru
		this.config.setTranslation(this.ru);

		this.setupSubscriptions();
		//Toolbar and menu visibility
		
	}

	async ngOnInit() {
		this.trySendToAPI();
	}
    
	setupSubscriptions(){
		var subscription = this.messageService.observableMainDesignVisibility.subscribe((isVisible) => {
			this.isToolbarAndMenuVisible = isVisible;
		});
		this.subscriptions.push(subscription);		

		var tokenSubscription = this.messageService.observableToken.subscribe(async (token)=>{
			if(token == null || token == ''){
				this.messageService.observableUser.next({});
				return;
			}
			var person = await this.authenticationService.getCurrentUser();
			
			this.messageService.observableUser.next(person);

		});
		this.subscriptions.push(tokenSubscription);		

		var shortClientNameSubscription = this.messageService.observableUser.subscribe((user)=>{
			var nameArray : string[] = [];
			if(user == null || user.nameClient == null){
				this.messageService.observableShortClientName.next('');
				return;
			}
			nameArray = user.nameClient?.split(' ');

			if(nameArray == null){
				return;
			}
			var shortName = `${nameArray[0]} ${nameArray[1] == null ? '' : nameArray[1][0]+'.'} ${nameArray[2] == null ? '' : nameArray[2][0] + '.'}`;
			this.messageService.observableShortClientName.next(shortName);
		})
		this.subscriptions.push(shortClientNameSubscription);	

		var navigationButtonsSubscription = this.messageService.observableToken.subscribe(async (token)=>{
			this.messageService.observableNavigationButtons.next(await this.agileInterfaceService.getNavigationButtons());
		})
		this.subscriptions.push(navigationButtonsSubscription);		
	}
	async trySendToAPI(){
		let status = await this.statusService.getServerStatus();
		if (status.id == null) {
			window.location.href = environment.apiURL + "/status"
		}
	}
	changeMenuState() {
		this.isOpen = !this.isOpen;
	}
	signIn() {
		this.router.navigateByUrl("auth");
	}

	async redirect(button : NavigationButton) {
		if(button.routerLink == null){
			return;
		}
		this.router.navigateByUrl(button.routerLink);
		if(button.caption == null){
			return;
		}
		this.messageService.observableTitle.next(button.caption)
	}
	async redirectToInitCheckTable() {
		console.debug("перенаправление");
		this.router.navigateByUrl("initCheckTable");
	}
	async logOut(){
		this.authenticationService.logOut()
	}
}
