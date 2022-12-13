import { formatDate } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { Vaccination } from 'src/app/models/vaccination';
import { Vaccine } from 'src/app/models/vaccine';
import { VaccinationsActionsService } from 'src/app/services/vaccinations/vaccinations.actions.service';
import { VaccinationsService } from 'src/app/services/vaccinations/vaccinations.service';
import { VaccinesActionsService } from 'src/app/services/vaccines/vaccines.actions.service';
import { VaccinesService } from 'src/app/services/vaccines/vaccines.service';
import { selectUserId } from 'src/app/store/app.states';


@Component({
  selector: 'edit-vaccination',
  templateUrl: './edit.vaccination.component.html',
  styleUrls: ['./edit.vaccination.component.scss']
})
export class EditVaccinationComponent implements OnInit {
  @Input() vaccination?: Vaccination;
  @Output() deselect = new EventEmitter<void>();
  showSpinner: boolean = false;
  errorMessage?: string;
  userId?: string;
  vaccines?: Vaccine[];

  constructor(private fb: FormBuilder,
    private VaccinationsService: VaccinationsService,
    private actions: VaccinationsActionsService,
    private VaccinesService: VaccinesService,
    private vaccineActions: VaccinesActionsService,
    private store: Store,
    private toastr: ToastrService,
    ) { }

  ngOnInit(): void {
    this.initialize();

    this.VaccinesService.getAllVaccines().subscribe(
      (vaccines) => (this.vaccines = vaccines)
    );

    this.store.select(selectUserId).pipe()
      .subscribe((userId) => {
        this.userId = userId;
      });
  }

  vaccinationForm = this.fb.group({
    date: ['', [Validators.required]],
    daysOfProtection: ['',[Validators.required, Validators.min(0)]],
    vaccineId: ['', [Validators.required]],
  });

  initialize() {
    if (this.vaccination) {
      this.vaccinationForm.setValue({
        date: this.vaccination.date,
        daysOfProtection: this.vaccination.daysOfProtection.toString(),
        vaccineId: this.vaccination.vaccine?.id!,
      })
      this.vaccineActions.emitVaccineSelect(this.vaccination.vaccine?.id!)
    }
  }

  edit(){
    if (!this.showSpinner && this.vaccinationForm.valid){
      this.showSpinner = true;
      const initialVaccination = {
        id: this.vaccination!.id,
        date: this.vaccination!.date,
        daysOfProtection: this.vaccination!.daysOfProtection,
        vaccineId: this.vaccination!.vaccineId,
      }

      let date = new Date(this.vaccinationForm.value.date!);
      let day = date.getDate();
      let month = date.getMonth() + 1;
      let year = date.getFullYear();

      let dateTime = year + '-' + month + '-' + day + 'T21:00:00'

      const modifiedVaccination: Vaccination = {
        id: this.vaccination?.id!,
        date: dateTime,
        daysOfProtection: Number(this.vaccinationForm.value.daysOfProtection!),
        userId: this.userId!,
        vaccineId: this.vaccinationForm.value.vaccineId!,
      }
      if (JSON.stringify(modifiedVaccination) !== JSON.stringify(initialVaccination)){
        this.VaccinationsService.patch(modifiedVaccination).pipe().subscribe({
          next: (vaccination) => {
            this.showSpinner = false;
            this.actions.emitVaccinationResponse(vaccination);
            this.toastr.info(`Vaccination on ${formatDate(vaccination.date, 'longDate', 'en-US')} updated`, `Updated`);
            this.closeEdit();
          },
          error: (err) => {
            this.showSpinner = false;
            console.log(`${err}`);
            this.toastr.error(`Appointment on ${formatDate(modifiedVaccination.date, 'longDate', 'en-US')} is still the same`, `Failed`);
            this.errorMessage = "Could not modify vaccination";
          },
        });
      }
    }

  }

  remove(){
    if (!this.showSpinner){
      this.showSpinner = true;
      this.VaccinationsService.delete(this.vaccination!.id).pipe().subscribe({
        next: () => {
          this.showSpinner = false;
          this.toastr.success(`Success`, `${this.vaccination!.date} removed`);
          this.actions.emitVaccinationDelete(this.vaccination!.id);
        },
        error: (err) => {
          this.toastr.warning(`Failed`, `${this.vaccination!.date} still persist`);
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

  selectVaccine(vaccineId: any) {
    this.vaccineActions.emitVaccineSelect(vaccineId.source.value)
  }
}
