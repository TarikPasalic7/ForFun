import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
@Output() cancelregister = new EventEmitter;
model: any = {};
  constructor(private authservice: AuthService, private alert: AlertifyService) { }

  ngOnInit() {
  }

  register() {
    this.authservice.register(this.model).subscribe(() => {
this.alert.success('registration sucessful');

    }, error => {
      this.alert.error(error);
    });
  }
  cancel() {
    this.cancelregister.emit(false);
   
  }

}
