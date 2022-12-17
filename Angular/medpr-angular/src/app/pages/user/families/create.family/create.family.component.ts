import { ToastrService } from 'ngx-toastr';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { Family } from 'src/app/models/family';
import { FamiliesService } from 'src/app/services/families/families.service';
import { FamiliesActionsService } from 'src/app/services/families/families.actions.service';

@Component({
  selector: 'create-family',
  templateUrl: './create.family.component.html',
  styleUrls: ['./create.family.component.scss']
})
export class CreateFamilyComponent implements OnInit {
  @Output() deselect = new EventEmitter<void>();
  showSpinner: boolean = false;
  errorMessage?: string;

  constructor(private fb: FormBuilder,
    private familiesService: FamiliesService,
    private toastr: ToastrService,
    private actions: FamiliesActionsService) { }

  ngOnInit(): void {
  }

  familyForm = this.fb.group({
    surname: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(15)]],
  });

  submit(){
    if (!this.showSpinner && this.familyForm.valid){
      this.showSpinner = true;
      const family: Family = {
        id: Guid.createEmpty().toString(),
        surname: this.familyForm.value.surname!,
      };
      this.familiesService.create(family).pipe().subscribe({
        next: (family) => {
          this.showSpinner = false;
          this.actions.emitFamilyResponse(family);
          this.toastr.success(`${family.surname} is now in the app`, `Created`);
          this.deselect.emit();
        },
        error: (err) => {
          this.showSpinner = false;
          console.log(err);
          this.toastr.error(`${family.surname} is still in your dreams`, `Failed`);
          this.errorMessage = "Could not create family";
        },
      });
    }
  }

  cancel() {
    if(!this.showSpinner){
      this.familyForm.reset();
    }
  }

  closeCreate(){
    if (!this.showSpinner){
      this.deselect.emit();
    }
  }
}
