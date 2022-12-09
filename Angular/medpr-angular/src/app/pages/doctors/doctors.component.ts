import { DoctorsService } from 'src/app/services/doctors/doctors.service';
import { Component } from '@angular/core';
import { Doctor } from 'src/app/models/doctor';

@Component({
  selector: 'doctors',
  templateUrl: './doctors.component.html',
  styleUrls: ['./doctors.component.scss'],
})
export class DoctorsComponent {
  doctors: Doctor[] = [];

  constructor(private DoctorsService: DoctorsService) {}

  ngOnInit() {
    this.DoctorsService.getAllDoctors().subscribe(doctors => this.doctors = doctors);
  }
}
