export enum UserType {
    Student,
    Tutor
}

export interface UserModel {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    userType: string;
    registrationDate: string;
    profilePhoto: any
}

export interface StudentModel extends UserModel {
    grade: number
}

export interface TutorModel extends UserModel {
    description: string,
    price: number,

}