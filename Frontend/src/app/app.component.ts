import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountDetailsComponent } from './components/account-details/account-details.component';
import { AccountComponent } from './components/account/account.component';

interface Tab {
  key: string;
  name: string;
  redirectPath: any;
  icon: any;
}


export const tabs: Map<string, Tab> = new Map<string, Tab>([
  ['home', { key: 'home', name: 'Home', redirectPath: '/home', icon: 'home' }],
  ['search', { key: 'search', name: 'Search', redirectPath: '/search', icon: 'search' }],
  ['account', { key: 'account', name: 'Account', redirectPath: '/account', icon: 'person' }],
  ['messages', { key: 'messages', name: 'Messages', redirectPath: '/messages', icon: 'message' }],
  ['classes', { key: 'classes', name: 'Classes', redirectPath: '/classes', icon: 'calendar_today' }],
]);


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {

  title = 'learning-platform';
  isLoggedIn = true // DEV ONLY CHANGE

  events: string[] = [];
  sidenavOpened: boolean = false;

  selectedTabKey: string = 'home'
  selectedPath: string = '/home'
  tabsArray = Array.from(tabs.values())

  constructor(private route: ActivatedRoute, private authService: AuthService, private router: Router) {

  }

  ngOnInit(): void {
    this.authService.isLoggedIn$.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
    });

  }

  onChangeTab(tabKey: any) {
    this.selectedTabKey = tabKey
    this.selectedPath = tabs.get(this.selectedTabKey)?.redirectPath
    this.router.navigate([this.selectedPath]);
  }

}
