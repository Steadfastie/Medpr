import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { User } from './models/user';
import * as userActions from 'src/app/store/actions/auth.actions';
import { selectStateUser } from './store/app.states';
import { SignalrService } from './modules/notifications/services/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  user?: User;
  constructor(private store: Store, private singlarService: SignalrService) {
    this.singlarService.startConnection();
    this.singlarService.addNotificationListner();
  }
  ngOnInit(): void {
    this.store
      .select(selectStateUser)
      .pipe()
      .subscribe((authStatus) => {
        this.user = authStatus;
      });
  }

  logout() {
    this.store.dispatch(userActions.logout());
  }
}
