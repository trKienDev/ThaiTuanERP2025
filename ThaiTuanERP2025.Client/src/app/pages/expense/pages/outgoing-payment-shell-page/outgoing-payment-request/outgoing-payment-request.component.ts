import { CommonModule } from "@angular/common";
import { Component, effect, inject } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { usePaymentDetail } from "../../../composables/use-payment-detail";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { OutgoingBankAccountOptionStore } from "../../../options/outgoing-bank-account-option.store";
import { KitDropdownComponent } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";

@Component({
      selector: 'outgoing-payment-request',
      templateUrl: './outgoing-payment-request.component.html',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitDropdownComponent],
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

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required] }),
            description: this.formBuilder.control<string>(''),
            bankName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            accountNumber: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            beneficiaryName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            totalAmount: this.formBuilder.nonNullable.control<number>(0),
            totalTax: this.formBuilder.nonNullable.control<number>(0),
            totalWithTax: this.formBuilder.nonNullable.control<number>(0),
            totalOutgoing: this.formBuilder.nonNullable.control<number>(0),
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
                        totalAmount: detail.totalAmount,
                        totalTax: detail.totalTax,
                        totalWithTax: detail.totalWithTax,
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