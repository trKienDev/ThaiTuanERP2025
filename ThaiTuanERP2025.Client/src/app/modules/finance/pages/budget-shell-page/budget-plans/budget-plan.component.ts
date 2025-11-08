import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { BudgetPlanRequestDialogComponent } from "../../../components/budget-plan-request-dialog/budget-plan-request-dialog.component";
import { BudgetPlanApproversDialogComponent } from "../../../components/budget-plan-apporvers-dilaog/budget-plan-approvers-dialog.component";

@Component({
      selector: 'budget-plan-panel',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './budget-plan.component.html',
})
export class BudgetPlanPanelComponent {
      private readonly dialog = inject(MatDialog);
      private readonly toastSer = inject(ToastService);

      openBudgetPlanRequestDialog() {
            const dialogRef = this.dialog.open(BudgetPlanRequestDialogComponent, {});
            dialogRef.afterClosed().subscribe();
      }

      openBudgetPlanApproversDialog() {
            const dialogRef = this.dialog.open(BudgetPlanApproversDialogComponent, {})
            dialogRef.afterClosed().subscribe();
      }
}