import { CommonModule } from "@angular/common";
import { Component, effect, inject } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { usePaymentDetail } from "../../../composables/use-payment-detail";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { OutgoingBankAccountOptionStore } from "../../../options/outgoing-bank-account-option.store";
import { KitDropdownComponent, KitDropdownOption } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { KitFileUploaderComponent } from "../../../../../shared/components/kit-file-uploader/kit-file-uploader.component";
import { UploadItem } from "../../../../../shared/components/kit-file-uploader/upload-item.model";
import { MoneyFormatDirective } from "../../../../../shared/directives/money/money-format.directive";
import { Kit404PageComponent } from "../../../../../shared/components/kit-404-page/kit-404-page.component";
import { KitLoadingSpinnerComponent } from "../../../../../shared/components/kit-loading-spinner/kit-loading-spinner.component";
import { provideMondayFirstDateAdapter } from "../../../../../shared/date/provide-monday-first-date-adapter";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { OutgoingPaymentService } from "../../../services/outgoing-payment.service";
import { first, firstValueFrom } from "rxjs";
import { OutgoingPaymentRequest } from "../../../models/outgoing-payment.model";

@Component({
      selector: 'outgoing-payment-request',
      templateUrl: './outgoing-payment-request.component.html',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitDropdownComponent, KitFileUploaderComponent, MoneyFormatDirective, Kit404PageComponent, KitLoadingSpinnerComponent, MatDatepickerModule,],
      styleUrls: ['./outgoing-payment-request.component.scss'],
      providers: [...provideMondayFirstDateAdapter()]
})
export class OutgoingPaymentRequestComponent {
      private route = inject(ActivatedRoute);
      private formBuilder = inject(FormBuilder);
      private OBAccountOptionsStore = inject(OutgoingBankAccountOptionStore);
      OBAccountOptions = this.OBAccountOptionsStore.options$;
      private outgoingPaymentService = inject(OutgoingPaymentService);

      private paymentLogic = usePaymentDetail();
      loading = this.paymentLogic.isLoading;
      err = this.paymentLogic.error;
      paymentDetail = this.paymentLogic.paymentDetail;

      public submitting = false;
      public submitted = false;
      public errorMessages: string[] = [];

      private toast = inject(ToastService);

      public readonly uploadMeta = {
            module: 'expense',
            entity: 'outgoing-payment-attachment',
            entityId: undefined as string | undefined,
            isPublic: false
      }
      public uploads: UploadItem[] = [];

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required] }),
            description: this.formBuilder.control<string>(''),
            bankName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            accountNumber: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            beneficiaryName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            outgoingAmount: this.formBuilder.nonNullable.control<number>(0, { validators: [Validators.required, Validators.min(1)] }),
            followerIds: this.formBuilder.nonNullable.control<string[]>([]),
            expensePaymentId: this.formBuilder.nonNullable.control<string>('', { validators: [Validators.required] }),
            outgoingBankAccountId: this.formBuilder.nonNullable.control<string>('', { validators: [Validators.required] }),
            dueDate: this.formBuilder.nonNullable.control<Date>(new Date(), { validators: [Validators.required] }),
            supplierId: this.formBuilder.control<string | null>(null),
            employeeId: this.formBuilder.control<string | null>(null),
      });
      
      private autoPatchEffect = effect(() => {
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
                        // employeeId: detail.employeeId || null,
                  });
            }
      });
      
      onOBAccountSelected(opt: KitDropdownOption) {
            this.form.patchValue({ outgoingBankAccountId: opt.id });
      }

      ngOnInit() {
            const id = this.route.snapshot.paramMap.get('id');
            if (id) this.paymentLogic.load(id);
      }

      async submit(): Promise<void> {
            this.submitted = true;
            
            if (this.form.invalid) {
                  this.form.markAllAsTouched();

                  // 🔍 In ra danh sách field lỗi + loại lỗi
                  const invalidControls = Object.entries(this.form.controls)
                        .filter(([_, control]) => control.invalid)
                        .map(([name, control]) => ({
                              field: name,
                              errors: control.errors
                        }));

                  console.group('⚠️ Form invalid');
                  console.table(invalidControls);
                  console.groupEnd();

                  // Scroll đến control đầu tiên bị lỗi
                  const firstInvalidControl = document.querySelector('.ng-invalid[formControlName]') as HTMLElement;
                  if (firstInvalidControl) {
                        firstInvalidControl.scrollIntoView({ behavior: 'smooth', block: 'center' });
                        firstInvalidControl.focus();
                  }

                  this.toast.warningRich('Vui lòng kiểm tra và điền đầy đủ các thông tin bắt buộc.');
                  return;
            }

            this.submitting = true;

            try {
                  const payload = this.form.getRawValue() as OutgoingPaymentRequest;
                  const result = await firstValueFrom(this.outgoingPaymentService.create(payload));
                  this.toast.successRich("Tạo yêu cầu khoản tiền ra thành công");
                  return;
            } catch(error) {
                  this.toast.errorRich("Có lỗi xảy ra, vui lòng thử lại");
                  console.error(error);
            } finally {
                  this.submitting = false;
            }
      }

      refresh() {
            this.paymentLogic.refresh();
      }

}