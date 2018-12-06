import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { TranslateModule, TranslateLoader } from "@ngx-translate/core";

import { LocalStoreManager } from "./services/local-store-manager.service";
import { AccountService } from "./services/account.service";
import { AuthService } from "./services/auth.service";
import { ConfigurationService } from "./services/configuration.service";
import { EndpointFactory } from "./services/endpoint-factory.service";
import { AccountEndpoint } from "./services/account-endpoint.service";
import { AlertService } from "./services/alert.service";
import { NotificationService } from "./services/notification.service";
import { NotificationEndpoint } from "./services/notification-endpoint.service";
import { AppTitleService } from "./services/app-title.service";
import { MessengerService } from "./services/messengers.service";
import { MessengerEndpoint } from "./services/messengers-endpoint.service";
import { SupportService } from "./services/support.service";
import { SupportEndpoint } from "./services/support-endpoint.service";
import { AccessRightsService } from "./services/access-rights.service";
import { AccessRightsEndpoint } from "./services/access-rights.endpoint";
import { StudentsService } from "./services/students.service";
import { StudentsEndpoint } from "./services/students-endpoint.service";
import { TypeMaterialsEndpoint } from "./services/typeMaterials-endpoint.service";
import { UnitsEndpoint } from "./services/units-endpoint.service";
import { GrpMaterialsEndpoint } from "./services/grpMaterials-endpoint.service";
import { SuppliersEndpoint } from "./services/suppliers-endpoint.service";
import { StudentstatusEndpoint } from "./services/studentstatus-endpoint.service";
import { GenderStudentEndpoint } from "./services/genderstudent-endpoint.service";
import { IOStudentListService } from "./services/iostudentlists.service";
import { IOStudentListEndpoint } from "./services/iostudentlists-endpoint.service";
import { MaterialsService } from "./services/materials.service";
import { MaterialsEndpoint } from "./services/materials-endpoint.service";
import { ConsultantsService } from "./services/consultants.service";
import { ConsultantsEndpoint } from "./services/consultants-endpoint.service";
import { IOStudentsService } from "./services/iostudents.service";
import { IOStudentsEndpoint } from "./services/iostudents-endpoint.service";
import { TypeMaterialLearnsService } from "./services/typeMaterialLearns.service";
import { TypeMaterialLearnsEndpoint } from "./services/typeMaterialLearns-endpoint.service";
import { BranchesEndpoint } from "./services/branches-endpoint.service";
import { BranchesService } from "./services/branches.service";
import { AppTranslationService, TranslateLanguageLoader } from './services/app-translation.service';

import { AlteComponent } from './components/alte.component';
import { HomeComponent } from './components/home/home.component';
import { HeaderComponent } from './shares/header/header.component';
import { MenuComponent } from './shares/menu/menu.component';
import { FooterComponent } from './shares/footer/footer.component';
import { SettingsComponent } from './shares/settings/settings.component';
import { LoginComponent } from './components/login/login.component';
import { HttpModule } from '@angular/http';


@NgModule({
  declarations: [
    AlteComponent,
    HomeComponent,
    HeaderComponent,
    MenuComponent,
    FooterComponent,
    SettingsComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpModule,
    FormsModule,
    TranslateModule.forRoot({
      loader: {
          provide: TranslateLoader,
          useClass: TranslateLanguageLoader,
          
      }
  }),
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      // { path: 'counter', component: CounterComponent },
      // { path: 'fetch-data', component: FetchDataComponent },
    ])
  ],
  providers: [
    LocalStoreManager,
        AccountService,     
        AuthService,   
        ConfigurationService,
        AppTranslationService,
        EndpointFactory,
        AccountEndpoint,
        AlertService,
        NotificationService,
        NotificationEndpoint,
        AppTitleService,
        MessengerService,
        MessengerEndpoint,
        SupportService,
        SupportEndpoint,
        AccessRightsService,
        AccessRightsEndpoint,
        StudentsService,
        StudentsEndpoint,
        TypeMaterialsEndpoint,
        UnitsEndpoint,
        GrpMaterialsEndpoint,
        SuppliersEndpoint,
        StudentstatusEndpoint,
        GenderStudentEndpoint,
        IOStudentListService,
        IOStudentListEndpoint,
        MaterialsService,
        MaterialsEndpoint,
        ConsultantsService,
        ConsultantsEndpoint,
        IOStudentsService,
        IOStudentsEndpoint,
        TypeMaterialLearnsService,
        TypeMaterialLearnsEndpoint,
        TypeMaterialsEndpoint,
        BranchesService,
        BranchesEndpoint
  ],
  bootstrap: [AlteComponent]
})
export class AppModule { }
