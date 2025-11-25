export interface SupplierDto {
      id: string;
      name: string;
      taxCode?: string | null;
      isActive: boolean;
}
export interface SupplierPayload {
      name: string,
      taxCode?: string | null;
}
