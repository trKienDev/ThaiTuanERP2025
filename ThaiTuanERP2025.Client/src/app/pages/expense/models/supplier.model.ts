export interface SupplierDto {
      id: string;
      name: string;
      taxCode?: string | null;
      isActive: boolean;
}
export interface SupplierRequest {
      name: string,
      taxCode?: string | null;
      isActive: boolean;
}
