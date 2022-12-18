import { Prescription } from 'src/app/models/prescription';
import { Appointment } from "./appointment";
import { Vaccination } from "./vaccination";

export interface Feed {
  appointments: Appointment[];
  vaccinations: Vaccination[];
  prescriptions: Prescription[];
}
