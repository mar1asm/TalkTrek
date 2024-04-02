export interface TutorModel {
    id: string,
    firstName: string,
    lastName: string,
    photo: string,
    education: string,
    description: string,
    country: string,
    price: number,
    teachedLanguage: string,
    teachingCategories: string[],
    additionalLanguages: LanguageModel[]
    accountCreationDate: Date,
    availabilityIntervals: IntervalModel[],
    rating: number,
    reviews: number
}


export interface LanguageModel {
    name: string,
    level: number
}

export interface IntervalModel {
    min: number,
    max: number
}