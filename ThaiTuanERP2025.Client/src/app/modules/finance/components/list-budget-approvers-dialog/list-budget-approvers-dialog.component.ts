import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { BudgetApproverFacade } from "../../facades/budget-approver.facade";
import { AvatarUrlPipe } from "../../../../shared/pipes/avatar-url.pipe";
import { BudgetApproverDto } from "../../models/budget-approvers.model";
import { ActionMenuOption } from "../../../../shared/components/kit-action-menu/kit-action-menu.model";
import { KitActionMenuComponent } from "../../../../shared/components/kit-action-menu/kit-action-menu.component";

@Component({
      selector: 'list-budget-approves-dialog',
      standalone: true,
      imports: [CommonModule, AvatarUrlPipe, KitActionMenuComponent],
      templateUrl: './list-budget-approvers-dialog.component.html'
})
export class ListBudgetApproversDialogComponent {
      private readonly dialog = inject(MatDialogRef<ListBudgetApproversDialogComponent>);
      private readonly toast = inject(ToastService);
      private readonly budgetApproverFacade = inject(BudgetApproverFacade);
      budgetApprovers$ = this.budgetApproverFacade.budgetApprovers$;

      buildApproverActions(approver: BudgetApproverDto): ActionMenuOption[] {
            return [
                  { label: 'Cập nhật user phê duyệt', }
            ]
      }

      close(): void {
            this.dialog.close();
      }
}