import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { formDirectiveProvider } from '@angular/forms/src/directives/ng_form';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
@Output() cancelregister = new EventEmitter;
user: User;
registerForm: FormGroup;
bsConfig: Partial<BsDatepickerConfig>;
  constructor(private authservice: AuthService, private alert: AlertifyService, private fb: FormBuilder,private router: Router) { }

  ngOnInit() {
   
   /* this.registerForm = new FormGroup({
       username: new FormControl('', Validators.required),
       password: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]),
       confirmPassword: new FormControl('', Validators.required)

    }, this.passwordMatching);
    */
   this.bsConfig = {
     containerClass: 'theme-green'

   };
   this.createRegisterForm(); // alternative
  }
  createRegisterForm() {
this.registerForm = this.fb.group({
  username: ['', Validators.required],
  password: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(20)] ],
  gender: ['male'],
  birthDate: [null, Validators.required],
  city: ['', Validators.required],
  country: ['', Validators.required],
  confirmPassword: ['', Validators.required]


},{validator: this.passwordMatching});

  }

  passwordMatching(p: FormGroup) {

    return p.get('password').value === p.get('confirmPassword').value ? null : {'mismatch': true};

  }

  register() {

    if (this.registerForm.valid) {
        this.user = Object.assign({}, this.registerForm.value);
           this.authservice.register(this.user).subscribe(() => {
              this.alert.success('Registration successful');

           }, error => {
               this.alert.error(error);
           }, () => {

                this.authservice.login(this.user).subscribe(() => {

                  this.router.navigate(['/members']);
                });
           });
    }

    /*
    this.authservice.register(this.model).subscribe(() => {
this.alert.success('registration sucessful');

    }, error => {
      this.alert.error(error);
    });
    */

  }
  cancel() {
    this.cancelregister.emit(false);
   
  }

}
