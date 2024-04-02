import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TutorProfileDetailsComponent } from './tutor-profile-details.component';

describe('TutorProfileDetailsComponent', () => {
  let component: TutorProfileDetailsComponent;
  let fixture: ComponentFixture<TutorProfileDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TutorProfileDetailsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TutorProfileDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
