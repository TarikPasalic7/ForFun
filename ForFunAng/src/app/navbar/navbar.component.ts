import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  model: any = {};
  constructor(private authservice: AuthService) { }

  ngOnInit() {
  }
  login() {

    this.authservice.login(this.model).subscribe(next => {

      console.log('Logged in sucessfully');
    }, error => {
      console.log('fails to login');
    });
  }
loggedin() {
const token = localStorage.getItem('token');
return !!token;
}
logout() {
localStorage.removeItem('token');
console.log('logged out');

}
}
