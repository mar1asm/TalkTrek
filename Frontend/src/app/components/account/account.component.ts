import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountDetailsComponent } from '../account-details/account-details.component';
import { ChangePasswordComponent } from '../change-password/change-password.component';
import { TutorProfileDetailsComponent } from '../tutor-profile-details/tutor-profile-details.component';

interface Tab {
  key: string;
  name: string;
  component: any;
  icon: any;
}


export const tabs: Map<string, Tab> = new Map<string, Tab>([
  ['account-details', { key: 'account-details', name: 'Details', component: AccountDetailsComponent, icon: 'account_box' }],
  ['tutor-profile', { key: 'tutor-profile', name: 'Profile', component: TutorProfileDetailsComponent, icon: 'account_box' }],
  ['change-password', { key: 'change-password', name: 'Change password', component: ChangePasswordComponent, icon: 'history' }],
  ['payment-methods', { key: 'payment-methods', name: 'Payment methods', component: AccountDetailsComponent, icon: 'account_balance_wallet' }],
  ['payment-history', { key: 'payment-history', name: 'Payment history', component: AccountDetailsComponent, icon: 'settings' }]
]);

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrl: './account.component.scss'
})



export class AccountComponent implements OnInit {

  selectedTabKey: string = "account-details"
  selectedTabComponent: any
  tabsArray = Array.from(tabs.values())


  constructor(private route: ActivatedRoute, private authService: AuthService, private router: Router) {

  }

  ngOnInit(): void {
    this.authService.isLoggedIn$.subscribe((isLoggedIn: boolean) => {
      !isLoggedIn && this.router.navigate(['auth'], { queryParams: { action: 'login' } }); //DEV ONLY
    });
    this.route.queryParams.subscribe(params => {
      this.selectedTabKey = params['tab'] || 'account-details';
      this.selectedTabComponent = tabs.get(this.selectedTabKey)?.component
    });
  }



  onChangeTab(tabKey: any) {
    this.selectedTabKey = tabKey
    this.selectedTabComponent = tabs.get(this.selectedTabKey)?.component
  }
}
