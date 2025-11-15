import { BudgetPlanDto, BudgetPlanReview, BudgetPlansByDepartmentDto } from './../../models/budget-plan.model';
import { CommonModule } from "@angular/common";
import { Component, Inject, inject } from "@angular/core";
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { MoneyFormatDirective } from '../../../../shared/directives/money/money-format.directive';
import { firstValueFrom } from 'rxjs';
import { BudgetPlanService } from '../../services/budget-plan.service';
import { HttpErrorHandlerService } from '../../../../core/services/http-errror-handler.service';
import { ToastService } from '../../../../shared/components/kit-toast-alert/kit-toast-alert.service';

@Component({
      selector: 'budget-plan-detail-dilaog',
      standalone: true,
      imports: [CommonModule, FormsModule, KitSpinnerButtonComponent, MoneyFormatDirective],
      templateUrl: './budget-plan-detail-dialog.component.html'
})
export class BudgetPlanDetailDialogComponent {
      private readonly dialog = inject(MatDialogRef<BudgetPlanDetailDialogComponent>);
      private readonly budgetPlanService = inject(BudgetPlanService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly toast = inject(ToastService);

      public title: string = '';
      public deparmtentName: string = '';
      public canReview: boolean = false;
      public submitting: boolean = false;

      public budgetPlans: BudgetPlanReview[] = [];

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: BudgetPlansByDepartmentDto
      ) {             
            this.budgetPlans = data.budgetPlans.map(p => ({
                  ...p,
                  isEditing: false,
                  editedAmount: p.amount
            }));
            this.deparmtentName = data.departmentName;
            this.canReview = data.budgetPlans.some(p => p.canReview);
      }

      trackById(index: number, item: BudgetPlanDto) { return item.id; }


      // ===== EDIT =====
      enableEdit(plan: any) {
            plan.isEditing = true;
            plan.editedAmount = plan.amount;
      }

      cancelEdit(plan: any) {
            plan.isEditing = false;
            plan.editedAmount = plan.amount;
      }

      async savePlan(plan: any) {
            this.submitting = true;

            try {
                  await firstValueFrom(this.budgetPlanService.updateAmount(plan.id, plan.editedAmount));

                  plan.amount = plan.editedAmount;
                  this.toast.successRich("Đã sửa ngân sách");

            } catch(error) {
                  this.httpErrorHandler.handle(error, "Sửa tiền thất bại");
            }
            finally {
                  plan.isEditing = false;
                  this.submitting = false;
            }
      }

      close(result: boolean = false) {
            this.dialog.close(result);
      }
}