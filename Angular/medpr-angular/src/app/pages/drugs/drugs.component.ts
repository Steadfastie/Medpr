import { DrugsService } from 'src/app/services/drugs/drugs.service';
import { Component } from '@angular/core';
import { Drug } from 'src/app/models/drug';
import { DrugsActionsService } from 'src/app/services/drugs/drugs.actions.service';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'drugs',
  templateUrl: './drugs.component.html',
  styleUrls: ['./drugs.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class DrugsComponent {
  drugs: Drug[] = [];
  randomDrug?: Drug;

  constructor(private DrugsService: DrugsService,
    private actions: DrugsActionsService) {}

  ngOnInit() {
    this.DrugsService.getAllDrugs().subscribe(drugs => this.drugs = drugs);
    this.DrugsService.getRandomDrug().subscribe(random => this.randomDrug = random);

    this.actions.drugResponseListner().subscribe(drugFromAction => {
      const presentDrug = this.drugs.find((presentDrug) => {
        return presentDrug.id === drugFromAction.id;
      })
      if (!presentDrug) {
        this.drugs.push(drugFromAction);
      }
      else{
        this.drugs.splice(this.drugs.indexOf(presentDrug), 1, drugFromAction);
      }
    });

    this.actions.drugDeleteListner().subscribe(drugId => {
      const presentDrug = this.drugs.find((presentDrug) => {
        return presentDrug.id === drugId;
      })
      this.drugs.splice(this.drugs.indexOf(presentDrug!), 1);
    });
  }
}
