import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/register/register.component';
import { AutomobileListComponent } from './components/automobile-list/automobile-list.component';
import { PublicPatComponent } from './components/public-pat/public-pat.component';
import { PrivatePatComponent } from './components/private-pat/private-pat.component';
import { ListAutoInsuranceComponent } from './components/list-auto-insurance/list-auto-insurance.component';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'home',
    component: HomeComponent,
    children: [
      { path: 'autoList', component: AutomobileListComponent },
      { path: 'PublicPat', component: PublicPatComponent },
      { path: 'PrivatePat', component: PrivatePatComponent },
      { path: 'ListAutoInsurance', component: ListAutoInsuranceComponent },

      { path: '', redirectTo: '', pathMatch: 'full' }
    ]
  },
  { path: '**', redirectTo: '/login' }
];
