import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ExpenseApprovalWorkflowService } from "../../../services/expense-approval-workflow.service";
import { ConnectedPosition, OverlayModule } from "@angular/cdk/overlay";

@Component({
      selector: 'expense-approval-workflow-engine-request',
      standalone: true,
      imports: [CommonModule, OverlayModule ],
      templateUrl: './expense-approval-workflow-engine-request.component.html',
      styleUrl: './expense-approval-workflow-engine-request.component.scss',
})
export class ExpenseApprovalWorkflowEngineRequest {
      private readonly approvalWorkflowService = inject(ExpenseApprovalWorkflowService);

      steps = [{ title: 'Duyệt: Mặc định' }];
      
      openMenu: number | null = null;

      addApproverBlockAfter(index: number) {
            this.steps.splice(index + 1, 0, { title: 'Bước duyệt mới'});
            this.openMenu = null;
      }

      private static readonly END = -1; // sential cho nút add cuối cùng
      stepMenuOpenIndex: number | null = null;
      stepMenuOverlayPosition: ConnectedPosition[] = [
            { originX: 'center', originY: 'bottom', overlayX: 'center', overlayY: 'top', offsetY: 8 },
            { originX: 'end', originY: 'top',    overlayX: 'end',    overlayY: 'bottom', offsetY: -8 },
      ]
      toggleStepMenu(i: number, ev: MouseEvent) {
            ev.stopPropagation();
            this.stepMenuOpenIndex = (this.stepMenuOpenIndex === i) ? null : i;
      }
      onMenuClosed() {
            this.stepMenuOpenIndex = null;
      }
}