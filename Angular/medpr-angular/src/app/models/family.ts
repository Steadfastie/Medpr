import { Member } from "./member";

export interface Family {
  id: string;
  surname: string;
  creator?: string;
  members?: Member[];
}
