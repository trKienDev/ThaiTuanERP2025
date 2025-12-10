import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { BudgetApproverFacade } from "../../facades/budget-approver.facade";
import { AvatarUrlPipe } from "../../../../shared/pipes/avatar-url.pipe";
import { BudgetApproverDto } from "../../models/budget-approvers.model";
import { ActionMenuOption } from "../../../../shared/components/kit-action-menu/kit-action-menu.model";
import { KitActionMenuComponent } from "../../../../shared/components/kit-action-menu/kit-action-menu.component";
import { BudgetPlanApproversDialogComponent } from "../budget-apporver-request-dilaog/budget-approver-request-dialog.component";
import { KitDropdownOption } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { HasPermissionDirective } from "../../../../core/auth/auth.directive";
      
@Component({
      selector: 'list-budget-approves-dialog',
      standalone: true,
      imports: [CommonModule, AvatarUrlPipe, KitActionMenuComponent, HasPermissionDirective],
      templateUrl: './list-budget-approvers-dialog.component.html'
})
export class ListBudgetApproversDialogComponent {
      private readonly dialog = inject(MatDialogRef<ListBudgetApproversDialogComponent>);
      private readonly matDialog = inject(MatDialog);
      private readonly toast = inject(ToastService);
      private readonly budgetApproverFacade = inject(BudgetApproverFacade);
      budgetApprovers$ = this.budgetApproverFacade.budgetApprovers$;

      budgetApproverDepartmentOptions: KitDropdownOption[] = []


      buildApproverActions(approver: BudgetApproverDto): ActionMenuOption[] {
            return [
                  { label: 'Cập nhật user phê duyệt', action: () => this.updateBudgetApprover(approver )}
            ]
      }

      updateBudgetApprover(approver: BudgetApproverDto): void {
            const dialogRef = this.matDialog.open(BudgetPlanApproversDialogComponent, { data: approver });

            dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                  this.toast.successRich('Cập nhật user phê duyệt thành công');
                  // Optionally reload list:
                  this.budgetApproverFacade.refresh();
                  }
            });
      }

      close(): void {
            this.dialog.close();
      }
}