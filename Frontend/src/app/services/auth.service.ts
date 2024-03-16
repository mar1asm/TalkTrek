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

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiRoot = 'https://localhost:7048';
  private apiUrl = '';
  private isLoggedInSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  isLoggedIn$ = this.isLoggedInSubject.asObservable();
  constructor(private http: HttpClient, private store: Store<IAuthState>) { }

  /*   encryptWithNonce(data: string, nonce: string): string {
      // Convert nonce to WordArray
      const nonceWordArray = CryptoJS.enc.Utf8.parse(nonce);
  
      // Generate a random initialization vector (IV)
      const iv = CryptoJS.lib.WordArray.random(16); // 16 bytes (128 bits)
  
      // Encrypt data using AES encryption with CBC mode
      const encryptedData = CryptoJS.AES.encrypt(data, nonceWordArray, {
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
      });
  
      // Combine IV and encrypted data into a single string
      const combinedData = CryptoJS.enc.Base64.stringify(iv.concat(encryptedData.ciphertext));
  
      // Return the combined data as a Base64-encoded string
      return combinedData;
    } */


  encryptPassword(password: string, secretKey: string): string {
    // Convert secret key to WordArray
    let key = CryptoJS.enc.Utf8.parse(secretKey);

    // Generate a random IV
    let iv = CryptoJS.lib.WordArray.random(16); // 16 bytes (128 bits)

    // Encrypt data using AES encryption with CBC mode and PKCS7 padding
    let encryptedBytes = CryptoJS.AES.encrypt(password, key, {
      iv: iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7
    });

    // Combine IV and encrypted data into a single string
    let encryptedPassword = CryptoJS.enc.Base64.stringify(iv.concat(encryptedBytes.ciphertext));

    return encryptedPassword;
  }

  async register(registerModel: RegisterModel) {
    const encryptedPassword = await this.encryptPassword(registerModel.password, "abcdefghijklmnop");
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
    const encryptedPassword = await this.encryptPassword(loginModel.password, "abcdefghijklmnop");

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
