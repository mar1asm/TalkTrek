import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { UserModel } from '../../models/user-model';
import { Observer } from 'rxjs';

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrl: './account-details.component.scss'
})
export class AccountDetailsComponent implements OnInit {

  user: UserModel = {
    id: '',
    firstName: '',
    lastName: '',
    email: '',
    userType: '',
    registrationDate: '',
    profilePhoto: null,
    country: ''
  }

  isProfileComplete: Boolean = false;

  constructor(private accountService: AccountService) {

  }
  ngOnInit(): void {
    const observer: Observer<UserModel> = {
      next: (user: UserModel) => {
        this.user = user;
        this.isProfileComplete = !!user.firstName;
      },
      error: (error: any) => {
        console.error('Error fetching user details:', error);
      },
      complete: () => {
        // Handle completion if needed
      }
    };

    this.accountService.getDetails().subscribe(observer);
  }



  onSubmit() {
    this.accountService.updateProfile(this.user).then(success => {
      if (success) {
        console.log('Profile updated successfully');
        this.isProfileComplete = true;
        // Handle success, such as showing a success message to the user
      } else {
        console.error('Failed to update profile');
        // Handle failure, such as showing an error message to the user
      }
    });
  }


  async onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = async () => {
        this.user.profilePhoto = reader.result;
        await this.accountService.updateProfilePhoto(file);
      };
    }
  }

  async deletePhoto() {
    this.user.profilePhoto = ''
    await this.accountService.deleteProfilePhoto();
    const profilePhotoInput = document.getElementById('profile-photo') as HTMLInputElement;
    if (profilePhotoInput) {
      profilePhotoInput.value = '';
    }
  }

  isBase64String(value: any): boolean {
    return typeof value === 'string' && value.startsWith('data:image');
  }
}
