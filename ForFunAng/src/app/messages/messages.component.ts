import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination, PaginatedResult } from '../_models/pagination';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
 messages: Message[];
 pagination: Pagination;
 messageContainer = 'Unread';
  constructor(private userService: UserService, private alertify: AlertifyService, 
    private route: ActivatedRoute, private authservice: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
     this.messages = data['messages'].result;
     this.pagination = data['messages'].pagination;

    });

  }
   loadMessages() {
      this.userService.getMesagges(this.authservice.decodedToken.nameid, this.pagination.currentPage, this.pagination.itemsPerPage
        , this.messageContainer)
        .subscribe((res: PaginatedResult<Message[]>) => {
            this.messages = res.result;
            this.pagination = res.pagination;

        }, _error => {
          this.alertify.error(' message method not working');
        });

   }
   deleteMessage(id: number) {
      
    this.alertify.confirm('Are you sure you want to delete this message', () => {
         this.userService.deleteMessage(id, this.authservice.decodedToken.nameid).subscribe( () => {
          this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
          this.alertify.success('Message has been deleted');
         }, error => {
           this.alertify.error('Could not delete the message');
         });

    });


   }

   pageChanged(event: any): void {
        this.pagination.currentPage = event.page;
        this.loadMessages();

   }
}
