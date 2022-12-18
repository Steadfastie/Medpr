import { Component, OnInit } from '@angular/core';
import { Family } from 'src/app/models/family';
import { FamiliesActionsService } from 'src/app/services/families/families.actions.service';
import { FamiliesService } from 'src/app/services/families/families.service';

@Component({
  selector: 'family-feed',
  templateUrl: './family.feed.component.html',
  styleUrls: ['./family.feed.component.scss']
})
export class FamilyFeedComponent implements OnInit {
  userFamilies: Family[] = [];

  constructor(private familiesService: FamiliesService,
    private actions: FamiliesActionsService) { }

  ngOnInit(): void {
    this.familiesService.getAllFamilies().pipe()
      .subscribe((families) => this.userFamilies = families);

    // Add family to list on create
    this.actions.familyResponseListner().subscribe(familyFromAction => {
      this.userFamilies.push(familyFromAction);
    });

    // Remove family from list on delete
    this.actions.familyDeleteListner().subscribe(familyId => {
      const presentFamily = this.userFamilies.find((family) => {
        return family.id === familyId;
      })
      this.userFamilies.splice(this.userFamilies.indexOf(presentFamily!), 1);
    });

    // Remove family from list if user got out
    this.actions.familyGetOutListner().subscribe(familyId => {
      const presentFamily = this.userFamilies.find((family) => {
        return family.id === familyId;
      })
      this.userFamilies.splice(this.userFamilies.indexOf(presentFamily!), 1);
    });

    this.actions.familyJoinedListner().subscribe(familyId => {
      this.familiesService.getFamilyById(familyId).subscribe((family) => {
        this.userFamilies.push(family);
      })
    })
  }

}
