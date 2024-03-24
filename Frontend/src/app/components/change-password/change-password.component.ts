import { AuthService } from './../../services/auth.service';
import { Component } from '@angular/core';
import { ChangePasswordModel } from '../../models/change-password-model';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss'
})
export class ChangePasswordComponent {

  changePasswordModel: ChangePasswordModel = {
    oldPassword: '',
    newPassword: '',
    confirmNewPassword: ''
  }

  changePasswordFailed: Boolean = false;
  submitted = false;

  constructor(private accountService: AccountService, private AuthService: AuthService) {

  }

  async onSubmit() {
    this.submitted = true;
    try {
      const result = await this.accountService.changePassword(this.changePasswordModel);
      this.AuthService.logout();

      console.log(result)
    } catch (error) {
      console.error('Change password failed:', error);
      this.changePasswordFailed = true;
      // Handle error
    }
  }
}
