import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ExpenseApprovalWorkflowService } from "../../../services/expense-approval-workflow.service";
import { ConnectedPosition, OverlayModule } from "@angular/cdk/overlay";
import { MatDialog } from "@angular/material/dialog";
import { ApprovalStepRequestDialog } from "./approval-step-request/approval-step-request.component";

@Component({
      selector: 'expense-approval-workflow-engine-request',
      standalone: true,
      imports: [CommonModule, OverlayModule ],
      templateUrl: './expense-approval-workflow-engine-request.component.html',
      styleUrl: './expense-approval-workflow-engine-request.component.scss',
})
export class ExpenseApprovalWorkflowEngineRequest {
      private readonly approvalWorkflowService = inject(ExpenseApprovalWorkflowService);
      private readonly dialog = inject(MatDialog);

      steps = [{ title: 'Duyệt: Mặc định' }];
      
      openMenu: number | null = null;

      addApproverBlockAfter(index: number) {
            this.steps.splice(index + 1, 0, { title: 'Bước duyệt mới'});
            this.openMenu = null;
      }

      stepMenuOpenIndex: number | null = null;
      stepMenuOverlayPosition: ConnectedPosition[] = [
            { originX: 'center', originY: 'bottom', overlayX: 'center', overlayY: 'top', offsetY: 8 },
            { originX: 'end', originY: 'top',    overlayX: 'end',    overlayY: 'bottom', offsetY: -8 },
      ]
      toggleStepMenu(i: number, ev: MouseEvent) {
            ev.stopPropagation();
            this.stepMenuOpenIndex = (this.stepMenuOpenIndex === i) ? null : i;
      }
      onStepMenuClosed() {
            this.stepMenuOpenIndex = null;
      }

      private static readonly END = -1; // sential cho nút add cuối cùng
      addStepMenuOpenIndex: number | null = null;
      addStepMenuOverlayPosition: ConnectedPosition[] = [
            { originX: 'center', originY: 'bottom', overlayX: 'center', overlayY: 'top', offsetY: 8 },
            { originX: 'end', originY: 'top',    overlayX: 'end',    overlayY: 'bottom', offsetY: -8 },
      ]
      toggleAddStepMenu(i: number, ev: MouseEvent) {
            ev.stopPropagation();
            this.addStepMenuOpenIndex = (this.addStepMenuOpenIndex === i) ? null : i;
      }
      onAddStepMenuClosed() {
            this.addStepMenuOpenIndex = null;
      }

      openApprovalStepRequestDialog(): void {
            const dialogRef = this.dialog.open(ApprovalStepRequestDialog, {
                  width: 'fit-content',
                  height: 'fit-content',
                  maxWidth: '90vw',
                  maxHeight: '80vh',
                  disableClose: true,
            });

            dialogRef.afterClosed().subscribe((result?: { isSuccess?: boolean }) => {
                  if (result?.isSuccess === true ) {
                        
                  }
            });
      }
}