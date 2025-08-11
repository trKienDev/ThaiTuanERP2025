export interface BankAccountDto {
      id: string;
      accountNumber: string;
      bankName: string;
      accountHolder?: string | null;
      departmentId?: string | null;
      departmentName?: string | null;
      customerName?: string | null;
      isActive: boolean;
      createdDate: string;
}

export interface CreateBankAccountCommand {
      accountNumber: string;
      bankName: string;
      accountHolder?: string | null;
      departmentId?: string | null; // XOR với customerName
      customerName?: string | null;
}

export interface UpdateBankAccountCommand extends CreateBankAccountCommand {
      id: string;
}