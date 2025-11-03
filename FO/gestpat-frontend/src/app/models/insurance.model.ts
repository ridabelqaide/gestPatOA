export interface Insurance {
  id?: string;
  company: string;
  type: string;
  amount: number;
  startDate: Date;
  enginId: string;
  endDate: Date;
  matricule?: string;
}
