import { Component } from '@angular/core';
import { TutorModel } from '../../models/tutor-model';

@Component({
  selector: 'app-tutor-profile-details',
  templateUrl: './tutor-profile-details.component.html',
  styleUrl: './tutor-profile-details.component.scss'
})
export class TutorProfileDetailsComponent {

  tutor: TutorModel = {
    id: '',
    firstName: '',
    photo: '',
    education: '',
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
    lastName: ''
  }


  onSubmit() {
    throw new Error('Method not implemented.');
  }

}
