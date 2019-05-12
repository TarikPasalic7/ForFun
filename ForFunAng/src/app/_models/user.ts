import { Photo } from './photo';

export interface User {

    id: number;
    username: string;
    gender: string;
    age: number;
    created: Date;
    lastActive: Date;
    city: string;
    country: string;
    photoURL: string;
    photos?: Photo[];
}

