import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiRoot = 'https://localhost:7048';
  private apiUrl = '';
  constructor(private http: HttpClient) { }

  async getUserCount(role: string): Promise<number> {
    this.apiUrl = `${this.apiRoot}/users/count/${role}`
    console.log(this.apiUrl)
    try {
      const count = await firstValueFrom(this.http.get<number>(this.apiUrl));
      return count;
    } catch (error) {
      console.error('Error fetching user count:', error);
      throw error; // Re-throw the error so the caller can handle it
    }
  }

  async getTutor(id: string) { }
}
