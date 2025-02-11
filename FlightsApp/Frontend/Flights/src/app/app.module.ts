import { ViewBoughtTickets } from './components/view-tickets/view-bought-tickets.component';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';

import { HttpClient, HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';

import { LoginFormComponent } from './components/login-form/login-form.component';
import { AuthGuard } from './guards/auth.guard';
import { PurchaseTicketsComponent } from './components/purchase-tickets/purchase-tickets.component';
import { HomeComponent } from './components/home/home.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { NewFlightComponent } from './components/new-flight/new-flight.component';
import { DatePipe } from '@angular/common';
import { FlightCardComponent } from './components/flight-card/flight-card.component';
import { PurchaseTicketsApiKeyComponent } from './components/purchase-tickets-api-key/purchase-tickets-api-key.component';
import { GenerateApiKeyComponent } from './components/generate-api-key/generate-api-key.component';

export function tokenGetter() { 
  return localStorage.getItem("jwt"); 
}

@NgModule({
  declarations: [
    AppComponent,
    LoginFormComponent,
    PurchaseTicketsComponent,
    ViewBoughtTickets,
    HomeComponent,
    SidebarComponent,
    RegistrationComponent,
    NewFlightComponent,
    FlightCardComponent,
    PurchaseTicketsApiKeyComponent,
    GenerateApiKeyComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5000"],
        disallowedRoutes: []
      }
    })
  ],
  providers: [AuthGuard,DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
