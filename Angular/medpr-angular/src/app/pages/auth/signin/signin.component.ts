import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { selectStateAuthStatus } from 'src/app/store/app.states';
import { FormBuilder, Validators } from '@angular/forms';
import * as userActions from 'src/app/store/actions/auth.actions';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SigninComponent implements OnInit {
  selected: boolean = false;
  isAuthenticated: boolean = false;

  constructor(private router: Router,
    private store: Store,
    private fb: FormBuilder,) {}

  ngOnInit() {
    this.store.select(selectStateAuthStatus).pipe()
      .subscribe((authStatus) => {
        this.isAuthenticated = authStatus;
      });
  }

  signUpForm = this.fb.group({
    login: ['', [Validators.required,Validators.email, Validators.minLength(5), Validators.maxLength(30)]],
    password: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(30)]],
  });

  submit(){
    if(this.signUpForm.valid){
      const user: User = {
        login: this.signUpForm.value.login!,
        password: this.signUpForm.value.password!,
      }
      this.store.dispatch(userActions.signin(user));
    }
  }

  selectToggle() {
    this.selected =!this.selected;
  }

  fillDefaultPassword() {
    this.signUpForm.patchValue({
      password: '8yQD!yya'
    })
  }
}
