import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { User } from './models/user';
import { selectStateUser } from './store/app.states';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  user?:User;
  constructor(private store: Store){
  }
  ngOnInit(): void {
    this.store.select(selectStateUser).pipe()
      .subscribe((authStatus) => {
        this.user = authStatus;
      });
  }
}
