import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';

import { Calendar, CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown'
import { ButtonModule } from 'primeng/button';
import { Table, TableModule } from 'primeng/table';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { AuthorisationComponent } from './starting-components/authorisation/authorisation.component';
import { AppRoutingModule } from './app-routing.module';
import { SimpleMessageService } from './services/SimpleMessageService/simple-message.service';
import { InputTextModule } from 'primeng/inputtext'
import { ToastModule } from 'primeng/toast'
import { ErrorService } from './services/ErrorService/error.service';
import { ConfigService } from './services/ConfigService/config.service';
import { StatusService } from './services/StatusService/status.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RequestComponent } from './request/request/request.component';
import { StatisticComponent } from './statistic/statistic.component';
import { DialogModule } from 'primeng/dialog';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { MessageService } from 'primeng/api';
import { AuthenticationService } from './services/AuthenticationService/authentication.service';
import { TokenInterceptorService } from './services/Interceptors/token-interceptor.service';
import { PasswordModule } from 'primeng/password';
import { DividerModule } from 'primeng/divider';
import { InitCheckTableComponent } from './init-check-table/init-check-table.component';
import { CookieService } from './services/CookieService/cookie.service';
import {ChartModule} from 'primeng/chart';
import {MultiSelectModule} from 'primeng/multiselect';
import {ContextMenuModule} from 'primeng/contextmenu';
import {ProgressBarModule} from 'primeng/progressbar';
import { RadioButtonModule } from 'primeng/radiobutton';
import { UserListComponent } from './admin/user-list/user-list.component';
import { NavigationCardsComponent } from './starting-components/NavigationCardsComponent/navigation-cards/navigation-cards.component';
import {TooltipModule} from 'primeng/tooltip';
import {InputTextareaModule} from 'primeng/inputtextarea';
import {saveAs} from 'file-saver';
import * as FileSaver from 'file-saver';
import { ChipModule } from 'primeng/chip';
import { DictionaryComponent } from './dictionary/dictionary.component';
import { ProfileComponent } from './starting-components/ProfileComponent/profile/profile.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthorisationComponent,
    RequestComponent,
    StatisticComponent,
    InitCheckTableComponent,
    UserListComponent,
    NavigationCardsComponent,
    DictionaryComponent,
    ProfileComponent,
  ],
  imports: [
    HttpClientModule,
    CalendarModule,
    AppRoutingModule,
    ButtonModule,
    DropdownModule,
    BrowserModule,
    BrowserAnimationsModule,
    BrowserModule,
    InputTextModule,
    FormsModule,
    TableModule,
    DialogModule,
    ToastModule,
    ReactiveFormsModule,
    MessagesModule,
    MessageModule,
    PasswordModule,
    DividerModule,
    ProgressBarModule,
    ChartModule,
    TooltipModule,
    RadioButtonModule,
    InputTextareaModule,
    ContextMenuModule,
    MultiSelectModule,
    ChipModule
  ],
  providers: [ErrorService, StatusService, ConfigService, MessageService, AuthenticationService, CookieService, SimpleMessageService, AuthenticationService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorService,
      multi: true,
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
