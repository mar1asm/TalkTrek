import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterModel } from '../models/register-model';
import { BehaviorSubject, Observable, catchError, firstValueFrom, tap, throwError } from 'rxjs';
import CryptoJS from 'crypto-js';
import { LoginModel } from '../models/login-model';
import { IAuthState } from '../../store/state/auth.state';
import { Store, select } from '@ngrx/store';
import * as AuthActions from './../../store/actions/auth.actions';
import * as authSelectors from './../../store/selectors/auth.selectors'
import { response } from 'express';
import { SecurityService } from './security.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiRoot = 'https://localhost:7048';
  private apiUrl = '';
  private isLoggedInSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);// DEV ONLY CHANGE
  isLoggedIn$ = this.isLoggedInSubject.asObservable();
  constructor(private http: HttpClient, private securityService: SecurityService, private store: Store<IAuthState>) { }



  async register(registerModel: RegisterModel) {
    const encryptedPassword = await this.securityService.encryptPassword(registerModel.password);
    console.log(encryptedPassword)

    const requestBody = {
      email: registerModel.email,
      password: encryptedPassword,
      userType: registerModel.userType
    };

    this.apiUrl = `${this.apiRoot}/register`
    console.log(this.apiUrl)
    console.log(requestBody)
    // Make HTTP POST request to register endpoint with the request body

    this.http.post<any>(this.apiUrl, requestBody)
      .pipe(
        tap({
          next: response => console.log('Registration successful:', response),
          error: error => console.error('Registration failed:', error)
        }),
        catchError(error => {
          console.error('Registration failed:', error);
          return throwError(() => error);
        })
      )
      .subscribe({
        next: () => console.log('HTTP request succeeded'),
        error: error => console.error('HTTP request failed:', error)
      });


  }



  async login(loginModel: LoginModel): Promise<any> {
    const encryptedPassword = await this.securityService.encryptPassword(loginModel.password);

    const requestBody = {
      email: loginModel.email,
      password: encryptedPassword
    };

    this.apiUrl = `${this.apiRoot}/login`
    // Make HTTP POST request to register endpoint with the request body

    return new Promise((resolve, reject) => {
      this.http.post<any>(this.apiUrl, requestBody)
        .subscribe({
          next: response => {
            console.log('Login successful:', response);
            this.isLoggedInSubject.next(true);
            resolve(response); // Resolve the promise with the response data
          },
          error: error => {
            console.error('Login failed:', error);
            reject(error); // Reject the promise with the error
          }
        });
    });
  } catch(error: any) {
    console.error('An error occurred during login:', error);
    throw error; // Rethrow the error
  }

  setToken(token: string): void {
    this.store.dispatch(AuthActions.setToken({ token }));
  }

  clearToken(): void {
    this.store.dispatch(AuthActions.clearToken());
  }



  getToken(): Observable<string | null> {
    return this.store.pipe(
      select(authSelectors.getToken)
    );
  }


  async confirmEmail(userId: string, confirmationCode: string): Promise<any> {
    this.apiUrl = `${this.apiRoot}/confirm-email`

    const requestBody = {
      userId: userId,
      confirmationCode: confirmationCode
    };

    return new Promise((resolve, reject) => {
      this.http.post<any>(this.apiUrl, requestBody)
        .subscribe({
          next: response => {
            console.log("ok", response)
            resolve(response);
          },
          error: error => {
            console.error('fail', error)
            reject(error);
          }
        });
    });
  }

  async checkAccountComplete(): Promise<boolean> {
    this.apiUrl = `${this.apiRoot}/account/check`;

    try {
      const token = await firstValueFrom(this.getToken());
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

      return await firstValueFrom(this.http.get<boolean>(this.apiUrl, { headers }));
    } catch (error) {
      console.error('Error checking account completeness:', error);
      return false; // Return false in case of any error
    }
  }

  logout() {
    this.clearToken();
    this.isLoggedInSubject.next(false);
  }
}
