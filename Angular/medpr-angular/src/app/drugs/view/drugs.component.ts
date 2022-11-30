import { DrugsService } from './../../services/drugs/drugs.service';
import { Component } from '@angular/core';
import { Drug } from 'src/app/models/drug';

@Component({
  selector: 'drugs',
  templateUrl: './drugs.component.html',
  styleUrls: ['./drugs.component.scss'],
})
export class DrugsComponent {
  drugs: Drug[] = [];
  randomDrug?: Drug;

  constructor(private DrugsService: DrugsService) {}

  ngOnInit() {
    this.DrugsService.getAllDrugs().subscribe(drugs => this.drugs = drugs);
    this.DrugsService.getRandomDrug().subscribe(random => this.randomDrug = random);
  }

}
