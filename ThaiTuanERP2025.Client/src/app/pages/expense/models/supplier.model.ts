export interface SupplierDto {
      id: string;
      name: string;
      taxCode?: string | null;
      isActive: boolean;
}
export interface CreateSupplierRequest {
      name: string,
      taxCode?: string | null;
      isActive: boolean;
}
export interface UpdateSupplierRequest {
      id: string;
      name: string;
      taxCode?: string | null;
      isActive: boolean;
}