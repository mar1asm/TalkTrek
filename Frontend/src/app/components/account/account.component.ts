import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountDetailsComponent } from '../account-details/account-details.component';
import { AccountDashboardComponent } from '../account-dashboard/account-dashboard.component';

interface Tab {
  key: string;
  name: string;
  component: any;
  icon: any;
}


export const tabs: Map<string, Tab> = new Map<string, Tab>([
  ['dashboard', { key: 'dashboard', name: 'Dashboard', component: AccountDashboardComponent, icon: 'dashboard' }],
  ['account-details', { key: 'account-details', name: 'Account details', component: AccountDetailsComponent, icon: 'account_box' }],
  ['history', { key: 'history', name: 'History', component: AccountDetailsComponent, icon: 'history' }],
  ['payments', { key: 'payments', name: 'Payments', component: AccountDetailsComponent, icon: 'account_balance_wallet' }],
  ['settings', { key: 'settings', name: 'Settings', component: AccountDetailsComponent, icon: 'settings' }],
  ['logout', { key: 'logout', name: 'Logout', component: AccountDetailsComponent, icon: 'logout' }]
]);

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrl: './account.component.scss'
})



export class AccountComponent implements OnInit {

  selectedTabKey: string = "Dashboard"
  selectedTabComponent: any
  tabsArray = Array.from(tabs.values())


  constructor(private route: ActivatedRoute, private authService: AuthService, private router: Router) {

  }

  ngOnInit(): void {
    this.authService.isLoggedIn$.subscribe((isLoggedIn: boolean) => {
      console.log("Is logged in " + isLoggedIn)
      !isLoggedIn && this.router.navigate(['auth'], { queryParams: { action: 'login' } });
    });
    this.route.queryParams.subscribe(params => {
      this.selectedTabKey = params['tab'] || 'dashboard';
      this.selectedTabComponent = tabs.get(this.selectedTabKey)?.component
    });
  }



  onChangeTab(tabKey: any) {
    console.log('ok')
    this.selectedTabKey = tabKey
    this.selectedTabComponent = tabs.get(this.selectedTabKey)?.component
  }
}
