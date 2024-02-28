import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss'
})
export class AuthComponent implements OnInit {

  action: string = "login";
  showRegistrationForm: boolean = true;

  constructor(private route: ActivatedRoute) {
  }
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.action = params['action'];
      this.showRegistrationForm = this.action == 'login' ? false : true;
    });
  }


}
