import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { provideMondayFirstDateAdapter } from "../../../../../shared/date/provide-monday-first-date-adapter";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { BudgetPeriodService } from "../../../services/budget-period.service";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { filter, firstValueFrom } from "rxjs";
import { BudgetPeriodDto } from "../../../models/budget-period.model";
import { KitSpinnerButtonComponent } from "../../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { HttpErrorResponse } from "@angular/common/http";
import { HasPermissionDirective } from "../../../../../core/auth/auth.directive";
import { ActionMenuOption } from "../../../../../shared/components/kit-action-menu/kit-action-menu.model";
import { MatDialog } from "@angular/material/dialog";
import { EditBudgetPeriodDialogComponent } from "../../../components/edit-budget-period-dialog/edit-budget-period-dialog.component";
import { KitActionMenuComponent } from "../../../../../shared/components/kit-action-menu/kit-action-menu.component";

@Component({
      selector: 'budget-period-panel',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitSpinnerButtonComponent, HasPermissionDirective, KitActionMenuComponent],
      templateUrl: './budget-period.component.html',
      styleUrls: ['./budget-period.component.scss'],
      providers: [...provideMondayFirstDateAdapter() ]
})
export class BudgetPeriodPanelComponent implements OnInit {
      private readonly dialog = inject(MatDialog);
      private readonly formBuilder = inject(FormBuilder);
      private readonly now = new Date();
      private readonly budgetPeriodService = inject(BudgetPeriodService);
      private readonly toastService = inject(ToastService);    
      public budgetPeriods: BudgetPeriodDto[] = [];   
      public isLoading = false;
      public submitting = false;

      constructor() {
            console.log('[DBG][group] HasPermissionDirective symbol (top-level):', HasPermissionDirective);
      }

      autoGenerateForm = this.formBuilder.group({
            year: this.formBuilder.control<number>(this.now.getFullYear(), { nonNullable: true, validators: [ Validators.required ] }),
      });

      trackById(index: number, item: BudgetPeriodDto) { return item.id; }

      ngOnInit(): void {
            this.autoGenerateForm.get('year')?.disable();
            this.loadBudgetPeriodsForYear();
      }

      async loadBudgetPeriodsForYear() {
            console.log('load');
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
                  await firstValueFrom(this.budgetPeriodService.createForYear(year));
                  this.toastService?.successRich('Tạo kỳ ngân sách thành công');
                  await this.loadBudgetPeriodsForYear();
            } catch (err) {
                  if (err instanceof HttpErrorResponse && [401,403,500,0].includes(err.status ?? 0)) {
                        return;
                  }
                  if (err instanceof HttpErrorResponse && err.status === 404) {
                        this.toastService?.warningRich('Không tìm thấy dữ liệu để tạo kỳ ngân sách.');
                  } else if (err instanceof HttpErrorResponse && err.status === 400) {
                        this.toastService?.errorRich(err.error?.message || 'Dữ liệu không hợp lệ.');
                  } else {
                        this.toastService?.errorRich('Tạo kỳ ngân sách thất bại.');
                  }
                  console.error('[autoGenerateForYear] Lỗi:', err);
            } finally {
                  this.submitting = false;
            }
      }

      buildBudgetPeriodActions(period: BudgetPeriodDto): ActionMenuOption[] {
            return [
                  { label: 'Chỉnh sửa kỳ ngân sách', action: () => this.openEditBudgetPeriodDialog(period ) },
            ]
      }

      openEditBudgetPeriodDialog(period: BudgetPeriodDto) {
            this.dialog.open(EditBudgetPeriodDialogComponent, { data: period })
                  .afterClosed()
                  .pipe(filter(isSuccess => !!isSuccess))
                  .subscribe(() => this.loadBudgetPeriodsForYear());
      }
}