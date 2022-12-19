import { formatDate } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Prescription } from 'src/app/models/prescription';
import { Drug } from 'src/app/models/drug';
import { DrugsActionsService } from 'src/app/services/drugs/drugs.actions.service';
import { DrugsService } from 'src/app/services/drugs/drugs.service';
import { Doctor } from 'src/app/models/doctor';
import { DoctorsActionsService } from 'src/app/services/doctors/doctors.actions.service';
import { DoctorsService } from 'src/app/services/doctors/doctors.service';
import { selectStateUser } from 'src/app/store/app.states';
import { Store } from '@ngrx/store';
import { User } from 'src/app/models/user';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'prescription-card',
  templateUrl: './prescription.card.component.html',
  styleUrls: ['./prescription.card.component.scss'],
  providers: [DrugsActionsService, DoctorsActionsService],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class PrescriptionCardComponent implements OnInit {
  @Input() prescription?: Prescription;
  drug?: Drug;
  doctor?: Doctor;
  selected: boolean;
  daysLeft?: number;
  endedBeforeToday: boolean = false;
  user?: User;
  currentUserId?: string;

  constructor(private drugsService: DrugsService,
    private drugActions: DrugsActionsService,
    private doctorsService: DoctorsService,
    private doctorActions: DoctorsActionsService,
    private store: Store,
    ){
    this.selected = false;
  }

  ngOnInit(): void {
    this.store.select(selectStateUser).pipe().subscribe((authUser) => {
      this.currentUserId = authUser?.userId;
    });

    if (this.prescription && this.prescription.user
      && this.prescription.user['id'] != this.currentUserId) {
        this.user = this.prescription.user;
    }

    if (this.prescription && this.prescription.drug && this.prescription.doctor) {
      let endDate = new Date(this.prescription.endDate);
      let now = new Date();

      if(endDate.getTime() > now.getTime()) {
        let difference = endDate.getTime() - now.getTime();
        this.daysLeft = Math.ceil(difference / (1000 * 3600 * 24));
      } else {
        let difference = now.getTime() - endDate.getTime();
        this.daysLeft = Math.ceil(difference / (1000 * 3600 * 24));
        this.endedBeforeToday = true;
      }

      this.drugsService.getDrugById(this.prescription.drug.id)
        .subscribe(drug => this.drug = drug);
      this.doctorsService.getDoctorById(this.prescription.doctor.id)
        .subscribe(doctor => this.doctor = doctor);
    };

    this.drugActions.drugSelectListner().subscribe((selectedDrugId) => {
      if(selectedDrugId !== ''){
        this.drugsService.getDrugById(selectedDrugId).subscribe((drug) => {
            this.drug = drug
          });
      }
      else{
        this.drug = undefined;
      }
    });

    this.doctorActions.doctorSelectListner().subscribe((selectedDoctorId) => {
      if(selectedDoctorId !== ''){
        this.doctorsService.getDoctorById(selectedDoctorId).subscribe((doctor) => {
            this.doctor = doctor
          });
      }
      else{
        this.doctor = undefined;
      }
    });
  }

  selectToggle(){
    this.selected = !this.selected;
  }
}
