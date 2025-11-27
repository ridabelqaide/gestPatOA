import { Auto } from './auto.model';
import { Official } from './Official.model';
export interface Affectation {
  id?: string;
  startDate?: Date;
  endDate?: Date;
  currentKm?: number;
  endKm?: number;
  object?: string;
  details?: string;
  enginId?: string;
  engin?: Auto;
  officialId?: string;
  official?: Official;

}
