import { Router } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { Appointment } from 'src/app/models/appointment';

@Component({
  selector: 'appointment-plate',
  templateUrl: './appointment.plate.component.html',
  styleUrls: ['./appointment.plate.component.scss']
})
export class AppointmentPlateComponent implements OnInit {
  @Input() appointment?: Appointment;
  userName?: string;

  constructor(private router: Router,
    ) { }

  ngOnInit(): void {
    if (this.appointment?.user?.fullName) {
      this.userName = this.appointment?.user?.fullName;
    } else {
      let dogIndex = this.appointment?.user?.login.lastIndexOf('@');
      this.userName = this.appointment?.user?.login.substring(0, dogIndex);
    }
  }

  goToDetails(){
    this.router.navigate([`appointments/${this.appointment?.id}`]);
  }
}
