export interface OutgoingPayment {
      name: string;
      description?: string;
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
      totalOutgoingAmount: number;
      outgoingAmount: number;
      followerIds: string[];
}

export interface OutgoingPaymentRequest extends OutgoingPayment {
      expensePaymentId: string;
      outgoingBankAccountId: string;
      postingDate: Date;
}