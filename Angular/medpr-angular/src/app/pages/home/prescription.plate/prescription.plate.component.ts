import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Prescription } from 'src/app/models/prescription';

@Component({
  selector: 'prescription-plate',
  templateUrl: './prescription.plate.component.html',
  styleUrls: ['./prescription.plate.component.scss']
})
export class PrescriptionPlateComponent implements OnInit {
  @Input() prescription?: Prescription;
  userName?: string;

  constructor(private router: Router) { }

  ngOnInit(): void {
    if (this.prescription?.user?.fullName) {
      this.userName = this.prescription?.user?.fullName;
    } else {
      let dogIndex = this.prescription?.user?.login.lastIndexOf('@');
      this.userName = this.prescription?.user?.login.substring(0, dogIndex);
    }
  }

  goToDetails(){
    this.router.navigate([`prescriptions/${this.prescription?.id}`]);
  }

}
