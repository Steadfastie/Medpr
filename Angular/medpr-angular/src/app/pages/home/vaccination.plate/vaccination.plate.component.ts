import { formatDate } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Vaccination } from 'src/app/models/vaccination';

@Component({
  selector: 'vaccination-plate',
  templateUrl: './vaccination.plate.component.html',
  styleUrls: ['./vaccination.plate.component.scss']
})
export class VaccinationPlateComponent implements OnInit {
  @Input() vaccination?: Vaccination;
  userName?: string;
  endDate?: string;

  constructor(private router: Router,
    ) { }

  ngOnInit(): void {
    if (this.vaccination?.user?.fullName) {
      this.userName = this.vaccination?.user?.fullName;
    } else {
      let dogIndex = this.vaccination?.user?.login.lastIndexOf('@');
      this.userName = this.vaccination?.user?.login.substring(0, dogIndex);
    }

    let endDate = new Date(this.vaccination!.date);
    endDate.setDate(endDate.getDate() + this.vaccination!.daysOfProtection);
    this.endDate = formatDate(endDate, 'fullDate', 'en-US');
  }

  goToDetails(){
    this.router.navigate([`vaccinations/${this.vaccination?.id}`]);
  }
}
