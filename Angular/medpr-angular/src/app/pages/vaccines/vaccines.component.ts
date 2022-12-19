import { VaccinesService } from 'src/app/services/vaccines/vaccines.service';
import { Component } from '@angular/core';
import { Vaccine } from 'src/app/models/vaccine';
import { VaccinesActionsService } from 'src/app/services/vaccines/vaccines.actions.service';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'vaccines',
  templateUrl: './vaccines.component.html',
  styleUrls: ['./vaccines.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class VaccinesComponent {
  vaccines: Vaccine[] = [];

  constructor(private vaccinesService: VaccinesService,
    private actions: VaccinesActionsService) {}

  ngOnInit() {
    this.vaccinesService.getAllVaccines().subscribe(vaccines => this.vaccines = vaccines);

    this.actions.vaccineResponseListner().subscribe(vaccineFromAction => {
      const presentVaccine = this.vaccines.find((presentVaccine) => {
        return presentVaccine.id === vaccineFromAction.id;
      })
      if (!presentVaccine) {
        this.vaccines.push(vaccineFromAction);
      }
      else{
        this.vaccines.splice(this.vaccines.indexOf(presentVaccine), 1, vaccineFromAction);
      }
    });

    this.actions.vaccineDeleteListner().subscribe(vaccineId => {
      const presentVaccine = this.vaccines.find((presentVaccine) => {
        return presentVaccine.id === vaccineId;
      })
      this.vaccines.splice(this.vaccines.indexOf(presentVaccine!), 1);
    });
  }
}
