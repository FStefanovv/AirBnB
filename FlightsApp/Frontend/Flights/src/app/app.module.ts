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

export function tokenGetter() { 
  return localStorage.getItem("jwt"); 
}

@NgModule({
  declarations: [
    AppComponent,
    LoginFormComponent,
    PurchaseTicketsComponent,
    HomeComponent,
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
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
