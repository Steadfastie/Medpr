import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { User } from 'src/app/models/user';
import { selectStateUser } from 'src/app/store/app.states';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user?: User;

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store
      .select(selectStateUser)
      .pipe()
      .subscribe((authStatus) => {
        this.user = authStatus;
      });
  }
}
