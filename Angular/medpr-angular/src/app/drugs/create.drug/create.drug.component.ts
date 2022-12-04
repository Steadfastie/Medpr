import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { Drug } from 'src/app/models/drug';
import { DrugsService } from './../../services/drugs/drugs.service';

@Component({
  selector: 'create-drug',
  templateUrl: './create.drug.component.html',
  styleUrls: ['./create.drug.component.scss']
})
export class CreateDrugComponent {
  drugForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
    pharmGroup: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    price: ['', [Validators.required, Validators.min(1)]],
  });

  constructor(private fb: FormBuilder, private DrugsService: DrugsService) { }

  drug: Drug = {
    id: Guid.createEmpty(),
    name: this.drugForm.value.name!,
    pharmGroup: this.drugForm.value.pharmGroup!,
    price: Number(this.drugForm.value.price!)
  }

  onSubmit(){
    this.DrugsService.create(this.drug);
  }
}
