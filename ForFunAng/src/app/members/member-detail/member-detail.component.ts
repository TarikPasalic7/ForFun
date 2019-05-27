import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryActionComponent, NgxGalleryAnimation } from 'ngx-gallery';
import { TabsetComponent } from 'ngx-bootstrap';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
@ViewChild('memberTabs') memberTabs: TabsetComponent;
  user: User;
galleryoptions: NgxGalleryOptions[];
galleryimages: NgxGalleryImage[];
  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });

    this.route.queryParams.subscribe( params => {
       const selectedTab = params['tab'];
       this.memberTabs.tabs[selectedTab > 0 ? selectedTab : 0].active = true;
          
    });
    this.galleryoptions = [
    {
       width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false


    }

    ];

    this.galleryimages = this.getImges();

  }
 /*  loaduser() {
this.userService.getuser(+this.route.snapshot.params['id']).subscribe((user: User) => {
this.user = user;

}, error => {

this.alertify.error('error');
});


  }
  */

 getImges() {
  const imageurl = [];
   for(let i = 0; i < this.user.photos.length;i++)
   {
    imageurl.push({
    small: this.user.photos[i].url,
    medium: this.user.photos[i].url,
    large: this.user.photos[i].url


    });

   }
   return imageurl;
 }

 selectTab(tabId: number) {

  this.memberTabs.tabs[tabId].active = true;
 }

}
