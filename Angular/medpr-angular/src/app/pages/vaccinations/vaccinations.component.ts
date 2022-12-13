import { VaccinationsService } from 'src/app/services/vaccinations/vaccinations.service';
import { Component } from '@angular/core';
import { Vaccination } from 'src/app/models/vaccination';
import { ActivatedRoute } from '@angular/router';
import { VaccinationsActionsService } from 'src/app/services/vaccinations/vaccinations.actions.service';

@Component({
  selector: 'vaccinations',
  templateUrl: './vaccinations.component.html',
  styleUrls: ['./vaccinations.component.scss']
})
export class VaccinationsComponent {
  vaccinations: Vaccination[] = [];
  idProvided: boolean = false;

  constructor(
    private vaccinationsService: VaccinationsService,
    private actions: VaccinationsActionsService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    const vaccinationId = this.route.snapshot.paramMap.get('id');
    if (vaccinationId == null) {
      this.vaccinationsService.getAllVaccinations()
        .subscribe((vaccinations) => (this.vaccinations = vaccinations));
    } else {
      this.vaccinationsService.getVaccinationById(vaccinationId)
        .subscribe((vaccination) => {
          this.vaccinations.push(vaccination),
          this.idProvided = true;
        });
    }

    this.actions.vaccinationResponseListner().subscribe((vaccinationFromAction) => {
        if (this.vaccinations) {
          const presentVaccination = this.vaccinations.find((presentVaccination) => {
              return presentVaccination.id === vaccinationFromAction.id;
            }
          );
          if (!presentVaccination) {
            this.vaccinations.push(vaccinationFromAction);
          } else {
            this.vaccinations.splice(
              this.vaccinations.indexOf(presentVaccination), 1, vaccinationFromAction
            );
          }
        } else {
          this.vaccinations = [];
          this.vaccinations.push(vaccinationFromAction);
        }
      });

      this.actions.vaccinationDeleteListner().subscribe(vaccinationId => {
        const presentVaccination = this.vaccinations.find((presentVaccination) => {
          return presentVaccination.id === vaccinationId;
        })
        this.vaccinations.splice(this.vaccinations.indexOf(presentVaccination!), 1);
      });
  }
}
