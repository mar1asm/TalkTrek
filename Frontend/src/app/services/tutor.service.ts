import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { TutorModel } from '../models/tutor-model';

@Injectable({
  providedIn: 'root'
})
export class TutorService {
  private tutorSubject: BehaviorSubject<TutorModel | null> = new BehaviorSubject<TutorModel | null>(null);

  constructor() { }

  setTutor(tutor: TutorModel) {
    this.tutorSubject.next(tutor);
  }

  getTutor(): Observable<TutorModel | null> {
    return this.tutorSubject.asObservable();
  }
}
