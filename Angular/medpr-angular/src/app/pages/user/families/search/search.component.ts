import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged, fromEvent, map, Observable, startWith, Subject, switchMap } from 'rxjs';
import { Family } from 'src/app/models/family';
import { FamiliesActionsService } from 'src/app/services/families/families.actions.service';
import { FamiliesService } from 'src/app/services/families/families.service';

@Component({
  selector: 'family-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
  families?: Family[];
  creationSelected: boolean = false;

  constructor(private fb: FormBuilder,
    private familiesService: FamiliesService,
    private familiesActions: FamiliesActionsService) {
   }

  ngOnInit(): void {
    this.searchForm.controls['substring'].valueChanges.pipe(
        map(event => typeof event === 'string' ? event : ''),
        debounceTime(400),
        distinctUntilChanged()
    ).subscribe((substring: string) => {
      if (substring != '' && this.searchForm.valid) {
        this.familiesService.getFamilyBySubstring(substring)
          .pipe()
          .subscribe((families: Family[]) => {
            this.families = families;
          })
      }
    });
  }

  searchForm = this.fb.group({
    substring: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(15)]]
  });

  selectToggle(){
    this.creationSelected = !this.creationSelected;
  }

  close(){
    this.searchForm.reset();
    this.searchForm.controls["substring"].setErrors(null);
    this.families = [];
  }
}
