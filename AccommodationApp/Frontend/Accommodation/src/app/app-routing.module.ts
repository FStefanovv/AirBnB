import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { CreateAccommodationComponent } from './components/create-accommodation/create-accommodation.component';
import { AuthGuard } from './auth-guard/auth.guard';
import { HomeComponent } from './components/home/home.component';
import { UserReservationsComponent } from './components/user-reservations/user-reservations.component';
import { PendingRequestsComponent } from './components/pending-requests/pending-requests.component';
import { UserInfoChangeComponent } from './components/user-info-change/user-info-change.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { UpdateAccommodationComponent } from './components/update-accommodation/update-accommodation.component';
import { FlightRecommendationsComponent } from './components/flight-recommendations/flight-recommendations.component';
import { BuyFlightTicketsComponent } from './components/buy-flight-tickets/buy-flight-tickets.component';
import { ShowAccommodationComponent } from './components/show-accommodation/show-accommodation.component';
import { CreateRequestComponent } from './components/create-request/create-request.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch:'full' },
  { path: 'home', component: HomeComponent },
  { path:'login', component: LoginComponent },
  { path:'create-accommodation', component: CreateAccommodationComponent, canActivate:[AuthGuard], data: {roles: ['HOST']} },
  { path:'my-reservations', component: UserReservationsComponent, canActivate:[AuthGuard], data: {roles: ['REGULAR_USER']} },
  { path:'pending-requests', component: PendingRequestsComponent, canActivate:[AuthGuard], data: {roles: ['HOST']} },
  { path:'get-host', component: UserInfoChangeComponent, canActivate:[AuthGuard], data: {roles: ['HOST']}},
  { path:'get-regular', component: UserInfoChangeComponent, canActivate:[AuthGuard], data: {roles: ['REGULAR_USER']}},
  { path:'register-user', component: RegistrationComponent},
  { path:'update-accommodation/:id/:startSeason/:endSeason/:price', component: UpdateAccommodationComponent},
  { path:'flight-recommendations/:id', component: FlightRecommendationsComponent, canActivate:[AuthGuard], data: {roles: ['REGULAR_USER']}},
  { path: 'buy-flight-tickets', component: BuyFlightTicketsComponent, canActivate:[AuthGuard], data: {roles: ['REGULAR_USER']}},
  { path: 'show-accommodation/:id', component: ShowAccommodationComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
