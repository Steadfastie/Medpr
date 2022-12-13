import { PrescriptionsService } from 'src/app/services/prescriptions/prescriptions.service';
import { Component } from '@angular/core';
import { Prescription } from 'src/app/models/prescription';
import { ActivatedRoute } from '@angular/router';
import { PrescriptionsActionsService } from 'src/app/services/prescriptions/prescriptions.actions.service';

@Component({
  selector: 'prescriptions',
  templateUrl: './prescriptions.component.html',
  styleUrls: ['./prescriptions.component.scss']
})
export class PrescriptionsComponent {
  prescriptions: Prescription[] = [];
  idProvided: boolean = false;

  constructor(
    private prescriptionsService: PrescriptionsService,
    private actions: PrescriptionsActionsService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    const prescriptionId = this.route.snapshot.paramMap.get('id');
    if (prescriptionId == null) {
      this.prescriptionsService.getAllPrescriptions()
        .subscribe((prescriptions) => (this.prescriptions = prescriptions));
    } else {
      this.prescriptionsService.getPrescriptionById(prescriptionId)
        .subscribe((prescription) => {
          this.prescriptions.push(prescription),
          this.idProvided = true;
        });
    }

    this.actions.prescriptionResponseListner().subscribe((prescriptionFromAction) => {
        if (this.prescriptions) {
          const presentPrescription = this.prescriptions.find((presentPrescription) => {
              return presentPrescription.id === prescriptionFromAction.id;
            }
          );
          if (!presentPrescription) {
            this.prescriptions.push(prescriptionFromAction);
          } else {
            this.prescriptions.splice(
              this.prescriptions.indexOf(presentPrescription), 1, prescriptionFromAction
            );
          }
        } else {
          this.prescriptions = [];
          this.prescriptions.push(prescriptionFromAction);
        }
      });

    this.actions.prescriptionDeleteListner().subscribe(prescriptionId => {
      const presentPrescription = this.prescriptions.find((presentPrescription) => {
        return presentPrescription.id === prescriptionId;
      })
      this.prescriptions.splice(this.prescriptions.indexOf(presentPrescription!), 1);
    });
  }
}
