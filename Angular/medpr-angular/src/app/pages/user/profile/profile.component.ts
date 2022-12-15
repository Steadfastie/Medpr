import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { User } from 'src/app/models/user';
import { UsersActionsService } from 'src/app/services/users/users.actions.service';
import { UsersService } from 'src/app/services/users/users.service';
import { selectStateUser } from 'src/app/store/app.states';

import * as userActions from 'src/app/store/actions/auth.actions';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user?: User;
  profile?: User;
  name?: string;

  constructor(private store: Store,
    private usersService: UsersService,
    private actions: UsersActionsService,
    ) {}

  ngOnInit(): void {
    this.store.select(selectStateUser).pipe().subscribe((authStatus) => {
        this.user = authStatus
      });

      this.usersService.getUserById(this.user!.userId!).pipe().subscribe((user) => {
        this.profile = user;
        this.profile.userId = user["id"];
      })

      this.actions.userResponseListner().subscribe((userFromAction) => {
        this.profile = userFromAction;
      });

      this.actions.userDeleteListner().subscribe(() => {
        this.store.dispatch(userActions.logout());
      });

      if(this.user?.fullName) {
        this.name = this.user.fullName;
      } else {
        let dogIndex = this.user?.login.lastIndexOf('@');
        this.name = this.user?.login.substring(0, dogIndex);
      }

  }
}
