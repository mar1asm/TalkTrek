import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {

  tutorCount: number = 0
  studentCount: number = 0
  classCount: number = 0
  languagesCount: number = 0

  constructor(private userService: UserService) {

  }
  ngOnInit() {
    this.initializeCounts();
  }

  async initializeCounts() {
    try {
      this.tutorCount = await this.userService.getUserCount("Tutor");
      this.studentCount = await this.userService.getUserCount("Student");
    } catch (error) {
      // Handle error if needed
      console.error('Error:', error);
    }
  }

}
