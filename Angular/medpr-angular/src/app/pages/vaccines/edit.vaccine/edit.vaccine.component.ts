import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Vaccine } from 'src/app/models/vaccine';
import { VaccinesActionsService } from 'src/app/services/vaccines/vaccines.actions.service';
import { VaccinesService } from 'src/app/services/vaccines/vaccines.service';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'edit-vaccine',
  templateUrl: './edit.vaccine.component.html',
  styleUrls: ['./edit.vaccine.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class EditVaccineComponent implements OnInit {
  @Input() vaccine?: Vaccine;
  @Output() deselect = new EventEmitter<void>();
  showSpinner: boolean = false;
  errorMessage?: string;

  constructor(private fb: FormBuilder,
    private VaccinesService: VaccinesService,
    private toastr: ToastrService,
    private actions: VaccinesActionsService) { }

  ngOnInit(): void {
    this.initialize();
  }

  vaccineForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
    reason: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    price: ['', [Validators.required, Validators.min(0)]],
  });

  initialize() {
    if (this.vaccine) {
      this.vaccineForm.setValue({
        name: this.vaccine.name,
        reason: this.vaccine.reason,
        price: this.vaccine.price.toString()
      })
    }
  }

  edit(){
    if (!this.showSpinner && this.vaccineForm.valid){
      this.showSpinner = true;
      const initialVaccine = {
        id: this.vaccine!.id,
        name: this.vaccine!.name,
        price: this.vaccine!.price,
      }
      const modifiedVaccine: Vaccine = {
        id: this.vaccine?.id!,
        name: this.vaccineForm.value.name!,
        reason: this.vaccineForm.value.reason!,
        price: Number(this.vaccineForm.value.price!)
      }
      if (JSON.stringify(modifiedVaccine) !== JSON.stringify(initialVaccine)){
        this.VaccinesService.patch(modifiedVaccine).pipe().subscribe({
          next: (vaccine) => {
            this.showSpinner = false;
            this.actions.emitVaccineResponse(vaccine);
            this.toastr.success(`Success`,`${vaccine.name} updated`);
            this.closeEdit();
          },
          error: (err) => {
            this.showSpinner = false;
            console.log(`${err}`);
            this.toastr.success(`Failed`,`${modifiedVaccine.name} is still the same`);
            this.errorMessage = "Could not modify vaccine";
          },
        });
      }
    }

  }

  remove(){
    if (!this.showSpinner){
      this.showSpinner = true;
      this.VaccinesService.delete(this.vaccine!.id).pipe().subscribe({
        next: () => {
          this.showSpinner = false;
          this.toastr.success(`Success`, `${this.vaccine!.name} removed`);
          this.actions.emitVaccineDelete(this.vaccine!.id);
        },
        error: (err) => {
          this.toastr.error(`Failed`, `${this.vaccine!.name} still persist`);
          console.log(`${err.message}`);
        },
      });
    }
  }

  closeEdit(){
    if (!this.showSpinner){
      this.deselect.emit();
    }
  }
}
