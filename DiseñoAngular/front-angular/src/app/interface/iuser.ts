export interface IUser {
    id: number;
    email: string;
    password: string;
    isDeleted: boolean;
    registrationDate: Date;
    personId: number;
    namePerson: string;
}