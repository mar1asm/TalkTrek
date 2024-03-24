import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { AuthComponent } from './components/auth/auth.component';
import { AccountComponent } from './components/account/account.component';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';
import { PresentationComponent } from './presentation/presentation.component';
import { SearchComponent } from './components/search/search.component';

const routes: Routes = [
  {
    path: 'home', component: HomeComponent
  },
  {
    path: 'auth', component: AuthComponent
  },
  {
    path: 'account', component: AccountComponent
  },
  {
    path: 'confirm-email', component: ConfirmEmailComponent
  },
  {
    path: 'search', component: SearchComponent
  },
  {
    path: '', component: PresentationComponent
  },
  {
    path: '*', redirectTo: 'home'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
