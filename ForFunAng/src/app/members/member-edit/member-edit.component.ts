import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editform: NgForm;
  user: User;
  photoUrl: string;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotificationevent($event: any) {
    if (this.editform.dirty) {
$event.returnValue = true;

    }
  }

  constructor(private route: ActivatedRoute, private alertify: AlertifyService, private userservice: UserService,
    private authservise: AuthService) { }

  ngOnInit() {

    this.route.data.subscribe(data => {
     this.user = data['user'];

    });
    this.authservise.currentPhotoUrl.subscribe(photourl => this.photoUrl = photourl);
  }
  updateUser() {

 this.userservice.updateUser(this.authservise.decodedToken.nameid, this.user).subscribe(next => {

  this.alertify.success('profile updated');
  this.editform.reset(this.user);  // method for reset with new updated data

 }, error => {
   this.alertify.error('eroora');
 });

  }
updateMainPhoto(photoUrl) {

  this.user.photoURL = photoUrl;
}
}
