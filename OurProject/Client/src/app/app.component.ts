import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Calendar } from 'primeng/calendar';
import { of, Subscription, timeInterval, timer } from 'rxjs';
import { environment } from 'src/environments/environment.prod';
import { BaseComponent } from './base-component/base.component';
import { SimpleMessageService } from './services/SimpleMessageService/simple-message.service';
import { PrimeNGConfig } from 'primeng/api';
import { ConfigService } from './services/ConfigService/config.service';
import { StatusService } from './services/StatusService/status.service';
import { ToastService, ToastType } from './services/ToastService/toast.service';
import { CookieService } from './services/CookieService/cookie.service';
import { AuthenticationService } from './services/AuthenticationService/authentication.service';
import { IfStmt } from '@angular/compiler';
import { NavigationButton } from './models/NavigationButton';
import { AgileInterfaceService } from './services/AgileInterfaceService/agile-interface.service';
import { RequestService } from './request/request/request.service';
import { RequestModel } from './newModels/RequestModel';
import { NotificationModel } from './newModels/NotificationModel';
import { OverlayPanel } from 'primeng/overlaypanel';
import { MachineModel } from './newModels/MachineModel';
import { DictionaryService } from './dictionary/dictionary.service';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent extends BaseComponent {
    @ViewChild("op") op : OverlayPanel | undefined;
	title = 'projectNew';
	isOpen = true;
	isToolbarAndMenuVisible = true;
	ru: any;
	timeLeft: number = 5;
  	interval: any;
	messageCount: number = 0;
	saveMessageCount: number = 0;
	visibleCount:number = 0;
	isWarning=true;
	cardsRequestAll: RequestModel[] = [];
	allEquipment: MachineModel[] = [];
	cardsRequestNotified: RequestModel[] = [];
	notifications: NotificationModel[] = [];
    showNotifications = false;
	constructor(public messageService: SimpleMessageService, private requestService : RequestService, 
		public router: Router, public configService: ConfigService, private statusService: StatusService, private config: PrimeNGConfig,
		private toastService: ToastService, private dictionaryService : DictionaryService, private cookieService: CookieService, private authenticationService: AuthenticationService, private agileInterfaceService : AgileInterfaceService) {
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
        setTimeout(()=> this.showNotifications = true, 5000)
		
	}

	async ngOnInit() {
		this.trySendToAPI();
		this.startTimer();
		
	}
    getUnreadNotificationNumber() : number{
        return this.notifications.filter((value)=>value.isRead == false).length;
    }
    async startTimer() {
		this.interval = setInterval(async () => {
		    await this.checkMessage();
            await this.checkEquipment()
		},2000)
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

        var notificationSubscription = this.messageService.observableNotificationMessage.subscribe((messageModel)=>{
            if(messageModel.text == null){
                return;
            }

            this.notifications.push(messageModel);
            this.notifications = this.notifications.sort(function(a,b){
                if(a.notificationDate == null || b.notificationDate == null){
                    return 0;
                }
                if(a.notificationDate > b.notificationDate){
                    return -1
                }
                else if(a.notificationDate < b.notificationDate){
                    return 1;
                }

                if(a.id == null || b.id == null){
                    return -1;
                }
                if(a.id > b.id){
                    return -1;
                }
                return 1
            })
        })
        this.subscriptions.push(notificationSubscription);

        setTimeout(()=>{
            var subscriptionOP = this.op?.onHide.subscribe(()=>{
                this.notifications.forEach((notification)=>{
                    notification.isRead = true;
                })
            });
    
            if(subscriptionOP != null){
                this.subscriptions.push(subscriptionOP)
            }
        },0)
        
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
    async checkEquipment(){
        if(this.allEquipment.length == 0){
            this.allEquipment = await this.dictionaryService.getDictionary();
            return;
        }
        var equipmentActual = await this.dictionaryService.getDictionary();
        this.allEquipment.forEach((equipment)=>{
            var singleEquipmentActial = equipmentActual.find(value => value.id == equipment.id);
            if(equipment.idState != 3 && singleEquipmentActial?.idState == 3){
                this.messageService.observableNotificationMessage.next({
                    header : 'Возникла поломка оборудования ' + equipment.id,
                    isRead: false,
                    notificationDate : new Date(),
                    text : `Оборудование ${equipment.id} перешло в состояние поломки`
                })
                this.toastService.show('Возникла поломка оборудования ' + equipment.id,
                `Оборудование ${equipment.id} перешло в состояние поломки`,ToastType.error);
            }
        })

        this.allEquipment = equipmentActual;

    }
	async checkMessage()
	{
		this.cardsRequestAll = await this.requestService.getRequests();
        this.cardsRequestAll.forEach((request)=>{

            var notifiedRequest = this.cardsRequestNotified.find(p=>p.id == request.id);
            if(notifiedRequest != null && notifiedRequest.id != null){
                return;
            }
            this.cardsRequestNotified.push(request);
            this.messageService.observableNotificationMessage.next({
                id: request.id,
                header : "Пришла новая заявка № " + request.id,
                isRead : false,
                notificationDate : new Date(),
                text : "От " + request.contractorName
            })
            if(this.showNotifications){
                this.toastService.show("Пришла новая заявка № " + request.id,"От " + request.contractorName);
                
            }
        })
		this.saveMessageCount = this.messageCount;
		this.isWarning = false;

	}
    refreshMessageStatus(){
        this.op
    }
}
