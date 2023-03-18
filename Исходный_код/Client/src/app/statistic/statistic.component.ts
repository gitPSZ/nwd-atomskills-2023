import { Component } from '@angular/core';
import { TypeRequest } from 'src/app/models/TypeRequest';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { catchError, from, groupBy, lastValueFrom, map, mergeMap, Observable, of, pipe, timer, toArray } from 'rxjs';
import { Router } from '@angular/router';
import { Input } from 'postcss';
import { StatusService } from 'src/app/services/StatusService/status.service';
import { timeout } from 'rxjs';
import { BaseComponent } from 'src/app/base-component/base.component';
import { SimpleMessageService } from 'src/app/services/SimpleMessageService/simple-message.service';
import { environment } from 'src/environments/environment.prod';
import { ConfigService } from 'src/app/services/ConfigService/config.service';
import { RequestModel } from 'src/app/models/RequestModel';
import { RequestService } from '../request/request/request.service';
import { ClaimModel } from '../models/ClaimModel';
import { mergeNsAndName } from '@angular/compiler';
import { State } from '../models/State';


@Component({
	selector: 'app-statistic',
	templateUrl: './statistic.component.html',
	styleUrls: ['./statistic.component.css']
})
export class StatisticComponent extends BaseComponent {

	claims: ClaimModel[] = [];
	states: State[] = [];
	groupedClaims: GroupedClaims[] = [];
	groupedClaimsByExecutor: GroupedClaims[] = [];


	selectedState: any;
	stateData: any;
	executorData: any;
	async ngOnInit() {
		var userSubscription = this.messageService.observableUser.subscribe(async (value) => {

			if (value.id == null) {
				return;
			}
			this.groupedClaimsByExecutor = [];
			this.groupedClaims = [];
			this.claims = await this.requestService.getClaimsWithAllAttributes();
			this.states = await this.requestService.getStates();

			var observable = from(this.claims).pipe(
				groupBy(o => o.state),
				mergeMap(group => group.pipe(toArray()))
			);
			observable.subscribe(r => {
				this.groupedClaims.push({ claims: r, groupFactor: r[0].state })
			});

			this.setUpData(observable);

			var observableExecutors = from(this.claims).pipe(
				groupBy(o => o.executor),
				mergeMap(group => group.pipe(toArray()))
			);
			observableExecutors.subscribe(r => {
				this.groupedClaimsByExecutor.push({ claims: r, groupFactor: r[0].executor == null ? 'Без исполнителя' : r[0].executor })
				this.setUpData(observableExecutors);
			}

			);
		})

		this.subscriptions.push(userSubscription);


	}
	constructor(private http: HttpClient, private configService: ConfigService,
		private messageService: SimpleMessageService, private router: Router, private statusService: StatusService, private requestService: RequestService) {
		super();
		this.selectedState = "byState"
	}
	setUpData(observable: Observable<ClaimModel[]>) {


		this.setUpState(observable)
		this.setUpExecutors(observable)


	}
	setUpExecutors(observable: Observable<ClaimModel[]>) {
		var executorString = this.groupedClaimsByExecutor.flatMap((value) => value.groupFactor);

		let data: any = [];

		executorString.forEach(executorString => {
			var result = this.groupedClaimsByExecutor.find(value => value.groupFactor == executorString);
			if (result != null) {
				data.push(result.claims?.length);
			}
			else {
				data.push(0);
			}
		})


		this.executorData = {
			labels: executorString,
			datasets: [
				{
					label: 'По исполнителю',
					backgroundColor: '#42A5F5',
					data: data
				}
			]
		};
	}

	//Делаем данные для состояний
	setUpState(observable: Observable<ClaimModel[]>) {
		var stateString = this.states.flatMap((value) => value.captionState);
		let data: any = [];


		stateString.forEach(stateString => {
			var result = this.groupedClaims.find(value => value.groupFactor == stateString);
			if (result != null) {
				data.push(result.claims?.length);
			}
			else {
				data.push(0);
			}
		})

		this.stateData = {
			labels: stateString,
			datasets: [
				{
					label: 'По статусу заявки',
					backgroundColor: '#42A5F5',
					data: data
				}
			]
		};
	}
	radioButtonClick(selectedValue: string) {

	}
}
export class GroupedClaims {
	claims?: ClaimModel[]
	groupFactor?: string

}


