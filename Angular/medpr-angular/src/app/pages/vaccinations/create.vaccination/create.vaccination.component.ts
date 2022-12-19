import { selectUserId } from '../../../store/app.states';
import { ToastrService } from 'ngx-toastr';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { Vaccination } from 'src/app/models/vaccination';
import { VaccinationsService } from 'src/app/services/vaccinations/vaccinations.service';

import { VaccinesService } from 'src/app/services/vaccines/vaccines.service';
import { Vaccine } from 'src/app/models/vaccine';
import { Store } from '@ngrx/store';
import { VaccinationsActionsService } from 'src/app/services/vaccinations/vaccinations.actions.service';
import { VaccinesActionsService } from 'src/app/services/vaccines/vaccines.actions.service';
import { formatDate } from '@angular/common';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'create-vaccination',
  templateUrl: './create.vaccination.component.html',
  styleUrls: ['./create.vaccination.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class CreateVaccinationComponent implements OnInit {
  @Output() deselect = new EventEmitter<void>();
  vaccines: Vaccine[] = [];
  userId?: string;

  showSpinner: boolean = false;
  errorMessage?: string;

  constructor(
    private fb: FormBuilder,
    private vaccinationsService: VaccinationsService,
    private VaccinesService: VaccinesService,
    private store: Store,
    private toastr: ToastrService,
    private vaccinationActions: VaccinationsActionsService,
    private vaccineActions: VaccinesActionsService,
  ) {}

  ngOnInit(): void {
    this.VaccinesService.getAllVaccines().subscribe(
      (vaccines) => (this.vaccines = vaccines)
    );

    this.store.select(selectUserId).pipe()
      .subscribe((userId) => {
        this.userId = userId;
      });
  }

  vaccinationForm = this.fb.group({
    vaccinationDate: ['', [Validators.required]],
    reVaccinationDate: ['',[Validators.required]],
    vaccineId: ['', [Validators.required]],
  });

  submit() {
    if (!this.showSpinner && this.vaccinationForm.valid) {
      this.showSpinner = true;

      let vaccinationDate = new Date(this.vaccinationForm.value.vaccinationDate!);
      let day = vaccinationDate.getDate();
      let month = vaccinationDate.getMonth() + 1;
      let year = vaccinationDate.getFullYear();

      let vaccinationDateTime = year +
        '-' + month.toLocaleString('en-US', {minimumIntegerDigits: 2}) +
        '-' + day.toLocaleString('en-US', {minimumIntegerDigits: 2})

      let reVaccinationDate = new Date(this.vaccinationForm.value.reVaccinationDate!);
      let difference = reVaccinationDate.getTime() - vaccinationDate.getTime();
      let daysOfProtection = Math.ceil(difference / (1000 * 3600 * 24));

      const vaccination: Vaccination = {
        id: Guid.createEmpty().toString(),
        date: vaccinationDateTime,
        daysOfProtection: daysOfProtection,
        userId: this.userId!,
        vaccineId: this.vaccinationForm.value.vaccineId!,
      };

      this.vaccinationsService.create(vaccination).pipe()
        .subscribe({
          next: (responseVaccination) => {
            this.showSpinner = false;
            this.vaccinationActions.emitVaccinationResponse(responseVaccination);
            this.toastr.success(`Vaccination on ${formatDate(responseVaccination.date, 'longDate', 'en-US')} is now in the app`, `Created`);
            this.closeCreate();
          },
          error: (err) => {
            this.showSpinner = false;
            console.log(err);
            this.toastr.error(`Vaccination on ${formatDate(vaccination.date, 'longDate', 'en-US')} is still in your dreams`, `Failed`);
            this.errorMessage = 'Could not create vaccination';
          },
        });
    }
  }

  selectVaccine(vaccineId: any) {
    this.vaccineActions.emitVaccineSelect(vaccineId.source.value)
  }

  cancel() {
    if (!this.showSpinner) {
      this.vaccinationForm.reset();
      this.vaccineActions.emitVaccineSelect('');
    }
  }

  closeCreate() {
    if (!this.showSpinner) {
      this.vaccineActions.emitVaccineSelect('');
      this.deselect.emit();
    }
  }
}
