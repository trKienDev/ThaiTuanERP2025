import { CommonModule } from "@angular/common";
import { Component, effect, inject, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { usePaymentDetail } from "../../../composables/use-payment-detail";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { OutgoingBankAccountOptionStore } from "../../../options/outgoing-bank-account-option.store";
import { KitDropdownComponent } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { KitFileUploaderComponent } from "../../../../../shared/components/kit-file-uploader/kit-file-uploader.component";
import { MoneyFormatDirective } from "../../../../../shared/directives/money/money-format.directive";
import { Kit404PageComponent } from "../../../../../shared/components/kit-404-page/kit-404-page.component";
import { KitLoadingSpinnerComponent } from "../../../../../shared/components/kit-loading-spinner/kit-loading-spinner.component";
import { provideMondayFirstDateAdapter } from "../../../../../shared/date/provide-monday-first-date-adapter";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { firstValueFrom } from "rxjs";
import { OutgoingPaymentPayload } from "../../../models/outgoing-payment.model";
import { KitSpinnerButtonComponent } from "../../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { KitOverlaySpinnerComponent } from "../../../../../shared/components/kit-overlay-spinner/kit-overlay-spinner.component";
import { OutgoingPaymentApiService } from "../../../services/api/outgoing-payment.service";
import { UploadItem } from "../../../../../shared/components/kit-file-uploader/upload-item.model";
import { UserOptionStore } from "../../../../account/options/user-dropdown.option";
import { MatDialog } from "@angular/material/dialog";
import { ExpensePaymentDetailDialogComponent } from "../../../components/dialogs/expense-payment-detail-dialog/expense-payment-detail-dialog.component";
import { HttpErrorHandlerService } from "../../../../../core/services/http-errror-handler.service";
import { ExpensePaymentItemsTableComponent } from "../../../components/tables/expense-payment-items-table/expense-payment-items-table.component";
import { ExpensePaymentItemLookupDto } from "../../../models/expense-payment-item.model";
import { FilePreviewService } from "../../../../files/file-preview.service";
import { OutgoingPaymentStatusPipe } from "../../../pipes/outgoing-payment-status.pipe";
import { OutgoingPaymentsTableComponent } from "../../../components/tables/outgoing-payments-table/outgoing-payments-table.component";
import { maxRemainingValidator } from "../../../../../shared/validators/max-value.validator";

@Component({
      selector: 'outgoing-payment-request',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitDropdownComponent, MoneyFormatDirective, Kit404PageComponent, KitLoadingSpinnerComponent, MatDatepickerModule, KitSpinnerButtonComponent, KitOverlaySpinnerComponent, KitFileUploaderComponent, ExpensePaymentItemsTableComponent, OutgoingPaymentStatusPipe, OutgoingPaymentsTableComponent],
      styleUrls: ['./outgoing-payment-request.component.scss'],
      templateUrl: './outgoing-payment-request.component.html',
      providers: [...provideMondayFirstDateAdapter()]
})
export class OutgoingPaymentRequestComponent implements OnInit {
      private readonly route = inject(ActivatedRoute);
      private readonly formBuilder = inject(FormBuilder);
      private readonly outgoingPaymentApi = inject(OutgoingPaymentApiService);
      private readonly dialog = inject(MatDialog)
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly filePreview = inject(FilePreviewService);

      userOptions = inject(UserOptionStore).option$;
      outgoingBankOptions = inject(OutgoingBankAccountOptionStore).options$;

      private readonly paymentLogic = usePaymentDetail();
      
      loading = this.paymentLogic.isLoading;
      err = this.paymentLogic.error;
      paymentDetail = this.paymentLogic.paymentDetail;

      public submitting = false;
      public submitted = false;
      public showErrors = false;


      public readonly uploadMeta = {
            module: 'expense',
            entity: 'outgoing-payment-attachment',
            entityId: undefined as string | undefined,
            isPublic: false
      }
      public uploads: UploadItem[] = [];

