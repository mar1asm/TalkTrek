import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.scss'
})
export class ConfirmEmailComponent implements OnInit {

  userId: string = ''
  confirmationCode: string = ''
  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) {

  }
  ngOnInit(): void {
    console.log("OK")
    this.route.queryParams.subscribe(params => {
      this.userId = params['userId'];
      this.confirmationCode = params['confirmationCode']
    });
    this.submitCode()
  }

  async submitCode() {
    try {
      const result = await this.authService.confirmEmail(this.userId, this.confirmationCode)
      console.log('Login successful:', result);
      this.router.navigateByUrl('/auth?action=login&confirmedEmail=true');
    } catch (error) {
      // Handle error
    }
  }

}
