import { ToastrService } from 'ngx-toastr';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { Drug } from 'src/app/models/drug';
import { DrugsService } from 'src/app/services/drugs/drugs.service';
import { DrugsActionsService } from 'src/app/services/drugs/drugs.actions.service';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'create-drug',
  templateUrl: './create.drug.component.html',
  styleUrls: ['./create.drug.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class CreateDrugComponent implements OnInit {
  @Input() randomDrug?: Drug;
  @Output() deselect = new EventEmitter<void>();
  showSpinner: boolean = false;
  errorMessage?: string;

  constructor(private fb: FormBuilder,
    private drugsService: DrugsService,
    private actions: DrugsActionsService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    if (this.randomDrug) {
      this.drugForm.setValue({
        name: this.randomDrug.name,
        pharmGroup: this.randomDrug.pharmGroup,
        price: this.randomDrug.price.toString()
      })
    }
  }

  drugForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
    pharmGroup: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    price: ['', [Validators.required, Validators.min(1)]],
  });

  submit(){
    if (!this.showSpinner){
      this.showSpinner = true;
      const drug: Drug = {
        id: Guid.createEmpty().toString(),
        name: this.drugForm.value.name!,
        pharmGroup: this.drugForm.value.pharmGroup!,
        price: Number(this.drugForm.value.price!)
      };
      this.drugsService.create(drug).pipe().subscribe({
        next: (drug) => {
          this.showSpinner = false;
          this.actions.emitDrugResponse(drug);
          this.toastr.success(`Created`,`${drug.name} is now in the app`);
          this.deselect.emit();
        },
        error: (err) => {
          this.showSpinner = false;
          console.log(err);
          this.toastr.error(`Failed`,`${drug.name} is still in your dreams`);
          this.errorMessage = "Could not create drug";
        },
      });
    }
  }

  cancel() {
    if(!this.showSpinner){
      if (this.randomDrug){
        this.drugForm.setValue({
          name: this.randomDrug.name,
          pharmGroup: this.randomDrug.pharmGroup,
          price: this.randomDrug.price.toString()
        })
      }
      else{
        this.drugForm.reset();
      }
    }
  }

  closeCreate(){
    if (!this.showSpinner){
      this.deselect.emit();
    }
  }
}
