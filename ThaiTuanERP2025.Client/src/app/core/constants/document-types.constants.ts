export const DOCUMENT_TYPE = {
      EXPENSE_PAYMENT: 'expense-payment',
      OUTGOING_PAYMENT: 'outgoing-payment',
      ADVANCED_PAYMENT: 'advanced-payment',
      USER_AVATAR: 'user-avatar',
      
} as const;

export type DocumentTypeLiteral = typeof DOCUMENT_TYPE[keyof typeof DOCUMENT_TYPE];