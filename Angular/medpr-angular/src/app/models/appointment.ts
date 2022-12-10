import { Doctor } from "./doctor";
import { User } from "./user";

export interface Appointment {
  id: string;
  date: Date;
  place: string;
  userId: string;
  doctorId: string;
}
