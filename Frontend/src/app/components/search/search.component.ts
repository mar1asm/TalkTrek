
import { Component, ElementRef, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { IntervalModel, TutorModel } from '../../models/tutor-model';
import { TutorService } from '../../services/tutor.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrl: './search.component.scss'
})
export class SearchComponent implements OnInit {


  initSearch: boolean = true;
  isFiltersPaneOpened: boolean = true;


  minimumRatings = [0, 3.5, 4, 4.5, 5]
  selectedMinimumRating: any;

  availabilityIntervals = ["12am-4am", "4am-8am", "8am-12pm", "12pm-4pm", "4pm-8pm", "8pm-12am"]
  selectedIntervals = this.availabilityIntervals;

  minimumExperience = [1, 3, 5]
  selectedMinimumExperience = ''


  @ViewChildren('descriptionRef')
  descriptionRefs!: QueryList<ElementRef>;


  isDescriptionOverflow: Map<string, boolean> = new Map<string, boolean>();
  isDescriptionExpanded: Map<string, boolean> = new Map<string, boolean>();

  startDateCalendar = new Date()
  endDateCalendar = new Date(new Date().setDate(this.startDateCalendar.getDate() + 30));

  tutors: TutorModel[] = [
    {
      id: this.generateRandomId(10),
      firstName: "Montana L.",
      lastName: "Landers",
      photo: "../../../assets/profile-pic.jpg",
      education: "Some school or cert",
      price: 25,
      country: 'UK',
      teachedLanguage: "English",
      teachingCategories: ["Business", "Conversational", "Language exam"],
      description: "Nice chuchor.",
      additionalLanguages: [
        { name: "Romanian", level: 1 },
        { name: "Spanish", level: 1 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.99,
      reviews: 25
    },
    {
      id: this.generateRandomId(10),
      firstName: "Ioana",
      lastName: "Alucu",
      photo: "../../../assets/jana.jpg",
      education: "Some school or cert",
      price: 30,
      country: 'Romania',
      teachedLanguage: "Romanian",
      teachingCategories: ["Conversational"],
      description: "Passionate about languages: English, French, Spanish, when drunk also others.",
      additionalLanguages: [
        { name: "French", level: 4 },
        { name: "English", level: 2 },
        { name: "Romanian", level: 2 },
        { name: "Spanish", level: 2 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.5,
      reviews: 25
    },
    {
      id: this.generateRandomId(10),
      firstName: "Toni",
      lastName: "S",
      photo: "../../../assets/toni.jpg",
      education: "Some school or cert",
      price: 50,
      country: 'Crucea',
      teachedLanguage: "Romanian",
      teachingCategories: ["Business", "Language exam"],
      description: "Chiar si limba, japoneza",
      additionalLanguages: [
        { name: "English", level: 2 },
        { name: "Romanian", level: 2 },
        { name: "Japanese", level: 2 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.01,
      reviews: 21
    },
    {
      id: this.generateRandomId(10),
      firstName: "John Doe", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 40,
      country: 'USA',
      teachedLanguage: "English",
      teachingCategories: ["Business", "Conversational"],
      description: "Experienced English tutor with a passion for teaching.lohh fdsjkagfk fdasjkfgudsa dfiusafiuds fdsa uifyiu dsaf dsiufyiusd fl adoiufaso dufd ufdsaufodisayfdgs fdhgflkasdgh iufdysafyuisda yfiusady fuidsayf uiasdy;oaiudoiufos fdsaj fashdiuf  dusafhyui aiusdfoisd foisdaf iuasodofys dajkndhfklj ahsdufipisdy8f dsajsduihf o fkdsjahl f daisfi oasdiofio adfsoiujfdsajh dssiodsfsd aiofds  oi dfsjfoiahsuidfh  d;oifuoiasdhf oisdaufo9saduf oii",
      additionalLanguages: [
        { name: "French", level: 3 },
        { name: "German", level: 2 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.8,
      reviews: 30
    },
    {
      id: this.generateRandomId(10),
      firstName: "Emma Smith", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 35,
      country: 'Canada',
      teachedLanguage: "French",
      teachingCategories: ["Conversational"],
      description: "Dynamic French tutor with a focus on spoken communication.",
      additionalLanguages: [
        { name: "English", level: 4 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.6,
      reviews: 28
    },
    {
      id: this.generateRandomId(10),
      firstName: "Sophia Garcia", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 45,
      country: 'Spain',
      teachedLanguage: "Spanish",
      teachingCategories: ["Business", "Language exam"],
      description: "Experienced Spanish tutor offering tailored lessons.",
      additionalLanguages: [
        { name: "English", level: 3 },
        { name: "French", level: 2 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.7,
      reviews: 32
    },
    {
      id: this.generateRandomId(10),
      firstName: "Elena Petrova", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 35,
      country: 'Russia',
      teachedLanguage: "Russian",
      teachingCategories: ["Conversational"],
      description: "Passionate Russian tutor dedicated to making learning enjoyable.",
      additionalLanguages: [
        { name: "English", level: 2 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.4,
      reviews: 26
    },
    {
      id: this.generateRandomId(10),
      firstName: "Antonio Silva", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 55,
      country: 'Portugal',
      teachedLanguage: "Portuguese",
      teachingCategories: ["Business", "Language exam"],
      description: "Dynamic Portuguese tutor with a focus on practical communication.",
      additionalLanguages: [
        { name: "English", level: 3 },
        { name: "Spanish", level: 2 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.9,
      reviews: 35
    },
    {
      id: this.generateRandomId(10),
      firstName: "Luisa Fernandez", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 40,
      country: 'Mexico',
      teachedLanguage: "Spanish",
      teachingCategories: ["Conversational"],
      description: "Passionate Spanish tutor with a focus on cultural immersion.",
      additionalLanguages: [
        { name: "English", level: 3 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.6,
      reviews: 29
    },
    {
      id: this.generateRandomId(10),
      firstName: "Hiroshi Yamamoto", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 45,
      country: 'Japan',
      teachedLanguage: "Japanese",
      teachingCategories: ["Business", "Language exam"],
      description: "Experienced Japanese tutor offering customized lessons.",
      additionalLanguages: [
        { name: "English", level: 2 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.8,
      reviews: 31
    },
    {
      id: this.generateRandomId(10),
      firstName: "Luisa Santos", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 35,
      country: 'Brazil',
      teachedLanguage: "Portuguese",
      teachingCategories: ["Conversational"],
      description: "Passionate Portuguese tutor focused on conversation and culture.",
      additionalLanguages: [
        { name: "English", level: 2 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.5,
      reviews: 31
    },
    {
      id: this.generateRandomId(10),
      firstName: "Ahmed Ali", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 40,
      country: 'Egypt',
      teachedLanguage: "Arabic",
      teachingCategories: ["Conversational"],
      description: "Friendly Arabic tutor dedicated to helping learners build confidence.",
      additionalLanguages: [
        { name: "English", level: 3 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.7,
      reviews: 28
    },
    {
      id: this.generateRandomId(10),
      firstName: "Anna Petrovna", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 30,
      country: 'Russia',
      teachedLanguage: "Russian",
      teachingCategories: ["Conversational", "Language exam"],
      description: "Experienced Russian tutor specializing in spoken communication.",
      additionalLanguages: [
        { name: "English", level: 2 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.4,
      reviews: 25
    },
    {
      id: this.generateRandomId(10),
      firstName: "Miguel Hernandez", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 50,
      country: 'Mexico',
      teachedLanguage: "Spanish",
      teachingCategories: ["Business"],
      description: "Dynamic Spanish tutor with a focus on professional communication.",
      additionalLanguages: [
        { name: "English", level: 3 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.8,
      reviews: 30
    },
    {
      id: this.generateRandomId(10),
      firstName: "Li Wei", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 45,
      country: 'China',
      teachedLanguage: "Mandarin",
      teachingCategories: ["Conversational", "Language exam"],
      description: "Passionate Mandarin tutor helping students achieve fluency.",
      additionalLanguages: [
        { name: "English", level: 3 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.6,
      reviews: 27
    },
    {
      id: this.generateRandomId(10),
      firstName: "Sofia Lopez", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 40,
      country: 'Spain',
      teachedLanguage: "Spanish",
      teachingCategories: ["Conversational", "Business"],
      description: "Experienced Spanish tutor offering tailored lessons.",
      additionalLanguages: [
        { name: "English", level: 3 },
        { name: "French", level: 2 },
        { name: "German", level: 2 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.7,
      reviews: 32
    },
    {
      id: this.generateRandomId(10),
      firstName: "Hiroshi Yamamoto", lastName: 'test',
      photo: "../../../assets/missing-profile.jpg",
      education: "Some school or cert",
      price: 45,
      country: 'Japan',
      teachedLanguage: "Japanese",
      teachingCategories: ["Language exam", "Business"],
      description: "Dynamic Japanese tutor with a focus on practical communication.",
      additionalLanguages: [
        { name: "English", level: 3 },
        { name: "Spanish", level: 2 },
        { name: "French", level: 2 }
      ],
      accountCreationDate: new Date(2024, 1, 23),
      availabilityIntervals: [
        { min: 0, max: 4 }
      ],
      rating: 4.9,
      reviews: 35
    }





  ];


  constructor(private tutorService: TutorService, private router: Router) {

  }


  availableLanguages: string[] = []
  availableCategories: string[] = []
  availableOtherLanguages: string[] = []
  availablePriceRange: IntervalModel = { min: -1, max: -1 }

  filteredTutors: any[] = [];

  selectedFilters = {
    language: '', // Holds the selected language filter
    otherLanguages: [] as string[],
    priceRange: { min: 0, max: 0 } as IntervalModel,
    minRating: 0 as number,
    teachingCategory: 'Any' as string,
    availability: 'Any' as string,
    minimumExperience: 0 as number
  };


  ngOnInit(): void {

    this.checkDescriptionOverflow();

    //initial values
    this.filteredTutors = this.tutors;
    this.availableLanguages = this.getAvailableLanguages();
    this.availableOtherLanguages = this.getAvailableOtherLanguages();
    this.availablePriceRange = this.getAvailablePriceRange();
    this.selectedFilters.priceRange = this.availablePriceRange;
    this.availableCategories = this.getAvailableCategories();
  }

  ngAfterViewInit() {
    // Check overflow for each description element after view initialization
    this.checkDescriptionOverflow();
  }

  onToggleFilterPane() {
    this.isFiltersPaneOpened = !this.isFiltersPaneOpened;
  }

  onToggleDescription(id: string) {
    this.isDescriptionExpanded.set(id, !this.isDescriptionExpanded.get(id));
  }

  filterOutMainLanguage(languages: string[]): string[] {
    return languages.filter((language) => language !== this.selectedFilters.language)
  }

  checkDescriptionOverflow() {
    if (this.descriptionRefs) {
      this.descriptionRefs.forEach((ref) => {

        const descriptionElement = ref.nativeElement;
        const tutorId = descriptionElement.getAttribute('id').replace('description-', '');
        // Temporarily remove overflow-y: hidden to measure overflow
        const containerStyle = descriptionElement.parentElement.style.overflowY;
        descriptionElement.parentElement.style.overflowY = 'visible';

        // Check if description content overflows its container
        const overflow = descriptionElement.scrollHeight > descriptionElement.clientHeight * 1.02;

        // Restore the original overflow-y style
        descriptionElement.parentElement.style.overflowY = containerStyle;

        // Set the overflow value for the corresponding index/key
        this.isDescriptionOverflow.set(tutorId, overflow);
      });
    }
  }


  getAvailablePriceRange(): IntervalModel {
    const minPrice = Math.min(...this.filteredTutors.map(tutor => tutor.price));
    const maxPrice = Math.max(...this.filteredTutors.map(tutor => tutor.price));

    return { min: minPrice, max: maxPrice };

  }

  // get list of all languages available for teaching
  getAvailableLanguages(): string[] {
    // Extract unique primary languages
    const uniqueLanguages = new Set<string>();
    this.tutors.forEach(tutor => {
      uniqueLanguages.add(tutor.teachedLanguage);
    });
    return Array.from(uniqueLanguages);
  }

  //get list of all other languages tutors speak
  getAvailableOtherLanguages(): string[] {
    // Extract unique primary languages
    const uniqueLanguages = new Set<string>();
    this.filteredTutors.forEach(tutor => {
      tutor.additionalLanguages.forEach((language: { name: string; }) => {
        uniqueLanguages.add(language.name);
      });
    });
    return Array.from(uniqueLanguages);
  }

  getAvailableCategories(): string[] {
    const uniqueCategories = new Set<string>();
    uniqueCategories.add("Any")
    this.filteredTutors.forEach(tutor => {
      tutor.teachingCategories.forEach((category: string) => {
        uniqueCategories.add(category);
      });
    });
    return Array.from(uniqueCategories);
  }

  filterTutors() {
    this.filteredTutors = this.tutors;

    if (this.selectedFilters.language) {
      this.filteredTutors = this.filteredTutors.filter((tutor: TutorModel) => tutor.teachedLanguage === this.selectedFilters.language);
      this.availableOtherLanguages = this.getAvailableOtherLanguages();
    }

    if (this.selectedFilters.otherLanguages.length > 0) {
      this.filteredTutors = this.filteredTutors.filter((tutor: TutorModel) =>
        tutor.additionalLanguages.some((language: { name: string; }) => {
          return this.selectedFilters.otherLanguages.includes(language.name);
        })
      );
    }

    if (this.selectedFilters.minRating) {
      this.filteredTutors = this.filteredTutors.filter((tutor: TutorModel) =>
        tutor.rating >= this.selectedFilters.minRating
      )
    }

    if (this.selectedFilters.teachingCategory && this.selectedFilters.teachingCategory !== "Any") {
      this.filteredTutors = this.filteredTutors.filter((tutor: TutorModel) =>
        tutor.teachingCategories.includes(this.selectedFilters.teachingCategory))
    }


    //if price slider was moved
    if (this.selectedFilters.priceRange.min != this.availablePriceRange.min || this.selectedFilters.priceRange.max != this.availablePriceRange.max) {
      this.filteredTutors = this.filteredTutors.filter((tutor: TutorModel) =>
        tutor.price >= this.selectedFilters.priceRange.min && tutor.price <= this.selectedFilters.priceRange.max
      )
    }
    else {
      this.availablePriceRange = this.getAvailablePriceRange();
      this.selectedFilters.priceRange.min = this.availablePriceRange.min
      this.selectedFilters.priceRange.max = this.availablePriceRange.max
    }
  }

  generateRandomId(length: number): string {
    let randomId = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    const charactersLength = characters.length;
    for (let i = 0; i < length; i++) {
      randomId += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return randomId;
  }

  onTutorProfileButtonClicked(index: number) {
    this.tutorService.setTutor(this.filteredTutors[index])
    this.router.navigate(['/tutor', this.filteredTutors[index].id]);
  }
}
