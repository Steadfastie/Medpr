import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { Drug } from 'src/app/models/drug';
import { DrugsService } from './../../services/drugs/drugs.service';


@Component({
  selector: 'edit-drug',
  templateUrl: './edit.drug.component.html',
  styleUrls: ['./edit.drug.component.scss']
})
export class EditDrugComponent implements OnInit {
  @Input() drug?: Drug;
  @Output() deselect = new EventEmitter<void>();

  drugForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
    pharmGroup: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    price: ['', [Validators.required, Validators.min(1)]],
  });

  constructor(private fb: FormBuilder, private DrugsService: DrugsService) { }

  ngOnInit(): void {
    this.initialize();
  }

  modifiedDrug: Drug = {
    id: this.drug?.id!,
    name: this.drugForm.value.name!,
    pharmGroup: this.drugForm.value.pharmGroup!,
    price: Number(this.drugForm.value.price!)
  }

  edit(){
    this.DrugsService.patch(this.modifiedDrug);
  }

  remove(){
    this.DrugsService.delete(this.modifiedDrug.id.toString());
  }

  initialize() {
    if (this.drug) {
      this.drugForm.setValue({
        name: this.drug.name,
        pharmGroup: this.drug.pharmGroup,
        price: this.drug.price.toString()
      })
    }
  }

  closeEdit(){
    this.deselect.emit();
  }
}
