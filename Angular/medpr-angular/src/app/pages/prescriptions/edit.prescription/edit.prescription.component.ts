import { formatDate } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { Prescription } from 'src/app/models/prescription';
import { Drug } from 'src/app/models/drug';
import { PrescriptionsActionsService } from 'src/app/services/prescriptions/prescriptions.actions.service';
import { PrescriptionsService } from 'src/app/services/prescriptions/prescriptions.service';
import { DrugsActionsService } from 'src/app/services/drugs/drugs.actions.service';
import { DrugsService } from 'src/app/services/drugs/drugs.service';
import { selectUserId } from 'src/app/store/app.states';
import { Doctor } from 'src/app/models/doctor';
import { DoctorsActionsService } from 'src/app/services/doctors/doctors.actions.service';
import { DoctorsService } from 'src/app/services/doctors/doctors.service';


@Component({
  selector: 'edit-prescription',
  templateUrl: './edit.prescription.component.html',
  styleUrls: ['./edit.prescription.component.scss']
})
export class EditPrescriptionComponent implements OnInit {
  @Input() prescription?: Prescription;
  @Output() deselect = new EventEmitter<void>();
  showSpinner: boolean = false;
  errorMessage?: string;
  userId?: string;
  drugs?: Drug[];
  doctors?: Doctor[];

  constructor(private fb: FormBuilder,
    private PrescriptionsService: PrescriptionsService,
    private actions: PrescriptionsActionsService,
    private drugsService: DrugsService,
    private drugActions: DrugsActionsService,
    private doctorsService: DoctorsService,
    private doctorActions: DoctorsActionsService,
    private store: Store,
    private toastr: ToastrService,
    ) { }

  ngOnInit(): void {
    this.initialize();

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

  initialize() {
    if (this.prescription) {
      this.prescriptionForm.setValue({
        startDate: this.prescription.date,
        endDate: this.prescription.endDate,
        dose: this.prescription.dose.toString(),
        drugId: this.prescription.drug?.id!,
        doctorId: this.prescription.doctor?.id!,
      })
      this.drugActions.emitDrugSelect(this.prescription.drug?.id!)
      this.doctorActions.emitDoctorSelect(this.prescription.doctor?.id!)
    }
  }

  edit(){
    if (!this.showSpinner && this.prescriptionForm.valid){
      this.showSpinner = true;
      const initialPrescription = {
        id: this.prescription!.id,
        date: this.prescription!.date,
        endDate: this.prescription!.endDate,
        dose: this.prescription!.dose,
        drugId: this.prescription!.drugId,
        doctorId: this.prescription!.doctorId,
        userId: this.prescription!.user!['id']!,
      }

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

      const modifiedPrescription: Prescription = {
        id: this.prescription?.id!,
        date: startDateTime,
        endDate: endDateTime,
        dose: Number(this.prescriptionForm.value.dose!),
        userId: this.prescription!.user!['id']!,
        drugId: this.prescriptionForm.value.drugId!,
        doctorId: this.prescriptionForm.value.doctorId!,
      }
      if (JSON.stringify(modifiedPrescription) !== JSON.stringify(initialPrescription)){
        this.PrescriptionsService.patch(modifiedPrescription).pipe().subscribe({
          next: (prescription) => {
            this.showSpinner = false;
            this.actions.emitPrescriptionResponse(prescription);
            this.toastr.info(`Prescription on ${formatDate(prescription.date, 'longDate', 'en-US')} updated`, `Updated`);
            this.closeEdit();
          },
          error: (err) => {
            this.showSpinner = false;
            console.log(`${err}`);
            this.toastr.error(`Prescription on ${formatDate(modifiedPrescription.date, 'longDate', 'en-US')} is still the same`, `Failed`);
            this.errorMessage = "Could not modify prescription";
          },
        });
      }
    }
  }

  remove(){
    if (!this.showSpinner){
      this.showSpinner = true;
      this.PrescriptionsService.delete(this.prescription!.id).pipe().subscribe({
        next: () => {
          this.showSpinner = false;
          this.toastr.success(`Prescription on ${formatDate(this.prescription?.date!, 'longDate', 'en-US')} removed`, `Success`);
          this.actions.emitPrescriptionDelete(this.prescription!.id);
        },
        error: (err) => {
          this.toastr.warning(`Prescription on ${formatDate(this.prescription?.date!, 'longDate', 'en-US')} still persist`, `Failed`);
          console.log(`${err.message}`);
        },
    });
  }
}

  closeEdit(){
    if (!this.showSpinner){
      this.initialize();
      this.deselect.emit();
    }
  }

  selectDrug(drugId: any) {
    this.drugActions.emitDrugSelect(drugId.source.value)
  }

  selectDoctor(doctorId: any) {
    this.doctorActions.emitDoctorSelect(doctorId.source.value)
  }
}
