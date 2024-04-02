import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { AccountComponent } from './components/account/account.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthComponent } from './components/auth/auth.component';
import { AccountDetailsComponent } from './components/account-details/account-details.component';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';
import { PresentationComponent } from './presentation/presentation.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { SearchComponent } from './components/search/search.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';


import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button'
import { MatRadioModule } from '@angular/material/radio';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatListModule } from '@angular/material/list';
import { MatSliderModule } from '@angular/material/slider';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCardModule } from '@angular/material/card';
import { MatSelectCountryModule } from "@angular-material-extensions/select-country";


import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { HttpClientModule } from '@angular/common/http';
import { StoreModule } from '@ngrx/store';
import { authReducer } from '../store/reducers/auth.reducers';
import { provideNativeDateAdapter } from '@angular/material/core';
import { TutorComponent } from './components/tutor/tutor.component';
import { LevelDescriptionPipe } from './pipes/level-description.pipe';
import { TutorProfileDetailsComponent } from './components/tutor-profile-details/tutor-profile-details.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    AuthComponent,
    AccountComponent,
    AccountDetailsComponent,
    ConfirmEmailComponent,
    PresentationComponent,
    ChangePasswordComponent,
    SearchComponent,
    PageNotFoundComponent,
    TutorComponent,
    LevelDescriptionPipe,
    TutorProfileDetailsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    MatToolbarModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    MatRadioModule,
    MatSidenavModule,
    MatFormFieldModule,
    MatListModule,
    MatSelectModule,
    HttpClientModule,
    MatSidenavModule,
    MatSliderModule,
    MatDatepickerModule,
    MatCardModule,
    MatSelectCountryModule.forRoot('en'),
    StoreModule.forRoot({ auth: authReducer })

  ],
  providers: [
    provideClientHydration(),
    provideAnimationsAsync(),
    provideNativeDateAdapter()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
