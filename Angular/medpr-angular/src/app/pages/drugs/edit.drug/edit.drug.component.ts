import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Drug } from 'src/app/models/drug';
import { DrugsActionsService } from 'src/app/services/drugs/drugs.actions.service';
import { DrugsService } from 'src/app/services/drugs/drugs.service';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'edit-drug',
  templateUrl: './edit.drug.component.html',
  styleUrls: ['./edit.drug.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class EditDrugComponent implements OnInit {
  @Input() drug?: Drug;
  @Output() deselect = new EventEmitter<void>();
  showSpinner: boolean = false;
  errorMessage?: string;

  constructor(private fb: FormBuilder,
    private DrugsService: DrugsService,
    private actions: DrugsActionsService,
    private toastr: ToastrService,) { }

  ngOnInit(): void {
    this.initialize();
  }

  drugForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
    pharmGroup: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    price: ['', [Validators.required, Validators.min(1)]],
  });

  initialize() {
    if (this.drug) {
      this.drugForm.setValue({
        name: this.drug.name,
        pharmGroup: this.drug.pharmGroup,
        price: this.drug.price.toString()
      })
    }
  }

  edit(){
    if (!this.showSpinner && this.drugForm.valid){
      this.showSpinner = true;
      const initialDrug = {
        id: this.drug!.id,
        name: this.drug!.name,
        pharmGroup: this.drug!.pharmGroup,
        price: this.drug!.price
      }
      const modifiedDrug: Drug = {
        id: this.drug?.id!,
        name: this.drugForm.value.name!,
        pharmGroup: this.drugForm.value.pharmGroup!,
        price: Number(this.drugForm.value.price!)
      }
      if (JSON.stringify(modifiedDrug) !== JSON.stringify(initialDrug)){
        this.DrugsService.patch(modifiedDrug).pipe().subscribe({
          next: (drug) => {
            this.showSpinner = false;
            this.actions.emitDrugResponse(drug);
            this.toastr.success(`Success`,`${drug.name} updated`);
            this.closeEdit();
          },
          error: (err) => {
            this.showSpinner = false;
            console.log(`${err}`);
            this.toastr.error(`Failed`,`${modifiedDrug.name} is still the same`);
            this.errorMessage = "Could not modify drug";
          },
        });
      }
    }

  }

  remove(){
    if (!this.showSpinner){
      this.showSpinner = true;
      this.DrugsService.delete(this.drug!.id).pipe().subscribe({
        next: () => {
          this.showSpinner = false;
          this.toastr.success(`Success`, `${this.drug!.name} removed`);
          this.actions.emitDrugDelete(this.drug!.id);
        },
        error: (err) => {
          this.toastr.warning(`Failed`, `${this.drug!.name} still persist`);
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
