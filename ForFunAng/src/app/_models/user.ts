import { Photo } from './photo';

export interface User {

    Id: number;
    Username: string;
    Gender: string;
    age: number;
    Created: Date;
    lastactive: Date;
    City: string;
    Country: string;
    photoURL: string;
    Photos?: Photo[];
}

