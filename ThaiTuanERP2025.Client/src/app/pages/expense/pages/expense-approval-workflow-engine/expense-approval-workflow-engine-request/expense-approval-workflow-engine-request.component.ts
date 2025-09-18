import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ConnectedPosition, OverlayModule } from "@angular/cdk/overlay";
import { MatDialog } from "@angular/material/dialog";
import { ApprovalStepRequestDialog } from "./approval-step-request/approval-step-request.component";
import { ApprovalStepRequest } from "../../../models/expense-approval-workflow.model";

@Component({
      selector: 'expense-approval-workflow-engine-request',
      standalone: true,
      imports: [CommonModule, OverlayModule ],
      templateUrl: './expense-approval-workflow-engine-request.component.html',
      styleUrl: './expense-approval-workflow-engine-request.component.scss',
})
export class ExpenseApprovalWorkflowEngineRequest {
      private readonly dialog = inject(MatDialog);

      steps: ApprovalStepRequest[] = [];
      
      openMenu: number | null = null;
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
            const dialogRef = this.dialog.open(ApprovalStepRequestDialog);

            dialogRef.afterClosed().subscribe((result?: { isSuccess?: boolean, step?: Omit<ApprovalStepRequest, 'order'> }) => {
                  console.log('result: ', result);
                  if (result?.isSuccess === true && result.step ) {
                        const order = this.steps.length + 1;
                        const newStep: ApprovalStepRequest = {
                              ...result.step,
                              order
                        };

                        this.steps.push(newStep);
                        console.log('steps: ', this.steps);
                  }
            });
      }

      editStep(i: number): void {
            const current = this.steps[i];
            const dialogRef = this.dialog.open(ApprovalStepRequestDialog, {
                  data: { step: current }
            });
            dialogRef.afterClosed().subscribe((result?: { isSuccess?: boolean, step?: ApprovalStepRequest}) => {
                  if(result?.isSuccess && result.step) {
                        const updated = { ...result.step, order: current.order };
                        this.steps[i] = updated;
                  }
            })
      }

      // Reorder helper
      private recomputeOrders(): void {
            this.steps.forEach((s, idx) => s.order = idx + 1);
      }
      moveUp(i: number): void {
            if(i <= 0) return;
            [this.steps[i-1], this.steps[i]] = [this.steps[i], this.steps[i - 1]];
      }
      moveDown(i: number): void {
            if(i >= this.steps.length - 1) return;
            [this.steps[i], this.steps[i + 1]] = [this.steps[i + 1], this.steps[i]];
            this.recomputeOrders();
      }
      removeStep(i: number): void {
            this.steps.splice(i, 1);
            this.recomputeOrders();
      }
}