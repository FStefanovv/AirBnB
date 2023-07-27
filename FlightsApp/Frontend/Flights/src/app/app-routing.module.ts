import { ViewBoughtTickets } from './components/view-tickets/view-bought-tickets.component';
import { NgModule, Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginFormComponent } from './components/login-form/login-form.component';
import { PurchaseTicketsComponent } from './components/purchase-tickets/purchase-tickets.component';
import { HomeComponent } from './components/home/home.component';

import { AuthGuard } from './guards/auth.guard';
import { RegistrationComponent } from './components/registration/registration.component';
import { NewFlightComponent } from './components/new-flight/new-flight.component';
import { PurchaseTicketsApiKeyComponent } from './components/purchase-tickets-api-key/purchase-tickets-api-key.component';
import { GenerateApiKeyComponent } from './components/generate-api-key/generate-api-key.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch:'full'},
  { path: 'home', component: HomeComponent},
  { path: 'login', component: LoginFormComponent},
  { path: 'register', component: RegistrationComponent},
  { path: 'purchase-tickets-regular', component: PurchaseTicketsComponent, canActivate:[AuthGuard], data: {roles: ['REGULAR_USER']}},
  { path: 'new-flight', component: NewFlightComponent, canActivate:[AuthGuard], data: {roles: ['ADMIN']}},
  { path: 'view-bought-tickets', component:ViewBoughtTickets, canActivate:[AuthGuard], data: {roles: ['REGULAR_USER']}},
  { path: 'purchase-tickets-api', component:PurchaseTicketsApiKeyComponent},
  { path: 'generate-api-key', component: GenerateApiKeyComponent, canActivate:[AuthGuard], data: {roles: ['REGULAR_USER']}}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
