import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { MatInputModule } from "@angular/material/input";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { provideMondayFirstDateAdapter } from "../../../../../../shared/date/provide-monday-first-date-adapter";
import { DateAdapter } from "@angular/material/core";
import { MatButtonModule } from "@angular/material/button";
import { FormBuilder, Validators, FormsModule, ReactiveFormsModule } from "@angular/forms";
import { InvoiceService } from "../../../../services/invoice.service";
import { firstValueFrom } from "rxjs";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { handleHttpError } from "../../../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../../../shared/components/toast/toast.service";
import { FileService } from "../../../../../../shared/services/file.service";

@Component({
      selector: 'mini-invoice-request-dialog',
      standalone: true,
      imports: [CommonModule, MatInputModule, MatDatepickerModule, MatButtonModule, FormsModule, 
            ReactiveFormsModule, MatSnackBarModule
      ],
      templateUrl: './mini-invoice-request-dialog.component.html',
      styleUrl: './mini-invoice-request-dialog.component.scss',
      providers: [...provideMondayFirstDateAdapter() ]
})
export class MiniInvoiceRequestDialogComponent {
      private ref = inject(MatDialogRef<MiniInvoiceRequestDialogComponent>);
      private adapter = inject<DateAdapter<Date>>(DateAdapter as any);
      private formBuilder = inject(FormBuilder);
      private fileService = inject(FileService);
      private invoiceService = inject(InvoiceService);
      private toast = inject(ToastService);

      // ============ UI state ============
      submitting = false;
      fileName = '';
      pendingFile: File | null = null;

      // ============ Form (tối thiểu) ============
      // Match đúng tên field của InvoiceRequestPageComponent
      form = this.formBuilder.group({
            invoiceName: ['', [ Validators.required, Validators.maxLength(256) ]],
            invoiceNumber: ['', [ Validators.required, Validators.maxLength(64) ]],
            issueDate: [ null as unknown as string | null, [ Validators.required ]],
            sellerTaxCode: ['', [ Validators.required, Validators.maxLength(32)] ],
            // Optional:
            paymentDate: [null as unknown as string | null],
            sellerName: [''],
            sellerAddress: [''],
            buyerName: [''],
            buyerTaxCode: [''],
            buyerAddress: [''],
      });

      // ============ Handlers ============
      onFileSelected(event: Event): void {
            const file = (event.target as HTMLInputElement).files?.[0] ?? null;
            this.pendingFile = file;
            this.fileName = file?.name ?? '';
      }

      // Chuẩn hóa date về 'yyyy-MM-dd'
      private toIsoDate(d: unknown): string | null {
            if (!d) return null;
            // nếu input là chuỗi 'yyyy-MM-dd' thì giữ nguyên
            if (typeof d === 'string' && /^\d{4}-\d{2}-\d{2}$/.test(d)) return d;
            try {
                  const date = new Date(d as any);
                  if (isNaN(date.getTime())) return null;
                  const yyyy = date.getFullYear();
                  const mm = String(date.getMonth() + 1).padStart(2, '0');
                  const dd = String(date.getDate()).padStart(2, '0');
                  return `${yyyy}-${mm}-${dd}`;
            } catch {
                  return null;
            }
      }

      async save(): Promise<void> {
            if (this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warning('Vui lòng điền đầy đủ thông tin bắt buộc');
                  return;
            }
            this.submitting = true;

            try {
                  const v = this.form.value;

                  // Map body giống invoice-request-page.component (draftBody)
                  const draftBody = {
                        invoiceNumber: v.invoiceNumber!,           // required
                        invoiceName: v.invoiceName!,              // required
                        issueDate: this.toIsoDate(v.issueDate)!,  // required ISO string
                        paymentDate: this.toIsoDate(v.paymentDate) || null,
                        sellerName: v.sellerName || "",
                        sellerTaxCode: v.sellerTaxCode!,          // required
                        sellerAddress: v.sellerAddress || null,
                        buyerName: v.buyerName || null,
                        buyerTaxCode: v.buyerTaxCode || null,
                        buyerAddress: v.buyerAddress || null,
                  };

                  // 1) Tạo draft
                  const draft = await firstValueFrom(this.invoiceService.createDraft(draftBody));
                  const invoiceId = (draft as any).id;

                  // 2) Nếu chọn file → upload rồi replace main
                  if (this.pendingFile) {
                        const up = await firstValueFrom(this.fileService.uploadFile(this.pendingFile, 'Expense', 'Invoice', invoiceId, false));
                        const fileId = (up as any).id ?? (up as any).data?.id;
                        await firstValueFrom(this.invoiceService.replaceMainFile(invoiceId, { newFileId: fileId }));
                  }

                  // 3) mini-dialog không có invoiceLines, nên kết thúc ở đây
                  // Trả về kết quả để caller có thể refresh list hoặc mở trang chi tiết
                  this.toast.successRich('Lưu hóa đơn thành công', 'Thành công', { duration: 3000 });
                  this.close({ success: true, invoiceId });
            } catch (err) {
                  const msgs = handleHttpError(err);
                  this.toast.errorRich(msgs, 'Lỗi'); 
                  this.close({ success: false, error: err });
            } finally {
                  this.submitting = false;
            }
      }

      close(result?: unknown) {
            this.ref.close(result);
      }
}