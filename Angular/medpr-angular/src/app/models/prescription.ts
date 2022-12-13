import { Doctor } from "./doctor";
import { Drug } from "./drug";
import { User } from "./user";

export interface Prescription {
  id: string;
  date: string;
  endDate: string;
  dose: number;
  userId: string;
  user?: User;
  drugId: string;
  drug?: Drug;
  doctorId: string;
  doctor?: Doctor;
}
