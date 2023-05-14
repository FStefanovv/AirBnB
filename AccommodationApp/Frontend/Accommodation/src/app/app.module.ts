import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';

import { LoginComponent } from './components/login/login.component';
import { CreateAccommodationComponent } from './components/create-accommodation/create-accommodation.component';
import { AuthGuard } from './auth-guard/auth.guard';
import { HomeComponent } from './components/home/home.component';

import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { UserReservationsComponent } from './components/user-reservations/user-reservations.component';
import { PendingRequestsComponent } from './components/pending-requests/pending-requests.component';
import { UserInfoChangeComponent } from './components/user-info-change/user-info-change.component';
import { RegistrationComponent } from './components/registration/registration.component';

import {MatToolbarModule} from '@angular/material/toolbar';
import { CreateRequestComponent } from './components/create-request/create-request.component';

export function tokenGetter() { 
  return localStorage.getItem("jwt"); 
}


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    CreateAccommodationComponent,
    HomeComponent,
    UserReservationsComponent,
    PendingRequestsComponent,
    UserInfoChangeComponent,
    RegistrationComponent,
    CreateRequestComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5000"],
        disallowedRoutes: []
      }
    }),
    NgMultiSelectDropDownModule.forRoot(),
    MatToolbarModule
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
