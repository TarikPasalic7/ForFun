import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import {map} from 'rxjs/operators';

import {JwtHelperService} from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';



@Injectable({
  providedIn: 'root'
})
export class AuthService {
baseUrl = environment.apiUrl + 'auth/';
JWTHelper = new JwtHelperService;
decodedToken: any;
currentUser: User;
photoUrl = new BehaviorSubject<string>('../../../../assets/16.1 user.png');
currentPhotoUrl = this.photoUrl.asObservable();
constructor(private http: HttpClient) { }
changeMemberphoto(photoUrl: string) {
  this.photoUrl.next(photoUrl);
}
login(model: any) {
 return this.http.post(this.baseUrl + 'login', model)
 .pipe(
map((response: any) => {

  const user = response;
  if (user) {
    localStorage.setItem('token', user.token);
    localStorage.setItem('user', JSON.stringify(user.user));
    this.currentUser = user.user;
    this.decodedToken = this.JWTHelper.decodeToken(user.token);
    this.changeMemberphoto(this.currentUser.photoURL);
  }
})

 );
}
register(user: User ) {

  return this.http.post(this.baseUrl + 'Register', user);

}

loggedin() {
const token = localStorage.getItem('token');
return !this.JWTHelper.isTokenExpired(token);

}
}
