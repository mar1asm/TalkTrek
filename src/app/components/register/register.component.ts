import { Component, OnInit } from '@angular/core';
import { RegisterModel } from '../../models/register-model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {

  registerModel: RegisterModel = {} as RegisterModel;
  userTypes = ['Student', 'Chutor'];
  ngOnInit(): void {

  }




  submitted = false;

  onSubmit(registerModel: any) { this.submitted = true; }
}
