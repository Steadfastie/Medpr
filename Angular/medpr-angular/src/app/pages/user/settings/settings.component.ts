import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
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
