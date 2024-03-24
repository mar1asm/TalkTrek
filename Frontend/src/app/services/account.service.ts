import { Injectable } from '@angular/core';
import { UserModel } from '../models/user-model';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable, switchMap, catchError, throwError, map, firstValueFrom } from 'rxjs';
import { ChangePasswordModel } from '../models/change-password-model';
import { SecurityService } from './security.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private apiRoot = 'https://localhost:7048';
  private apiUrl = '';
  constructor(private http: HttpClient, private securityService: SecurityService, private authService: AuthService) { }




  getDetails(): Observable<UserModel> {
    this.apiUrl = `${this.apiRoot}/account/details`
    return this.authService.getToken().pipe(
      switchMap(token => {
        token ? token : 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1YTU1MWY5MC04Njg5LTQwZjQtYWY4Mi01YmQ0ZGU5MzBmMmIiLCJqdGkiOiJjMmIzZmEzYi0xZjMzLTQzNzEtYWM2Mi05YWIxODRjNDdmMjEiLCJpYXQiOjYzODQ1MTcyMDEyMjQzNzA3OSwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc3lzdGVtIjoiIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiU3R1ZGVudCIsImV4cCI6MTcwOTY2MTYxMiwiaXNzIjoiVGFsa1RyZWsgQVBJIiwiYXVkIjoiVGFsa1RyZWsgQVBJIn0.OOtFpxj_m6qMYxC7-Jo83R2Jym2_BiQA6AnCE-j0KQg'
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return this.http.get<any>(this.apiUrl, { headers }).pipe(
          map(response => response.user), // Extract 'user' property from response
          catchError((error: HttpErrorResponse) => {
            if (error.error instanceof ErrorEvent) {
              // Client-side error occurred
              console.error('An error occurred:', error.error.message);
            } else if (error.error && typeof error.error === 'object') {
              // Server-side error occurred with JSON response
              console.error(
                `Backend returned code ${error.status}, ` +
                `body was: ${JSON.stringify(error.error)}`);
            } else {
              // Server-side error occurred with non-JSON response
              console.error(
                `Backend returned code ${error.status}, ` +
                `body was: ${error.error}`);
            }
            // Return an observable with a user-friendly error message
            return throwError('Something bad happened; please try again later.');
          })
        );
      })
    );
  }


  async updateProfile(user: UserModel) {
    this.apiUrl = `${this.apiRoot}/account/profile`
    try {
      const token = await firstValueFrom(this.authService.getToken());
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      await firstValueFrom(this.http.put(this.apiUrl, user, { headers }))
      return true;
    } catch (error) {
      console.error('Error updating profile:', error);
      return false; // Return false in case of any error
    }
  }

  async updateProfilePhoto(photoFile: File) {
    this.apiUrl = `${this.apiRoot}/account/photo`;
    try {
      const token = await firstValueFrom(this.authService.getToken());
      const formData = new FormData();
      formData.append('photo', photoFile);
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      await firstValueFrom(this.http.put(this.apiUrl, formData, { headers }));
      return true;
    } catch (error) {
      console.error('Error updating profile photo:', error);
      return false; // Return false in case of any error
    }
  }

  async deleteProfilePhoto() {
    this.apiUrl = `${this.apiRoot}/account/photo`;
    try {
      const token = await firstValueFrom(this.authService.getToken());
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      await firstValueFrom(this.http.delete(this.apiUrl, { headers }));
      return true;
    } catch (error) {
      console.error('Error deleting profile photo:', error);
      return false; // Return false in case of any error
    }
  }

  async changePassword(changePasswordModel: ChangePasswordModel) {
    this.apiUrl = `${this.apiRoot}/change-password`;
    const encryptedOldPassword = await this.securityService.encryptPassword(changePasswordModel.oldPassword);
    const encryptedNewPassword = await this.securityService.encryptPassword(changePasswordModel.newPassword);
    const requestBody = {
      oldPassword: encryptedOldPassword,
      newPassword: encryptedNewPassword
    };
    try {
      const token = await firstValueFrom(this.authService.getToken());
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      await firstValueFrom(this.http.put(this.apiUrl, requestBody, { headers }));
      return true;
    } catch (error) {
      console.error('Error changing password:', error);
      return false; // Return false in case of any error
    }
  }

}
