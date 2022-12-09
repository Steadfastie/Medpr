import { ToastrService } from 'ngx-toastr';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { Drug } from 'src/app/models/drug';
import { DrugsService } from 'src/app/services/drugs/drugs.service';

@Component({
  selector: 'create-drug',
  templateUrl: './create.drug.component.html',
  styleUrls: ['./create.drug.component.scss']
})
export class CreateDrugComponent implements OnInit {
  @Input() randomDrug?: Drug;
  @Output() deselect = new EventEmitter<void>();
  showSpinner: boolean = false;
  errorMessage?: string;

  constructor(private fb: FormBuilder,
    private drugsService: DrugsService,
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
        next: () => {
          this.showSpinner = false;
          window.location.reload();
        },
        error: (err) => {
          this.showSpinner = false;
          console.log(err);
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
