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
import { ExpenseStepTemplateRequestDialogComponent } from "../../../components/expense-step-request-dialog/expense-step-template-request-dialog.component";

@Component({
      selector: 'update-expense-workflow-panel',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './update-expense-workflow-panel.component.html'
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
      workflowDetailDto!: ExpenseWorkflowTemplateDto;
      workflowDetailPayload!: ExpenseWorkflowTemplatePayload;

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            version: this.formBuilder.control<number>(0, { nonNullable: true }),
            steps: this.formBuilder.control<ExpenseStepTemplatePayload[]>([], { nonNullable: true, validators: [ Validators.required ]})
      });

      ngOnInit(): void {
            this.workflowId = this.route.snapshot.paramMap.get('id')!;
            this.getExpenseWorkflowTemplateDetail(this.workflowId);
      }

      async getExpenseWorkflowTemplateDetail(id: string) {
            this.workflowDetailDto = await firstValueFrom(this.expenseWorkflowTemplateApi.getById(id));
            
            this.workflowDetailPayload = mapExpenseWorkflowTemplateDtoToPayload(this.workflowDetailDto);
            this.form.patchValue(this.workflowDetailPayload);
      }

      openExpenseStepRequestDialog(approveMode: ExpenseApproveMode): void {
            const dialogRef = this.dialog.open(ExpenseStepTemplateRequestDialogComponent, {
                  data: { approveMode }
            });

            dialogRef.afterClosed().subscribe((step?: ExpenseStepTemplatePayload) => {
                  if (step) {
                        const order = this.workflowDetailDto.steps.length + 1;
                        const newStep: ExpenseStepTemplatePayload = {
                              ...step,
                              order
                        };

                        this.workflowDetailPayload.steps.push(newStep);
                  }
            });
      }

      buildAddStepAction(): ActionMenuOption[] {
            return [
                  { label: 'Thêm bước duyệt thông thường', action: () => this.openExpenseStepRequestDialog('Standard') },
                  { label: 'Thêm bước duyệt theo điều kiện', action: () => this.openExpenseStepRequestDialog('Condition') }
            ]
      }

      private recomputeOrders(): void {
            this.workflowDetailPayload.steps.forEach((s, idx) => s.order = idx + 1);
      }
      moveUp(i: number): void {
            if(i <= 0) return;
            [this.workflowDetailPayload.steps[i-1], this.workflowDetailPayload.steps[i]] = [this.workflowDetailPayload.steps[i], this.workflowDetailPayload.steps[i - 1]];
            this.recomputeOrders();
      }
      moveDown(i: number): void {
            if(i >= this.workflowDetailPayload.steps.length - 1) return;
            [this.workflowDetailPayload.steps[i], this.steps[i + 1]] = [this.steps[i + 1], this.steps[i]];
            this.recomputeOrders();
      }
      removeStep(i: number): void {
            this.steps.splice(i, 1);
            this.recomputeOrders();
      }

}