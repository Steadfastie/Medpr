import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { User } from './models/user';
import * as userActions from 'src/app/store/actions/auth.actions';
import { selectStateUser } from './store/app.states';
import { SignalrService } from './modules/notifications/services/signalr.service';
import { UsersService } from './services/users/users.service';
import { UsersActionsService } from './services/users/users.actions.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  user?: User;
  name?: string;

  constructor(private store: Store,
    private singlarService: SignalrService,
    private usersService: UsersService,
    private usersActions: UsersActionsService,
    private router: Router,
    ) {
    this.singlarService.startConnection();
    this.singlarService.addNotificationListner();
  }
  ngOnInit(): void {
    this.store.select(selectStateUser).pipe()
      .subscribe((authStatus) => {
        this.user = authStatus;
        let dogIndex = this.user?.login.lastIndexOf('@');
        this.name = this.user?.login.substring(0, dogIndex);
      });

      this.usersService.getUserById(this.user!.userId!).pipe().subscribe((user) => {
        this.user = user;
        this.user.userId = user["id"];
        if (user.fullName){
          this.name = this.user.fullName;
        }
      });

      this.usersActions.userResponseListner().subscribe((changedUser) => {
        this.user = changedUser;
        if (changedUser.fullName){
          this.name = this.user.fullName;
        }
      });
  }

  logout() {
    // this.router.navigate(['/signin']);
    this.store.dispatch(userActions.logout());
  }
}
