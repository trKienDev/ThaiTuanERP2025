import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { provideMondayFirstDateAdapter } from "../../../../../shared/date/provide-monday-first-date-adapter";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { BudgetPeriodRequest } from "../../../models/budget-period.model";
import { catchError, firstValueFrom, throwError } from "rxjs";
import { BudgetPeriodFacade } from "../../../facades/budget-period.facade";

@Component({
      selector: 'budget-period-panel',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MatDatepickerModule],
      templateUrl: './budget-period.component.html',
      providers: [...provideMondayFirstDateAdapter() ]
})
export class BudgetPeriodPanelComponent {
      private formBuilder = inject(FormBuilder);
      private toast = inject(ToastService);
      public submitting = false;
      private now = new Date();
      private budgetPeriodFacade = inject(BudgetPeriodFacade);

      form = this.formBuilder.group({
            month: this.formBuilder.control<number>(this.now.getMonth() + 1, { nonNullable: true, validators: [ Validators.required ] }),
            year: this.formBuilder.control<number>(this.now.getFullYear(), { nonNullable: true, validators: [ Validators.required ] }),
      });

      async onSubmit(): Promise<void> {
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warning('Vui lòng điền đầy đủ thông tin');
                  return;
            }
            this.submitting = true;

            try {
                  const payload: BudgetPeriodRequest = this.form.getRawValue() as BudgetPeriodRequest;
                  
                  this.budgetPeriodFacade.create(payload).pipe(
                        catchError(err => {
                              this.toast.errorRich("Tạo ngân sách thất bại");
                              this.submitting = false;
                              return throwError(() => err);
                        })
                  ).subscribe((created) => {
                        this.toast.successRich("Tạo kỳ ngân sách thành công")
                  });
            } catch(error) {
                  const messages = handleHttpError(error);
                  this.toast.errorRich("Thêm kỳ ngân sách thất bại");
            } finally {
                  this.submitting = false;
            }
      }

}