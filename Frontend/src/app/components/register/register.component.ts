import { Component, OnInit } from '@angular/core';
import { RegisterModel } from '../../models/register-model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {

  constructor(private authService: AuthService) {

  }

  registerModel: RegisterModel = {} as RegisterModel;
  userTypes = ['Student', 'Chutor'];
  ngOnInit(): void {

  }




  submitted = false;

  onSubmit() {
    this.submitted = true;
    console.log('OKKKKKK')
    this.authService.register(this.registerModel);
  }
}
