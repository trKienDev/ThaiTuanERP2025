import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { OverlayModule } from "@angular/cdk/overlay";
import { MatDialog } from "@angular/material/dialog";
import { ApprovalStepRequestDialog } from "./approval-step-request/approval-step-request.component";
import { ActionMenuOption } from "../../../../../shared/components/kit-action-menu/kit-action-menu.model";
import { KitActionMenuComponent } from "../../../../../shared/components/kit-action-menu/kit-action-menu.component";
import {  ApprovalStepTemplateRequest } from "../../../models/approval-step-template.model";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { ApprovalWorkflowTemplateService } from "../../../services/approval-workflow-template.service";
import { SaveApprovalWorkflowTemplateComponent } from "./save-approval-workflow-template/save-approval-workflow-template.component";

@Component({
      selector: 'expense-approval-workflow-engine-request',
      standalone: true,
      imports: [CommonModule, OverlayModule, KitActionMenuComponent, ReactiveFormsModule],
      templateUrl: './expense-approval-workflow-engine-request.component.html',
      styleUrl: './expense-approval-workflow-engine-request.component.scss',
})
export class ExpenseApprovalWorkflowEngineRequest {
      private readonly dialog = inject(MatDialog);
      private readonly formBuilder = inject(FormBuilder);
      private readonly toastService = inject(ToastService);
      private readonly approvalWorkflowTemplateService = inject(ApprovalWorkflowTemplateService);

      private static readonly END = -1; // sential cho nút add cuối cùng

      steps: ApprovalStepTemplateRequest[] = [];

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            version: this.formBuilder.control<number>(0, { nonNullable: true }),
      })
      
      buildStepActtions(index: number): ActionMenuOption[] {
            return [
                  { label: '⚙️ Sửa', action: () => this.editStep(index) },
                  { label: '⬅️ Về trước', action: () => this.moveUp(index), disabled: index === 0 },
                  { label: '➡️ Về sau', action:() => this.moveDown(index), disabled: index === this.steps.length - 1 },
                  { label: '⛔ Xóa', color: 'red', action: () => this.removeStep(index)},
            ]
      }

      buildAddStepAction(): ActionMenuOption[] {
            return [
                  { label: 'Thêm bước duyệt thông thường', action: () => this.openApprovalStepRequestDialog('standard') },
                  { label: 'Thêm bước duyệt theo điều kiện', action: () => this.openApprovalStepRequestDialog('condition') }
            ]
      }

      openApprovalStepRequestDialog(approverMode?: 'standard' | 'condition'): void {
            const dialogRef = this.dialog.open(ApprovalStepRequestDialog, {
                  data: { approverMode }
            });

            dialogRef.afterClosed().subscribe((result?: { isSuccess?: boolean, step?: Omit<ApprovalStepTemplateRequest, 'order'> }) => {
                  if (result?.isSuccess === true && result.step ) {
                        const order = this.steps.length + 1;
                        const newStep: ApprovalStepTemplateRequest = {
                              ...result.step,
                              order
                        };

                        this.steps.push(newStep);
                  }
            });
      }

      editStep(i: number): void {
            const current = this.steps[i];
            const dialogRef = this.dialog.open(ApprovalStepRequestDialog, {
                  data: { step: current }
            });
            dialogRef.afterClosed().subscribe((result?: { isSuccess?: boolean, step?: ApprovalStepTemplateRequest}) => {
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
            this.recomputeOrders();
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

      openSaveApprovalWorkflowTemplate(): void {
            const dialogRef = this.dialog.open(SaveApprovalWorkflowTemplateComponent, {
                  data: { steps: this.steps }
            });
      }
}