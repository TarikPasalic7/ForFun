import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
@Output() cancelregister = new EventEmitter;
model: any = {};
  constructor(private authservice: AuthService) { }

  ngOnInit() {
  }

  register() {
    this.authservice.register(this.model).subscribe(() => {
console.log('registration sucessful');

    }, error => {
      console.log(error);
    });
  }
  cancel() {
    this.cancelregister.emit(false);
    console.log('cancel');
  }

}
