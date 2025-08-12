export interface BankAccountDto {
      id: string;
      accountNumber: string;
      bankName: string;
      accountHolder?: string | null;
      ownerName?: string | null;
      isActive: boolean;
      createdDate: string;
}

export interface CreateBankAccountCommand {
      accountNumber: string;
      bankName: string;
      accountHolder?: string | null;
      ownerName?: string | null;
}

export interface UpdateBankAccountCommand extends CreateBankAccountCommand {
      id: string;
}