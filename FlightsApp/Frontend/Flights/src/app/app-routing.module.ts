import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginFormComponent } from './components/login-form/login-form.component';
import { PurchaseTicketsComponent } from './components/purchase-tickets/purchase-tickets.component';
import { HomeComponent } from './components/home/home.component';

import { AuthGuard } from './guards/auth.guard';
import { RegistrationComponent } from './components/registration/registration.component';
import { NewFlightComponent } from './components/new-flight/new-flight.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch:'full'},
  { path: 'home', component: HomeComponent},
  { path: 'login', component: LoginFormComponent},
  { path: 'register', component: RegistrationComponent},
  { path: 'purchase-tickets-regular', component: PurchaseTicketsComponent, canActivate:[AuthGuard], data: {roles: ['REGULAR_USER']}},
  { path: 'new-flight', component: NewFlightComponent, canActivate:[AuthGuard], data: {roles: ['ADMIN']}}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
