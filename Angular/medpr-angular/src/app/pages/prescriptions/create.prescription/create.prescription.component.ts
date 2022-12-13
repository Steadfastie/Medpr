import { selectUserId } from '../../../store/app.states';
import { ToastrService } from 'ngx-toastr';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { Prescription } from 'src/app/models/prescription';
import { PrescriptionsService } from 'src/app/services/prescriptions/prescriptions.service';

import { DrugsService } from 'src/app/services/drugs/drugs.service';
import { Drug } from 'src/app/models/drug';
import { Store } from '@ngrx/store';
import { PrescriptionsActionsService } from 'src/app/services/prescriptions/prescriptions.actions.service';
import { DrugsActionsService } from 'src/app/services/drugs/drugs.actions.service';
import { formatDate } from '@angular/common';
import { Doctor } from 'src/app/models/doctor';
import { DoctorsService } from 'src/app/services/doctors/doctors.service';
import { DoctorsActionsService } from 'src/app/services/doctors/doctors.actions.service';

@Component({
  selector: 'create-prescription',
  templateUrl: './create.prescription.component.html',
  styleUrls: ['./create.prescription.component.scss']
})
export class CreatePrescriptionComponent implements OnInit {
  @Output() deselect = new EventEmitter<void>();
  drugs: Drug[] = [];
  doctors?: Doctor[];
  userId?: string;

  showSpinner: boolean = false;
  errorMessage?: string;

  constructor(
    private fb: FormBuilder,
    private prescriptionsService: PrescriptionsService,
    private prescriptionActions: PrescriptionsActionsService,
    private drugsService: DrugsService,
    private drugActions: DrugsActionsService,
    private doctorsService: DoctorsService,
    private doctorActions: DoctorsActionsService,
    private store: Store,
    private toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    this.drugsService.getAllDrugs().subscribe(
      (drugs) => (this.drugs = drugs)
    );

    this.doctorsService.getAllDoctors().subscribe(
      (doctors) => (this.doctors = doctors)
    );

    this.store.select(selectUserId).pipe()
      .subscribe((userId) => {
        this.userId = userId;
      });
  }

  prescriptionForm = this.fb.group({
    startDate: ['', [Validators.required]],
    endDate: ['',[Validators.required]],
    dose: ['',[Validators.required, Validators.min(1)]],
    drugId: ['', [Validators.required]],
    doctorId: ['', [Validators.required]],
  });

  submit() {
    if (!this.showSpinner && this.prescriptionForm.valid) {
      this.showSpinner = true;

      let startDate = new Date(this.prescriptionForm.value.startDate!);
      let startDay = startDate.getDate();
      let startMonth = startDate.getMonth() + 1;
      let startYear = startDate.getFullYear();

      let startDateTime = startYear + '-' + startMonth + '-' + startDay

      let endDate = new Date(this.prescriptionForm.value.endDate!);
      let endDay = endDate.getDate();
      let endMonth = endDate.getMonth() + 1;
      let endYear = endDate.getFullYear();

      let endDateTime = endYear + '-' + endMonth + '-' + endDay

      const prescription: Prescription = {
        id: Guid.createEmpty().toString(),
        date: startDateTime,
        endDate: endDateTime,
        dose: Number(this.prescriptionForm.value.dose!),
        userId: this.userId!,
        drugId: this.prescriptionForm.value.drugId!,
        doctorId: this.prescriptionForm.value.doctorId!,
      };

      this.prescriptionsService.create(prescription).pipe()
        .subscribe({
          next: (responsePrescription) => {
            this.showSpinner = false;
            this.prescriptionActions.emitPrescriptionResponse(responsePrescription);
            this.toastr.success(`Prescription on ${formatDate(responsePrescription.date, 'longDate', 'en-US')} is now in the app`, `Created`);
            this.closeCreate();
          },
          error: (err) => {
            this.showSpinner = false;
            console.log(err);
            this.toastr.error(`Prescription on ${formatDate(prescription.date, 'longDate', 'en-US')} is still in your dreams`, `Failed`);
            this.errorMessage = 'Could not create prescription';
          },
        });
    }
  }

  selectDrug(drugId: any) {
    this.drugActions.emitDrugSelect(drugId.source.value)
  }

  selectDoctor(doctorId: any) {
    this.doctorActions.emitDoctorSelect(doctorId.source.value)
  }

  cancel() {
    if (!this.showSpinner) {
      this.prescriptionForm.reset();
      this.drugActions.emitDrugSelect('');
      this.doctorActions.emitDoctorSelect('');
    }
  }

  closeCreate() {
    if (!this.showSpinner) {
      this.drugActions.emitDrugSelect('');
      this.doctorActions.emitDoctorSelect('');
      this.deselect.emit();
    }
  }
}
