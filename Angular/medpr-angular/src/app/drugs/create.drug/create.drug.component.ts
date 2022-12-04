import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { Drug } from 'src/app/models/drug';
import { DrugsService } from './../../services/drugs/drugs.service';

@Component({
  selector: 'create-drug',
  templateUrl: './create.drug.component.html',
  styleUrls: ['./create.drug.component.scss']
})
export class CreateDrugComponent implements OnInit {
  @Input() randomDrug?: Drug;
  @Output() deselect = new EventEmitter<void>();

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

  constructor(private fb: FormBuilder, private DrugsService: DrugsService) { }

  drug: Drug = {
    id: Guid.createEmpty(),
    name: this.drugForm.value.name!,
    pharmGroup: this.drugForm.value.pharmGroup!,
    price: Number(this.drugForm.value.price!)
  }

  submit(){
    this.DrugsService.create(this.drug);
  }

  cancel() {
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

  closeCreate(){
    this.deselect.emit();
  }
}
