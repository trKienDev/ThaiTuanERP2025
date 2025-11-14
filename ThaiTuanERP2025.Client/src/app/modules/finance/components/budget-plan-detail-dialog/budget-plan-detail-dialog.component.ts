import { BudgetPlanDto, BudgetPlansByDepartmentDto } from './../../models/budget-plan.model';
import { CommonModule } from "@angular/common";
import { Component, Inject, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";

@Component({
      selector: 'budget-plan-detail-dilaog',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './budget-plan-detail-dialog.component.html'
})
export class BudgetPlanDetailDialogComponent {
      private readonly dialog = inject(MatDialogRef<BudgetPlanDetailDialogComponent>);

      public title: string = '';
      public budgetPlans: BudgetPlanDto[] = [];

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: BudgetPlansByDepartmentDto
      ) { this.budgetPlans = data.budgetPlans; }

      trackById(index: number, item: BudgetPlanDto) { return item.id; }

      close(result: boolean = false) {
            this.dialog.close(result);
      }
}