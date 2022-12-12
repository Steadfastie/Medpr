import { ToastrService } from 'ngx-toastr';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { Vaccine } from 'src/app/models/vaccine';
import { VaccinesService } from 'src/app/services/vaccines/vaccines.service';
import { VaccinesActionsService } from 'src/app/services/vaccines/vaccines.actions.service';

@Component({
  selector: 'create-vaccine',
  templateUrl: './create.vaccine.component.html',
  styleUrls: ['./create.vaccine.component.scss']
})
export class CreateVaccineComponent implements OnInit {
  @Output() deselect = new EventEmitter<void>();
  showSpinner: boolean = false;
  errorMessage?: string;

  constructor(private fb: FormBuilder,
    private vaccinesService: VaccinesService,
    private toastr: ToastrService,
    private actions: VaccinesActionsService) { }

  ngOnInit(): void {
  }

  vaccineForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
    reason: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    price: ['', [Validators.required, Validators.min(0)]],
  });

  submit(){
    if (!this.showSpinner && this.vaccineForm.valid){
      this.showSpinner = true;
      const vaccine: Vaccine = {
        id: Guid.createEmpty().toString(),
        name: this.vaccineForm.value.name!,
        reason: this.vaccineForm.value.reason!,
        price: Number(this.vaccineForm.value.price!)
      };
      this.vaccinesService.create(vaccine).pipe().subscribe({
        next: (vaccine) => {
          this.showSpinner = false;
          this.actions.emitVaccineResponse(vaccine);
          this.toastr.success(`Created`,`${vaccine.name} is now in the app`);
          this.deselect.emit();
        },
        error: (err) => {
          this.showSpinner = false;
          console.log(err);
          this.toastr.success(`Failed`,`${vaccine.name} is still in your dreams`);
          this.errorMessage = "Could not create vaccine";
        },
      });
    }
  }

  cancel() {
    if(!this.showSpinner){
      this.vaccineForm.reset();
    }
  }

  closeCreate(){
    if (!this.showSpinner){
      this.deselect.emit();
    }
  }
}
