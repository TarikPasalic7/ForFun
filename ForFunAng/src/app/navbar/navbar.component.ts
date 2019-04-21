import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  model: any = {};
  constructor(public authservice: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }
  login() {

    this.authservice.login(this.model).subscribe(next => {

      this.alertify.success('Logged in sucessfully'); // using alertify methods from our created alertifyservice
    }, error => {
      this.alertify.error('fails to login');
    });
  }
loggedin() {
return this.authservice.loggedin();
}
logout() {
localStorage.removeItem('token');
this.alertify.message ('logged out');

}
}
