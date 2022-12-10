import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Appointment } from 'src/app/models/appointment';
import { Doctor } from 'src/app/models/doctor';
import { DoctorsService } from 'src/app/services/doctors/doctors.service';

@Component({
  selector: 'appointment-card',
  templateUrl: './appointment.card.component.html',
  styleUrls: ['./appointment.card.component.scss']
})
export class AppointmentCardComponent implements OnInit {
  @Input() appointment?: Appointment;
  doctor?: Doctor;
  selected: boolean;

  constructor(private doctorsService: DoctorsService,
    private changeDetectorRef: ChangeDetectorRef) {
    this.selected = false;
  }

  ngOnInit(): void {
    this.resetDoctor();
  }

  selectToggle(){
    this.selected = !this.selected;
    this.resetDoctor();
  }

  resetDoctor(){
    if(this.appointment != undefined && this.appointment.doctor) {
      this.selectDoctor(this.appointment.doctor.id)
    }
    else{
      this.doctor = undefined;
    }
  }

  selectDoctor(doctorId:string){
    if(doctorId !== ''){
      this.doctorsService.getDoctorById(doctorId).subscribe(
        (doctor) => {
          console.log('success'),
          this.doctor = doctor
        }
      );
    }
    else{
      this.doctor = undefined;
    }
    this.changeDetectorRef.detectChanges();
  }

  createFromBlank(){
    this.selectToggle();
  }
}
