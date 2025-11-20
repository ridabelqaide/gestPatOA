import { Insurance } from "./insurance.model";
import { EnginType } from "./engin-type.model";
export interface Auto {
  id?: string;
  matricule: string;
  genre: string;
  enginTypeCode: string;
  enginTypeName?: string;        
  enginTypeDescription?: string;  marque: string;
  model: string;
  modeCarburant: string;
  acquisition: string;
  miseCirculationDate: string;
  etat: string;
  th: number;
  tj: number;
  insurance?: Insurance;
}
