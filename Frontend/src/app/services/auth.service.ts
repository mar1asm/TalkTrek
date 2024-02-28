import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserType } from '../models/user-model';
import { RegisterModel } from '../models/register-model';
import { hash } from 'bcryptjs';
import { catchError, tap, throwError } from 'rxjs';
import CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiRoot = 'https://localhost:7048';
  private apiUrl = '';
  constructor(private http: HttpClient) { }

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

}
