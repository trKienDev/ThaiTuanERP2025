export interface OutgoingBankAccount {
      name: string;
      bankName: string;
      accountNumber: string;
      ownerName: string;
}

export interface OutgoingBankAccountDto extends OutgoingBankAccount {
      id: string;
      isActive: boolean;
}

export interface OutgoingBankAccountRequest extends OutgoingBankAccount {}