      private readonly toast = inject(ToastService);

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required] }),
            description: this.formBuilder.control<string>(''),
            bankName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            accountNumber: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            beneficiaryName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            outgoingAmount: this.formBuilder.nonNullable.control<number | null>(0, { validators: [Validators.required, Validators.min(1000)] }),
            followerIds: this.formBuilder.nonNullable.control<string[]>([]),
            expensePaymentId: this.formBuilder.nonNullable.control<string>('', { validators: [Validators.required] }),
            outgoingBankAccountId: this.formBuilder.nonNullable.control<string | null>(null, { validators: [Validators.required] }),
            dueAt: this.formBuilder.nonNullable.control<Date>(new Date(), { validators: [Validators.required] }),
            supplierId: this.formBuilder.control<string | null>(null),
            employeeId: this.formBuilder.control<string | null>(null),
      });
      
      private readonly autoPatchEffect = effect(() => {
            const isLoading = this.loading();
            const detail = this.paymentDetail();

            // 🔹 Khi đang loading → disable form để tránh user nhập
            if (isLoading) {
                  this.form.disable({ emitEvent: false });
                  return;
            }

            // 🔹 Khi load xong (hết loading)
            this.form.enable({ emitEvent: false });

            // Nếu có dữ liệu thì patch form
            if (detail) {
                  this.form.patchValue({
                        bankName: detail.bankName,
                        accountNumber: detail.accountNumber,
                        beneficiaryName: detail.beneficiaryName,
                        expensePaymentId: detail.id,
                        supplierId: detail.supplierId || null,
                        followerIds: detail.followers?.map(f => f.id) || [],
                        // employeeId: detail.employeeId || null,
                  });
            }
      });

      ngOnInit() {
            const id = this.route.snapshot.paramMap.get('id');
            if (id) this.paymentLogic.load(id);
      }

      openExpensePaymentDetailDialog(paymentId: string) { 
            this.dialog.open(ExpensePaymentDetailDialogComponent, { data: paymentId });
      }

      async submit(): Promise<void> {
            this.showErrors = true;
            
            if (this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich('Vui lòng kiểm tra và điền đầy đủ các thông tin bắt buộc.');
                  return;
            }

            try {
                  this.submitting = true;

                  const payload = this.form.getRawValue() as OutgoingPaymentPayload;
                  console.log('Submitting payload:', payload);
                  await firstValueFrom(this.outgoingPaymentApi.create(payload));

                  this.showErrors = false;
                  this.toast.successRich("Tạo yêu cầu khoản tiền ra thành công");
                  return;
            } catch(error) {
                  this.httpErrorHandler.handle(error, 'Không thể tạo yêu cầu khoản chi');
            } finally {
                  this.submitting = false;
            }
      }

      refresh() {
            this.paymentLogic.refresh();
      }
      get isBusy() {
            return this.submitting;
      }

      previewInvoice(item: ExpensePaymentItemLookupDto) {
            if (!item.invoiceFile) {
                  this.toast.errorRich("Không tìm thấy hóa đơn");
                  return;
            }

            this.filePreview.previewStoredFile({
                  fileId: item.invoiceFile.fileId ?? '',
                  objectKey: item.invoiceFile.objectKey ?? '',
                  fileName: item.invoiceFile.fileName ?? 'invoice',
            });
      }

      private readonly updateOutgoingAmountValidator = effect(() => {
            const detail = this.paymentDetail();

            if (!detail) return; // chưa có dữ liệu

            const remaining = detail.remainingOutgoingAmount ?? 0;

            const control = this.form.get('outgoingAmount');

            if (control) {
                  control.setValidators([
                        Validators.required,
                        Validators.min(1),
                        maxRemainingValidator(remaining)
                  ]);

                  control.updateValueAndValidity({ emitEvent: false });
            }
      });
}