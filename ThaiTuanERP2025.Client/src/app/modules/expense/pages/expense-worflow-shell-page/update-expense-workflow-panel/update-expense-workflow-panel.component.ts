import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ExpenseWorkflowTemplateApiService } from "../../../services/expense-workflow-template.service";
import { ExpenseWorkflowTemplateDto, ExpenseWorkflowTemplatePayload, mapExpenseWorkflowTemplateDtoToPayload } from "../../../models/expense-workflow-template.model";
import { firstValueFrom } from "rxjs";
import { FormBuilder, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { ExpenseApproveMode, ExpenseStepTemplatePayload } from "../../../models/expense-step-template.model";
import { ExpenseStepTemplateApiService } from "../../../services/expense-step-template.service";
import { ActionMenuOption } from "../../../../../shared/components/kit-action-menu/kit-action-menu.model";
import { KitActionMenuComponent } from "../../../../../shared/components/kit-action-menu/kit-action-menu.component";
import { ExpenseWorkflowStepDialogComponent } from "../../../components/dialogs/expense-workflow-step-dialog/expense-workflow-step-dialog.component";

@Component({
      selector: 'update-expense-workflow-panel',
      standalone: true,
      imports: [CommonModule, KitActionMenuComponent],
      templateUrl: './update-expense-workflow-panel.component.html',
      styleUrl: './update-expense-workflow-panel.component.scss'
})
export class UpdateExpenseWorkflowPanelComponent implements OnInit {
      private readonly route = inject(ActivatedRoute);
      private readonly expenseWorkflowTemplateApi = inject(ExpenseWorkflowTemplateApiService);
      private readonly dialog = inject(MatDialog);
      private readonly formBuilder = inject(FormBuilder);
      private readonly toast = inject(ToastService);
      private readonly expenseStepTemplateApi = inject(ExpenseStepTemplateApiService);
      private readonly router = inject(Router);

      workflowId!: string;
      workflowDetailDto: ExpenseWorkflowTemplateDto | null = null;
      workflowDetailPayload: ExpenseWorkflowTemplatePayload | null = null;

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            version: this.formBuilder.control<number>(0, { nonNullable: true }),
            steps: this.formBuilder.control<ExpenseStepTemplatePayload[]>([], { nonNullable: true, validators: [ Validators.required ]})
      });

      ngOnInit(): void {
            this.workflowId = this.route.snapshot.paramMap.get('id')!;
            this.getExpenseWorkflowTemplateDetail(this.workflowId);
      }

      editStep(i: number): void {
             if (!this.workflowDetailPayload) return;  

            const current = this.workflowDetailPayload?.steps[i];
            const dialogRef = this.dialog.open(ExpenseWorkflowStepDialogComponent, {
                  data: { step: current, approveMode: current?.approveMode }
            });
            dialogRef.afterClosed().subscribe((step?: ExpenseStepTemplatePayload) => {
                  if(step) {
                        const updated = { ...step, order: current?.order };
                        this.workflowDetailPayload!.steps[i] = updated;
                  }
            })
      }

      async getExpenseWorkflowTemplateDetail(id: string) {
            this.workflowDetailDto = await firstValueFrom(this.expenseWorkflowTemplateApi.getById(id));
            
            this.workflowDetailPayload = mapExpenseWorkflowTemplateDtoToPayload(this.workflowDetailDto);
            this.form.patchValue(this.workflowDetailPayload);
      }

      openExpenseStepRequestDialog(approveMode: ExpenseApproveMode): void {
            const dialogRef = this.dialog.open(ExpenseWorkflowStepDialogComponent, {
                  data: { approveMode }
            });

            dialogRef.afterClosed().subscribe((step?: ExpenseStepTemplatePayload) => {
                  if (step) {
                        const order = (this.workflowDetailDto?.steps.length ?? 0) + 1;
                        const newStep: ExpenseStepTemplatePayload = {
                              ...step,
                              order
                        };

                        this.workflowDetailPayload?.steps.push(newStep);
                  }
            });
      }

      buildStepActtions(index: number): ActionMenuOption[] {
            const lastIndex = (this.workflowDetailPayload?.steps.length ?? 1) - 1;

            return [
                 // { label: '⚙️ Sửa', action: () => this.editStep(index) },
                  { label: '⬅️ Về trước', action: () => this.moveUp(index), disabled: index === 0 },
                  { label: '➡️ Về sau', action:() => this.moveDown(index), disabled: index === lastIndex },
                  { label: '⛔ Xóa', color: 'red', action: () => this.removeStep(index)},
            ]
      }
      buildAddStepAction(): ActionMenuOption[] {
            return [
                  { label: 'Thêm bước duyệt thông thường', action: () => this.openExpenseStepRequestDialog('Standard') },
                  { label: 'Thêm bước duyệt theo điều kiện', action: () => this.openExpenseStepRequestDialog('Condition') }
            ]
      }

      private recomputeOrders(): void {
            this.workflowDetailPayload?.steps.forEach((s, idx) => s.order = idx + 1);
      }
      moveUp(i: number): void {
            if (!this.workflowDetailPayload) return;
            if(i <= 0) return;

            const steps = this.workflowDetailPayload.steps;
            [steps[i - 1], steps[i]] = [steps[i], steps[i - 1]];
            this.recomputeOrders();
      }
      moveDown(i: number): void {
            if (!this.workflowDetailPayload) return;

            const steps = this.workflowDetailPayload.steps;
            if(i >= steps.length - 1) return;

            [steps[i], steps[i + 1]] = [steps[i + 1], steps[i]];
            this.recomputeOrders();
      }
      removeStep(i: number): void {
            if (!this.workflowDetailPayload) return;

            this.workflowDetailPayload.steps.splice(i, 1);
            this.recomputeOrders();
      }

}