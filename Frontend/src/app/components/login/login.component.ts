import { Component } from '@angular/core';
import { LoginModel } from '../../models/login-model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  loginModel: LoginModel = {} as LoginModel;
  ngOnInit(): void {

  }

  submitted = false;

  onSubmit(loginModel: any) { this.submitted = true; }
}
