import { BudgetPlanDto } from './../../../models/budget-plan.model';
import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { BudgetPlanRequestDialogComponent } from "../../../components/budget-plan-request-dialog/budget-plan-request-dialog.component";
import { BudgetPlanApproversDialogComponent } from "../../../components/budget-apporver-request-dilaog/budget-approver-request-dialog.component";
import { ListBudgetApproversDialogComponent } from "../../../components/list-budget-approvers-dialog/list-budget-approvers-dialog.component";
import { HasPermissionDirective } from "../../../../../core/auth/auth.directive";
import { BudgetPlanService } from "../../../services/budget-plan.service";
import { firstValueFrom } from 'rxjs';

@Component({
      selector: 'budget-plan-panel',
      standalone: true,
      imports: [CommonModule, HasPermissionDirective],
      templateUrl: './budget-plan.component.html',
})
export class BudgetPlanPanelComponent implements OnInit {
      private readonly dialog = inject(MatDialog);
      private readonly toastSer = inject(ToastService);
      private readonly budgetPlanService = inject(BudgetPlanService);
      myBudgetPlans: BudgetPlanDto[] = [];

      ngOnInit(): void {
            this.loadMyBudgetPlans();
      }

      trackById(index: number, item: BudgetPlanDto) { return item.id; }

      async loadMyBudgetPlans() {
            this.myBudgetPlans = await firstValueFrom(this.budgetPlanService.getByMyDepartment())
            console.log('CHECK department:', this.myBudgetPlans[0].department);
            console.log('IS UNDEFINED:', this.myBudgetPlans[0].department === undefined);
      }

      openBudgetPlanRequestDialog() {
            const dialogRef = this.dialog.open(BudgetPlanRequestDialogComponent, {});
            dialogRef.afterClosed().subscribe();
      }

      openListBudgetApproverDialog() {
            const dialogRef = this.dialog.open(ListBudgetApproversDialogComponent, {})
            dialogRef.afterClosed().subscribe();
      }

      openBudgetPlanApproversDialog() {
            const dialogRef = this.dialog.open(BudgetPlanApproversDialogComponent, {})
            dialogRef.afterClosed().subscribe();
      }
}