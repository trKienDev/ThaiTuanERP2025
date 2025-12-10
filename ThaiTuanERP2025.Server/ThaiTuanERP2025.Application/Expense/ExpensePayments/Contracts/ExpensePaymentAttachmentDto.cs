using ThaiTuanERP2025.Application.Files.Contracts;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts
{
        public sealed record ExpensePaymentAttachmentDto
        {
                public Guid ExpensePaymentId { get; init; }
                public Guid StoredFileId { get; init; }
                public StoredFileMetadataDto StoredFile { get; set; }
        }
}
