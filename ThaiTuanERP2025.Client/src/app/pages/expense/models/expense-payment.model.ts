export type PayeeType = 'supplier' | 'employee';

export interface ExpensePaymentItemRequest {
      itemName: string;               // bắt buộc
      invoiceId?: string | null;      // có thể null nếu chưa gắn hoá đơn
      quantity: number;               // >= 1 (số nguyên)
      unitPrice: number;              // >= 0
      taxRate: number;                // ví dụ 0.1 = 10%
      amount: number;                 // quantity * unitPrice (server có thể tự tính lại)
      taxAmount: number;              // gợi ý hoặc người dùng override
      totalWithTax: number;           // amount + taxAmount
      budgetCodeId?: string | null;   // id mã ngân sách
      cashoutCodeId?: string | null;  // id mã dòng tiền ra (nếu có)
}

export interface ExpensePaymentAttachment {
      objectKey: string;              // khóa truy xuất file (bắt buộc để tải/ngầm định)
      fileId?: string;                // id bản ghi StoredFile (nếu backend có)
      fileName?: string;              // tên hiển thị
      size?: number;                  // byte
      url?: string;                   // link truy cập (nếu public / có signed-url)
}

export interface CreateExpensePaymentRequest {
      // Thông tin chung
      name: string;                   // bắt buộc
      payeeType: PayeeType;           // 'supplier' | 'employee'
      supplierId?: string | null;     // chỉ có khi payeeType='supplier'

      // Tài khoản thụ hưởng (khi trả NCC thì auto-fill theo NCC; trả nhân viên có thể để trống/nhập tay tuỳ rule)
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;

      // Hạn thanh toán và cờ nhập kho
      paymentDate: string;            // ISO date string, khuyến nghị 'YYYY-MM-DD'
      hasGoodsReceipt: boolean;

      // Chi tiết hạng mục
      items: ExpensePaymentItemRequest[];  // tối thiểu 1 dòng

      // Tổng (server vẫn nên recalculation để đảm bảo toàn vẹn)
      totalAmount: number;
      totalTax: number;
      totalWithTax: number;

      // Bổ sung (không thấy lưu trong form hiện tại nhưng thường cần)
      followerIds?: string[];         // danh sách user id theo dõi
      attachments?: ExpensePaymentAttachment[]; // file đính kèm
}