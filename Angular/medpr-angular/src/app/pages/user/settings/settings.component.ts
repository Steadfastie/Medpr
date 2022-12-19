import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class SettingsComponent {
  profileLinkClass = "activeLink"
  familiesLinkClass = "inactiveLink"

  constructor(private router: Router) {
  }

  switchTabs(tab: string) {
    if (tab == "families") {
      this.router.navigateByUrl("user/families");
      this.profileLinkClass = "inactiveLink";
      this.familiesLinkClass = "activeLink";
    }
    else{
      this.router.navigateByUrl("user/profile");
      this.profileLinkClass = "activeLink";
      this.familiesLinkClass = "inactiveLink";
    }
  }
}
