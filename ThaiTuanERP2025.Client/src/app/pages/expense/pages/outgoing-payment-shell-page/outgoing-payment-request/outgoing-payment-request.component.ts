import { CommonModule } from "@angular/common";
import { Component, effect, inject } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { usePaymentDetail } from "../../../composables/use-payment-detail";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { OutgoingBankAccountOptionStore } from "../../../options/outgoing-bank-account-option.store";
import { KitDropdownComponent } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { KitFileUploaderComponent } from "../../../../../shared/components/kit-file-uploader/kit-file-uploader.component";
import { UploadItem } from "../../../../../shared/components/kit-file-uploader/upload-item.model";
import { MoneyFormatDirective } from "../../../../../shared/directives/money/money-format.directive";


@Component({
      selector: 'outgoing-payment-request',
      templateUrl: './outgoing-payment-request.component.html',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitDropdownComponent, KitFileUploaderComponent, MoneyFormatDirective],
      styleUrls: ['./outgoing-payment-request.component.scss']
})
export class OutgoingPaymentRequestComponent {
      private route = inject(ActivatedRoute);
      private formBuilder = inject(FormBuilder);
      public submitting = false;
      private OBAccountOptionsStore = inject(OutgoingBankAccountOptionStore);
      OBAccountOptions = this.OBAccountOptionsStore.options$;

      private paymentLogic = usePaymentDetail();
      loading = this.paymentLogic.isLoading;
      err = this.paymentLogic.error;
      paymentDetail = this.paymentLogic.paymentDetail;

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
            totalOutgoingAmount: this.formBuilder.nonNullable.control<number>(0),
            outgoingAmount: this.formBuilder.nonNullable.control<number>(0, { validators: [Validators.required, Validators.min(1)] }),
            followerIds: this.formBuilder.nonNullable.control<string[]>([]),
            expensePaymentId: this.formBuilder.nonNullable.control<string>('', { validators: [Validators.required] }),
            outgoingBankAccountId: this.formBuilder.nonNullable.control<string>('', { validators: [Validators.required] }),
            postingDate: this.formBuilder.nonNullable.control<Date>(new Date(), { validators: [Validators.required] }),
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
                        name: `[OUT] ${detail.name}`,
                        bankName: detail.bankName,
                        accountNumber: detail.accountNumber,
                        beneficiaryName: detail.beneficiaryName,
                  });
            }
      });

      ngOnInit() {
            const id = this.route.snapshot.paramMap.get('id');
            if (id) this.paymentLogic.load(id);
      }

      refresh() {
            this.paymentLogic.refresh();
      }

}