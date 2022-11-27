import { Guid } from "guid-typescript";

export interface Drug {
  id: Guid;
  name: string;
  pharmGroup: string;
  price: number;
}
