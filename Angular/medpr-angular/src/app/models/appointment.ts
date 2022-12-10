import { Doctor } from "./doctor";
import { User } from "./user";

export interface Appointment {
  id: string;
  date: string;
  place: string;
  userId: string;
  doctorId: string;
  doctor?: Doctor;
  user?: User;
}
