import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { provideMondayFirstDateAdapter } from "../../../../../shared/date/provide-monday-first-date-adapter";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { BudgetPeriodService } from "../../../services/budget-period.service";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { catchError, firstValueFrom, of } from "rxjs";
import { BudgetPeriodDto } from "../../../models/budget-period.model";
import { KitSpinnerButtonComponent } from "../../../../../shared/components/kit-spinner-button/kit-spinner-button.component";

@Component({
      selector: 'budget-period-panel',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitSpinnerButtonComponent],
      templateUrl: './budget-period.component.html',
      styleUrls: ['./budget-period.component.scss'],
      providers: [...provideMondayFirstDateAdapter() ]
})
export class BudgetPeriodPanelComponent implements OnInit {
      private formBuilder = inject(FormBuilder);
      private now = new Date();
      private budgetPeriodService = inject(BudgetPeriodService);
      private toastService = inject(ToastService);    
      public budgetPeriods: BudgetPeriodDto[] = [];   
      public isLoading = false;
      public submitting = false;

      autoGenerateForm = this.formBuilder.group({
            year: this.formBuilder.control<number>(this.now.getFullYear(), { nonNullable: true, validators: [ Validators.required ] }),
      });

      ngOnInit(): void {
            this.autoGenerateForm.get('year')?.disable();
            this.loadBudgetPeriodsForYear();
      }

      async loadBudgetPeriodsForYear() {
            this.isLoading = true;
            try {
                  const year = this.autoGenerateForm.getRawValue().year;
                  this.budgetPeriods = await firstValueFrom(this.budgetPeriodService.getForYear(year));
            } catch (err) {
                  console.error(err);
                  this.toastService?.errorRich('Không thể tải danh sách kỳ ngân sách');
            } finally {
                  this.isLoading = false;
            }
      }

      async autoGenerateForYear() {
            if (this.autoGenerateForm.invalid) return;
            const year = this.autoGenerateForm.getRawValue().year;

            this.submitting = true;
            try {
                  const result = await firstValueFrom(
                        this.budgetPeriodService.autoGenerateForYear(year).pipe(
                              catchError(err => {
                                    this.toastService?.errorRich('Tạo kỳ ngân sách thất bại');
                                    console.error(err);
                                    return of(null);
                              })
                        )
                  );

                  if (result) {
                        this.toastService?.successRich('Tạo kỳ ngân sách thành công');
                        await this.loadBudgetPeriodsForYear();
                  }
            } finally {
                  this.submitting = false;
            }
      }
}