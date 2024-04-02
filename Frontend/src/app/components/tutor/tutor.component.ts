import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TutorModel } from '../../models/tutor-model';
import { TutorService } from '../../services/tutor.service';


@Component({
  selector: 'app-tutor',
  templateUrl: './tutor.component.html',
  styleUrls: ['./tutor.component.scss']
})
export class TutorComponent implements OnInit {

  id: string | null = null;

  startDateCalendar = new Date()
  endDateCalendar = new Date(new Date().setDate(this.startDateCalendar.getDate() + 30));

  availableTimes: string[] = ['10:00 AM', '11:00 AM', '12:00 PM', '1:00 PM', '2:00 PM', '3:00 PM'];

  // Model for booking form
  bookingModel: {
    fullName: string,
    email: string,
    phone: string,
    date: string,
    message: string
  } = {
      fullName: '',
      email: '',
      phone: '',
      date: '',
      message: ''
    };

  tutor: TutorModel = {
    id: '',
    firstName: '',
    lastName: '',
    photo: '',
    description: '',
    country: '',
    price: 0,
    teachedLanguage: '',
    teachingCategories: [],
    additionalLanguages: [],
    accountCreationDate: new Date(),
    availabilityIntervals: [],
    rating: 0,
    reviews: 0,
    education: ''
  }


  constructor(private route: ActivatedRoute, private tutorService: TutorService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.id = params['id'];
    });

    this.tutorService.getTutor().subscribe(tutor => {
      this.tutor = tutor as TutorModel;
    });

  }

  selectTime(time: string) {
    // Logic to handle time selection
  }

  onSubmit() {
    // Logic to handle form submission
    console.log('Form submitted:', this.bookingModel);
  }
}
