import { formatDate } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Store } from '@ngrx/store';
import { User } from 'src/app/models/user';
import { Vaccination } from 'src/app/models/vaccination';
import { Vaccine } from 'src/app/models/vaccine';
import { VaccinesActionsService } from 'src/app/services/vaccines/vaccines.actions.service';
import { VaccinesService } from 'src/app/services/vaccines/vaccines.service';
import { selectStateUser } from 'src/app/store/app.states';

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
  endDate?: string;
  daysLeft?: number;
  endedBeforeToday: boolean = false;
  user?: User;
  currentUserId?: string;

  constructor(private vaccinesService: VaccinesService,
    private vaccineActions: VaccinesActionsService,
    private store: Store,
    ){
    this.selected = false;
  }

  ngOnInit(): void {
    this.store.select(selectStateUser).pipe().subscribe((authUser) => {
      this.currentUserId = authUser?.userId;
    });

    if (this.vaccination && this.vaccination.user
      && this.vaccination.user['id'] != this.currentUserId) {
        this.user = this.vaccination.user;
    }

    if (this.vaccination && this.vaccination.vaccine) {
      let endDate = new Date(this.vaccination.date);
      endDate.setDate(endDate.getDate() + this.vaccination.daysOfProtection);
      this.endDate = formatDate(endDate, 'fullDate', 'en-US');

      let now = new Date();
      if(endDate.getTime() > now.getTime()) {
        let difference = endDate.getTime() - now.getTime();
        this.daysLeft = Math.ceil(difference / (1000 * 3600 * 24));
      } else {
        let difference = now.getTime() - endDate.getTime();
        this.daysLeft = Math.ceil(difference / (1000 * 3600 * 24));
        this.endedBeforeToday = true;
      }


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
