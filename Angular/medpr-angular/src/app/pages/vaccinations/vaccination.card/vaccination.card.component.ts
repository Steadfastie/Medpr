import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Vaccination } from 'src/app/models/vaccination';
import { Vaccine } from 'src/app/models/vaccine';
import { VaccinesActionsService } from 'src/app/services/vaccines/vaccines.actions.service';
import { VaccinesService } from 'src/app/services/vaccines/vaccines.service';

@Component({
  selector: 'vaccination-card',
  templateUrl: './vaccination.card.component.html',
  styleUrls: ['./vaccination.card.component.scss'],
  providers: [VaccinesActionsService]
})
export class VaccinationCardComponent implements OnInit {
  @Input() vaccination?: Vaccination;
  vaccine?: Vaccine;
  selected: boolean;

  constructor(private vaccinesService: VaccinesService,
    private vaccineActions: VaccinesActionsService,
    ){
    this.selected = false;
  }

  ngOnInit(): void {
    if (this.vaccination && this.vaccination.vaccine) {
      this.vaccinesService.getVaccineById(this.vaccination.vaccine.id)
        .subscribe(vaccine => this.vaccine = vaccine);
    };

    this.vaccineActions.vaccineSelectListner().subscribe((selectedVaccineId) => {
      if(selectedVaccineId !== ''){
        this.vaccinesService.getVaccineById(selectedVaccineId).subscribe((vaccine) => {
            this.vaccine = vaccine
          });
      }
      else{
        this.vaccine = undefined;
      }
    });
  }

  selectToggle(){
    this.selected = !this.selected;
  }
}
