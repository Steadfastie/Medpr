import { User } from "./user";

export interface Member {
  id: string;
  isAdmin: boolean;
  userId: string;
  familyId: string;
  user?: User;
}
