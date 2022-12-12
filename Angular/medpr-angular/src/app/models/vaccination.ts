import { User } from "./user";
import { Vaccine } from "./vaccine";

export interface Vaccination {
  id: string;
  date: string;
  daysOfProtection: number;
  userId: string;
  vaccineId: string;
  vaccine?: Vaccine;
  user?: User;
}
