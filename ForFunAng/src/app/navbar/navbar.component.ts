import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  model: any = {};
  photoUrl: string;
  constructor(public authservice: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.authservice.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }
  login() {

    this.authservice.login(this.model).subscribe(next => {

      this.alertify.success('Logged in sucessfully'); // using alertify methods from our created alertifyservice
    }, error => {
      this.alertify.error('fails to login');
    }, () => {
this.router.navigate(['/members']);

    });
  }
loggedin() {
return this.authservice.loggedin();
}
logout() {
localStorage.removeItem('token');
localStorage.removeItem('user');
this.authservice.decodedToken = null;
this.authservice.currentUser = null;
this.alertify.message ('logged out');
this.router.navigate(['/home']); // Navigates back to /home route

}
}
