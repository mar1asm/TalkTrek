import { AuthService } from './../../services/auth.service';
import { Component } from '@angular/core';
import { LoginModel } from '../../models/login-model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  constructor(private authService: AuthService,
    private router: Router, private route: ActivatedRoute) {
  }

  loginModel: LoginModel = {} as LoginModel;
  emailConfirmed: Boolean = false;
  loginFailed: Boolean = false;
  submitted = false;



  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.emailConfirmed = params['confirmedEmail'] ?? false;
    });
  }


  async onSubmit() {
    this.submitted = true;
    try {
      const result = await this.authService.login(this.loginModel);
      console.log('Login successful');
      // Do something with the result

      if (result.token) {
        this.authService.setToken(result.token);

        // Subscribe to getToken() after setting the token
        this.authService.getToken().subscribe(token => {
        });
        const isAccountComplete = await this.authService.checkAccountComplete();

        if (isAccountComplete) {
          this.router.navigate(['home']);
        } else {
          this.router.navigate(['account'], { queryParams: { tab: 'account-details' } });
        }
      } else {
        console.error('Login failed: Token is undefined');
        this.loginFailed = true;
      }


    } catch (error) {
      console.error('Login failed:', error);
      this.loginFailed = true;
      // Handle error
    }
  }
}
