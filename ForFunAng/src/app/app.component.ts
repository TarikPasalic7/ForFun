import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { listLazyRoutes } from '@angular/compiler/src/aot/lazy_routes';
import {JwtHelperService} from '@auth0/angular-jwt';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
JWTHelper = new JwtHelperService;
 constructor(private authservice: AuthService) {}

  ngOnInit() {
const token = localStorage.getItem('token');
const user: User = JSON.parse(localStorage.getItem('user'));
if (token) {
  this.authservice.decodedToken = this.JWTHelper.decodeToken(token);
}
if (user) {
  this.authservice.currentUser = user;
  this.authservice.changeMemberphoto(user.photoURL);
}

  }
}
