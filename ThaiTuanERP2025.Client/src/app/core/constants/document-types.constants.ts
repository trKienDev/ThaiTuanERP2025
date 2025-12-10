export const DOCUMENT_TYPE = {
      EXPENSE_PAYMENT: 'ExpensePayment',
      OUTGOING_PAYMENT: 'OutgoingPayment',
      ADVANCED_PAYMENT: 'AdvancedPayment',
      
} as const;

export type DocumentTypeLiteral = typeof DOCUMENT_TYPE[keyof typeof DOCUMENT_TYPE];