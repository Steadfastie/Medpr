import { DoctorsService } from 'src/app/services/doctors/doctors.service';
import { Component } from '@angular/core';
import { Doctor } from 'src/app/models/doctor';
import { DoctorsActionsService } from 'src/app/services/doctors/doctors.actions.service';

@Component({
  selector: 'doctors',
  templateUrl: './doctors.component.html',
  styleUrls: ['./doctors.component.scss'],
})
export class DoctorsComponent {
  doctors: Doctor[] = [];

  constructor(private DoctorsService: DoctorsService,
    private actions: DoctorsActionsService) {}

  ngOnInit() {
    this.DoctorsService.getAllDoctors().subscribe(doctors => this.doctors = doctors);
    this.actions.doctorResponseListner().subscribe(doctorFromAction => {
      const presentDoctor = this.doctors.find((presentDoctor) => {
        return presentDoctor.id === doctorFromAction.id;
      })
      if (!presentDoctor) {
        this.doctors.push(doctorFromAction);
      }
      else{
        this.doctors.splice(this.doctors.indexOf(presentDoctor), 1, doctorFromAction);
      }
    });
  }
}
