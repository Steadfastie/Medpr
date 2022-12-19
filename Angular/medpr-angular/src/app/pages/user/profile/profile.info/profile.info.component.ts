import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { UsersService } from 'src/app/services/users/users.service';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'profile-info',
  templateUrl: './profile.info.component.html',
  styleUrls: ['./profile.info.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})

export class ProfileInfoComponent implements OnInit {
  @Input() user?: User;
  name?: string;

  constructor(private userService: UsersService) { }

  ngOnInit(): void {
    if(this.user?.fullName) {
      this.name = this.user.fullName;
    } else {
      let dogIndex = this.user?.login.lastIndexOf('@');
      this.name = this.user?.login.substring(0, dogIndex);
    }
  }
}
