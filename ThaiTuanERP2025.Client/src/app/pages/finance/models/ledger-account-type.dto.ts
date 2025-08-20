import { TaxDto } from "./tax.dto";

export interface LedgerAccountTypeDto {
      id: string;
      code: string;
      name: string;
      isActive: boolean;
      description?: string;
}
export type CreateLedgerAccountTypeRequest = Omit<LedgerAccountTypeDto, 'id' | 'isActive'> & { isActive?: boolean }
export type UpdateLedgerAccountTypeRequest = Partial<Omit<LedgerAccountTypeDto, 'id'>>;