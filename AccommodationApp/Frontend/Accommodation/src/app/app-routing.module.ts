import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { CreateAccommodationComponent } from './components/create-accommodation/create-accommodation.component';
import { AuthGuard } from './auth-guard/auth.guard';
import { HomeComponent } from './components/home/home.component';
import { UserReservationsComponent } from './components/user-reservations/user-reservations.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch:'full' },
  { path: 'home', component: HomeComponent },
  { path:'login', component: LoginComponent },
  { path:'create-accommodation', component: CreateAccommodationComponent, canActivate:[AuthGuard], data: {roles: ['HOST']} },
  { path:'my-reservations', component: UserReservationsComponent, canActivate:[AuthGuard], data: {roles: ['REGULAR_USER']} }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
