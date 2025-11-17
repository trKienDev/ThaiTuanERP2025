import { BudgetPlanDetailDto, BudgetPlanDto } from './../../models/budget-plan.model';
import { CommonModule } from "@angular/common";
import { Component, Inject, inject} from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { FormsModule } from '@angular/forms';
import { MoneyFormatDirective } from "../../../../shared/directives/money/money-format.directive";
import { HttpErrorHandlerService } from '../../../../core/services/http-errror-handler.service';
import { firstValueFrom } from 'rxjs';
import { ToastService } from '../../../../shared/components/kit-toast-alert/kit-toast-alert.service';
import { BudgetPlanEventsService } from '../../services/events/budget-plan-event.service';
import { BudgetPlanApiService } from '../../services/api/budget-plan-api.service';
import { KitFlipCountdownComponent } from '../../../../shared/components/kit-flip-countdown/kit-flip-countdown.component';

@Component({
      selector: 'budget-plan-detail-dialog',
      standalone: true,
      imports: [CommonModule, KitSpinnerButtonComponent, FormsModule, MoneyFormatDirective, KitFlipCountdownComponent ],
      templateUrl: './budget-plan-detail-dialog.component.html'
})
export class BudgetPlanDetailDialogComponent {
      private readonly dialogRef = inject(MatDialogRef<BudgetPlanDetailDialogComponent>);
      public canReview: boolean = false;
      public submitting: boolean = false;
      public budgetPlan: BudgetPlanDto;
      private readonly budgetPlanApi = inject(BudgetPlanApiService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly toast = inject(ToastService);
      private readonly budgetPlanEvents = inject(BudgetPlanEventsService);

      editingIndex: number | null = null; 
      editAmountValue: number | null = null;

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: BudgetPlanDto
      ) {             
            this.budgetPlan = data;
            this.canReview = data.canReview;
      }

      close(isSuccess: boolean = false) {
            this.dialogRef.close(isSuccess);
      }

      // === EDITING ===
      startEdit(i: number, detail: BudgetPlanDetailDto) {
            this.editingIndex = i;
            this.editAmountValue = detail.amount;
      }
      cancelEdit() {
            this.editingIndex = null;
            this.editAmountValue = null;
      }
      async saveEdit(detail: BudgetPlanDetailDto) {
            if (this.editAmountValue == null || this.editAmountValue < 0) return;

            try {
                  this.submitting = true;
                  await firstValueFrom(this.budgetPlanApi.updateDetailAmount(detail.id, this.editAmountValue));
                  this.toast.successRich("Cập nhật số tiền thành công");
                  this.cancelEdit();
                  await this.reloadBudgetPlan();
                  this.budgetPlanEvents.notifyUpdated();       
            } catch(error) {
                  this.httpErrorHandler.handle(error, 'cập nhật số tiền');
            } finally {
                  this.submitting = false;
            }
      }

      // === MARK REVIEW ===
      async markReview() {
            try {
                  this.submitting = true;
                  await firstValueFrom(this.budgetPlanApi.markReview(this.budgetPlan.id));
                  this.toast.successRich("Xem xét thành công");
                  this.budgetPlanEvents.notifyUpdated();
                  this.close(true);
            } catch(error) {
                  this.httpErrorHandler.handle(error, "Xem xét thất bại");
            } finally { 
                  this.submitting = false;
            }
      }

      // === HELPERS ===
      isExpired(dueAt: string): boolean {
            console.log('dueAt: ', dueAt);
            return new Date(dueAt).getTime() <= Date.now();
      }
      getSecondsRemaining(dueAt: string): number {
            const sec = Math.max(0, Math.floor((new Date(dueAt).getTime() - Date.now()) / 1000));
            return sec
      }

      private async reloadBudgetPlan() {
            try {
                  const updated = await firstValueFrom(this.budgetPlanApi.getById(this.budgetPlan.id));
                  this.budgetPlan = updated;
            } catch (err) {
                  this.httpErrorHandler.handle(err, 'tải lại kế hoạch ngân sách');
            }
      }
}