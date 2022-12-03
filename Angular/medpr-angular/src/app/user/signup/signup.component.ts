import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user';
import * as userActions from 'src/app/store/actions/auth.actions';
import { AppState, selectAuthState } from 'src/app/store/app.states';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent {
  getState: Observable<any>;
  errorMessage?: string | null;

  userform: FormGroup = new FormGroup({
    username: new FormControl(''),
    password: new FormControl(''),
  });

  user: User = {
    login: this.userform.controls['username'].value,
    password: this.userform.controls['password'].value,
  };

  constructor(private store: Store<AppState>) {
    this.getState = this.store.select(selectAuthState);
  }

  ngOnInit() {
    this.getState.subscribe((state) => {
      this.errorMessage = state.errorMessage;
    });
  }

  onSubmit(): void {
    this.store.dispatch(userActions.signup(this.user));
  }
}
