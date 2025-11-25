import { ExpenseApproveMode } from '../../../models/expense-step-template.model';
import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ExpenseStepTemplatePayload } from "../../../models/expense-step-template.model";
import { FormBuilder, Validators } from "@angular/forms";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { ExpenseStepTemplateApiService } from "../../../services/expense-step-template.service";
import { ActionMenuOption } from "../../../../../shared/components/kit-action-menu/kit-action-menu.model";
import { KitActionMenuComponent } from "../../../../../shared/components/kit-action-menu/kit-action-menu.component";
import { MatDialog } from "@angular/material/dialog";
import { SaveExpenseWorkflowTemplateDialogComponent } from "../../../components/save-expense-workflow-template-dialog/save-expense-workflow-template-dialog.component";
import { ActivatedRoute, Router } from '@angular/router';
import { ExpenseWorkflowStepDialogComponent } from '../../../components/expense-workflow-step-dialog/expense-workflow-step-dialog.component';

@Component({
      selector: 'expense-workflow-request-panel',
      standalone: true,
      imports: [CommonModule, KitActionMenuComponent],
      templateUrl: './create-expense-workflow-panel.component.html',
      styleUrl: './create-expense-workflow-panel.component.scss'
})
export class CreateExpenseWorkflowPanel {
      private readonly dialog = inject(MatDialog);
      private readonly formBuilder = inject(FormBuilder);
      private readonly toast = inject(ToastService);
      private readonly expenseStepTemplateApi = inject(ExpenseStepTemplateApiService);
      private readonly router = inject(Router);
      private readonly route = inject(ActivatedRoute);
      steps: ExpenseStepTemplatePayload[] = [];

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            version: this.formBuilder.control<number>(0, { nonNullable: true }),
      });

      buildStepActtions(index: number): ActionMenuOption[] {
            return [
                 // { label: '⚙️ Sửa', action: () => this.editStep(index) },
                  { label: '⬅️ Về trước', action: () => this.moveUp(index), disabled: index === 0 },
                  { label: '➡️ Về sau', action:() => this.moveDown(index), disabled: index === this.steps.length - 1 },
                  { label: '⛔ Xóa', color: 'red', action: () => this.removeStep(index)},
            ]
      }

      buildAddStepAction(): ActionMenuOption[] {
            return [
                  { label: 'Thêm bước duyệt thông thường', action: () => this.openExpenseStepRequestDialog('Standard') },
                  { label: 'Thêm bước duyệt theo điều kiện', action: () => this.openExpenseStepRequestDialog('Condition') }
            ]
      }

      openExpenseStepRequestDialog(approveMode: ExpenseApproveMode): void {
            const dialogRef = this.dialog.open(ExpenseWorkflowStepDialogComponent, {
                  data: { approveMode }
            });

            dialogRef.afterClosed().subscribe((step?: ExpenseStepTemplatePayload) => {
                  if (step) {
                        const order = this.steps.length + 1;
                        const newStep: ExpenseStepTemplatePayload = {
                              ...step,
                              order
                        };

                        this.steps.push(newStep);
                  }
            });
      }

      openSaveExpenseWorkflowTemplateDialog() {
            const dialogRef = this.dialog.open(SaveExpenseWorkflowTemplateDialogComponent, {
                  data: { data: this.steps }
            });
            dialogRef.afterClosed().subscribe((isSuccess: boolean) => {
                  if(isSuccess === true) {
                        this.router.navigate(['../expense-workflows'], { relativeTo: this.route });
                  }
            })
      }

      editStep(i: number): void {
            const current = this.steps[i];
            const dialogRef = this.dialog.open(ExpenseWorkflowStepDialogComponent, {
                  data: { step: current, approveMode: current.approveMode }
            });
            dialogRef.afterClosed().subscribe((step?: ExpenseStepTemplatePayload) => {
                  if(step) {
                        const updated = { ...step, order: current.order };
                        this.steps[i] = updated;
                  }
            })
      }

      // Step
      private recomputeOrders(): void {
            for (const [idx, step] of this.steps.entries()) {
                  step.order = idx + 1;
            }
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

      get canSave(): boolean {
            return this.steps.length > 0;
      }
}