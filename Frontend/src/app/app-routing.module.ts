import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { AuthComponent } from './components/auth/auth.component';
import { AccountComponent } from './components/account/account.component';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';
import { PresentationComponent } from './presentation/presentation.component';
import { SearchComponent } from './components/search/search.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { TutorComponent } from './components/tutor/tutor.component';

const routes: Routes = [
  {
    path: '', component: PresentationComponent, pathMatch: 'full'
  },
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
    path: '', component: PresentationComponent, pathMatch: 'full'
  },
  {
    path: 'tutor/:id', component: TutorComponent
  },

  {
    path: 'not-found', component: PageNotFoundComponent
  },
  {
    path: '**',
    redirectTo: '/not-found'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
