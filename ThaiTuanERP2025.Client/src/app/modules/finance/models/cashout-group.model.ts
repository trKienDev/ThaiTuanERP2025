import { CashoutCodeTreeDto } from "./cashout-code.model";

export interface CashoutGroupDto {
      id: string;
      code: string;
      name: string;
      isActive: boolean;
      description?: string;
}

export interface CashoutGroupTreeDto {
      id: string;
      parentId?: string;
      name: string;
      level: number;
      path?: string;
      description?: string;
}

export interface CashoutGroupTreeWithCodeDto {
      id: string;
      name: string;
      level: number;
      orderNumber: number;
      path: string;
      children: CashoutGroupTreeWithCodeDto[];
      codes: CashoutCodeTreeDto[];

      parentId?: string | null;
      description?: string | null;
}

// ===== Tree =====
export type CashoutTreeNode =
      | ({ type: 'group' } & CashoutGroupTreeWithCodeDto)
      | ({ type: 'code'; parentId: string; level: number } & CashoutCodeTreeDto);

export interface CashoutGroupFlatNode {
      type: 'group';
      id: string;
      name: string;
      level: number;
      parentId?: string | null;
      description?: string | null;
}
export interface CashoutCodeFlatNode {
      type: 'code';
      id: string;
      name: string;
      postingLedgerAccountId: string;
      postingLedgerAccountName: string;
      description?: string;
      level: number;
      parentId: string;   // luôn có group cha
}
// =========

export interface CashoutGroupPayload {
      name: string;
      parentId?: string | null;
      description?: string | null;
}
