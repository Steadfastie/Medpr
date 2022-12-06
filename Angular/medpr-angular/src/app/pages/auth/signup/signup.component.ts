import { Observable } from 'rxjs/internal/Observable';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { selectStateAuthStatus } from 'src/app/store/app.states';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
})
export class SignupComponent implements OnInit {
  selected: boolean = false;
  isAuthenticated: boolean;

  constructor(private router: Router,
    private store: Store,
    private fb: FormBuilder,) {
    this.isAuthenticated = false;
  }

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
