import { BudgetPlanDto } from './../../models/budget-plan.model';
import { CommonModule } from "@angular/common";
import { Component, Inject, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";

@Component({
      selector: 'budget-plan-detail-dialog',
      standalone: true,
      imports: [CommonModule, KitSpinnerButtonComponent],
      templateUrl: './budget-plan-detail-dialog.component.html'
})
export class BudgetPlanDetailDialogComponent {
      private readonly dialogRef = inject(MatDialogRef<BudgetPlanDetailDialogComponent>);
      public canReview: boolean = false;
      public submitting: boolean = false;
      public budgetPlan: BudgetPlanDto;

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: BudgetPlanDto
      ) {             
            this.budgetPlan = data;
            this.canReview = data.canReview;
      }



      close(isSuccess: boolean) {
            this.dialogRef.close(isSuccess);
      }
}