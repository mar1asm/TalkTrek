import { Component, ElementRef, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrl: './search.component.scss'
})
export class SearchComponent implements OnInit {



  initSearch: boolean = true;
  isFiltersPaneOpened: boolean = true;

  minPossiblePrice: number = 0
  maxPossiblePrice: number = 200

  minSelectedPrice: number = 0
  maxSelectedPrice: number = 200

  languages = ["English", "Spanish", "Romanian", "Chinese", "Italian"]
  selectedLanguage = ""

  languageCategories = ["Conversational", "Business", "Language Exam"]
  selectedlanguageCategory = ""

  otherLanguages = ["English", "Spanish", "Romanian", "Chinese", "Italian"]
  selectedOtherLanguages = []

  minimumRatings = [3.5, 4, 4.5, 5]
  selectedMinimumRating: any;

  availabilityIntervals = ["12am-4am", "4am-8am", "8am-12pm", "12pm-4pm", "4pm-8pm", "8pm-12am"]
  selectedIntervals = this.availabilityIntervals;

  minimumExperience = [1, 3, 5]
  selectedMinimumExperience = ''


  @ViewChildren('descriptionRef')
  descriptionRefs!: QueryList<ElementRef>;

  isDescriptionOverflow: boolean[] = [];
  isDescriptionExpanded: boolean[] = [];

  descriptions: string[] = []

  startDateCalendar = new Date()
  endDateCalendar = new Date(new Date().setDate(this.startDateCalendar.getDate() + 30));



  ngOnInit(): void {
    this.isDescriptionExpanded = new Array(10).fill(false);
    this.descriptions = new Array(10).fill('a')
    for (let index = 0; index < 10; index++) {
      this.descriptions[index] = this.getRandomText()
    }
  }

  ngAfterViewInit() {
    // Check overflow for each description element after view initialization
    this.checkDescriptionOverflow();
  }

  onToggleFilterPane() {
    this.isFiltersPaneOpened = !this.isFiltersPaneOpened;
  }

  onToggleDescription(index: number) {
    this.isDescriptionExpanded[index] = !this.isDescriptionExpanded[index];
  }

  filterMainLanguage(languages: string[]): string[] {
    return languages.filter((language) => language !== this.selectedLanguage)
  }

  checkDescriptionOverflow() {
    if (this.descriptionRefs) {
      // Iterate over each description element
      this.isDescriptionOverflow = this.descriptionRefs.map(ref => {
        const descriptionElement = ref.nativeElement;
        // Temporarily remove overflow-y: hidden to measure overflow
        const containerStyle = descriptionElement.parentElement.style.overflowY;
        descriptionElement.parentElement.style.overflowY = 'visible';

        // Check if description content overflows its container
        const overflow = descriptionElement.scrollHeight > descriptionElement.clientHeight * 1.02;

        // Restore the original overflow-y style
        descriptionElement.parentElement.style.overflowY = containerStyle;

        return overflow;
      });
    }
  }

  getRandomText(): string {
    const l = Math.random() * (1000 - 20) + 20;
    const a = 'best loafy in the world. can speak english and romanian more than fifty fifty. is a very nice loafy and a best bee.Lorem ipsum dolor sit, amet consectetur adipisicing elit.Itaque, ducimus quia nam repellendu adipisci sapiente tenetur alias similique consequatur repellat dicta provident tempore Whole every miles as tiled at seven or</h2><p>Two exquisite objection delighted deficient yet its contained. Cordial because are account evident its subject but eat. Can properly followed learning prepared you doubtful yet him. Over many our good lady feet ask that. Expenses own moderate day fat trifling stronger sir domestic feelings. Itself at be answer always exeter up do. Though or my plenty uneasy do. Friendship so considered remarkably be to sentiments. Offered mention greater fifteen one promise because nor. Why denoting speaking fat indulged saw dwelling raillery.</p><p>Delightful remarkably mr on announcing themselves entreaties favourable. About to in so terms voice at. Equal an would is found seems of. The particular friendship one sufficient terminated frequently themselves. It more shed went up is roof if loud case. Delay music in lived noise an. Beyond genius really enough passed is up.</p><p>On it differed repeated wandered required in. Then girl neat why yet knew rose spot. Moreover property we he kindness greatest be oh striking laughter. In me he at collecting affronting principles apartments. Has visitor law attacks pretend you calling own excited painted. Contented attending smallness it oh ye u'
    return a.substring(0, l)
  }
}
