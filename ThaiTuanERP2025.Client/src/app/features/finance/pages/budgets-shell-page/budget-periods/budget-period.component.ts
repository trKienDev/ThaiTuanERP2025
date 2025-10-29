import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { provideMondayFirstDateAdapter } from "../../../../../shared/date/provide-monday-first-date-adapter";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { BudgetPeriodRequest } from "../../../models/budget-period.model";
import { catchError, firstValueFrom, throwError } from "rxjs";
import { BudgetPeriodFacade } from "../../../facades/budget-period.facade";
import { KitDropdownComponent, KitDropdownOption } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";

@Component({
      selector: 'budget-period-panel',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MatDatepickerModule, KitDropdownComponent ],
      templateUrl: './budget-period.component.html',
      styleUrls: ['./budget-period.component.scss'],
      providers: [...provideMondayFirstDateAdapter() ]
})
export class BudgetPeriodPanelComponent implements OnInit {
      private formBuilder = inject(FormBuilder);
      private toast = inject(ToastService);
      public submitting = false;
      private now = new Date();
      private budgetPeriodFacade = inject(BudgetPeriodFacade);

      form = this.formBuilder.group({
            month: this.formBuilder.control<string>(String(this.now.getMonth() + 1), { nonNullable: true, validators: [ Validators.required ] }),
            year: this.formBuilder.control<number>(this.now.getFullYear(), { nonNullable: true, validators: [ Validators.required ] }),
      });

      months = Array.from({ length: 12 }, (_, i) => ({
            id: String(i + 1),
            label: `Tháng ${i + 1}`,
      }));

      ngOnInit(): void {
            this.form.get('year')?.disable();
      }

      onMonthSelected(opt: KitDropdownOption) {
            this.form.patchValue({ month: opt.id })
      }

      async onSubmit(): Promise<void> {
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warning('Vui lòng điền đầy đủ thông tin');
                  return;
            }
            this.submitting = true;

            try {
                  const payload = {
                        month: Number(this.form.value.month),
                        year: this.form.value.year!,
                  } as BudgetPeriodRequest;
                  
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