import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/register/register.component';
import { AutomobileListComponent } from './components/automobile-list/automobile-list.component';
import { PublicPatComponent } from './components/public-pat/public-pat.component';
import { PrivatePatComponent } from './components/private-pat/private-pat.component';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'autoList', component: AutomobileListComponent },
  { path: 'PublicPat', component: PublicPatComponent },
  { path: 'PrivatePat', component: PrivatePatComponent },



  { path: '**', redirectTo: '/login' }
];
