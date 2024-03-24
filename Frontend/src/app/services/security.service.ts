import { Injectable } from '@angular/core';
import CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root'
})
export class SecurityService {

  secretKey: string = "abcdefghijklmnop"
  constructor() { }
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


  encryptPassword(password: string): string {
    // Convert secret key to WordArray
    let key = CryptoJS.enc.Utf8.parse(this.secretKey);

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
}
