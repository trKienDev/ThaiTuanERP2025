import { CommonModule } from "@angular/common";
import { Component, effect, inject } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { usePaymentDetail } from "../../../composables/use-payment-detail";
import { ExpensePaymentDetailDto } from "../../../models/expense-payment.model";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";

@Component({
      selector: 'outgoing-payment-request',
      templateUrl: './outgoing-payment-request.component.html',
      standalone: true,
      imports: [ CommonModule, ReactiveFormsModule ],
      styleUrls: ['./outgoing-payment-request.component.scss']
})
export class OutgoingPaymentRequestComponent {
      private route = inject(ActivatedRoute);
      private formBuilder = inject(FormBuilder);
      public submitting = false;

      private paymentLogic = usePaymentDetail();
      loading = this.paymentLogic.isLoading;
      err = this.paymentLogic.error;
      paymentDetail = this.paymentLogic.paymentDetail;

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required] }),
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