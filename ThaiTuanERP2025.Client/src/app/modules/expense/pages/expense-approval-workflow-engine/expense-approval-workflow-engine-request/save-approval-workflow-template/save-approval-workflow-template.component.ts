import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { ToastService } from "../../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { firstValueFrom } from "rxjs";
import { ExpenseWorkflowTemplateApiService } from "../../../../services/expense-workflow-template.service";
import { ExpenseStepTemplatePayload } from "../../../../models/expense-step-template.model";
import { ExpenseWorkflowTemplatePayload } from "../../../../models/expense-workflow-template.model";

@Component({
      selector: 'save-approval-workflow-template',
      standalone: true,
      imports: [ CommonModule , ReactiveFormsModule ],
      templateUrl: './save-approval-workflow-template.component.html'
})
export class SaveApprovalWorkflowTemplateComponent implements OnInit {
      private dialogRef = inject(MatDialogRef<SaveApprovalWorkflowTemplateComponent>);
      private formBuilder = inject(FormBuilder);
      private readonly toastService = inject(ToastService);
      private readonly wftService = inject(ExpenseWorkflowTemplateApiService);

      submitting: boolean = false;
      dialogTitle: string = 'Lưu luồng duyệt 2';

      stepTemplateRequest: ExpenseStepTemplatePayload[] = [];

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            version: this.formBuilder.control<number>(0, { nonNullable: true }),
      });

      constructor(
            @Inject(MAT_DIALOG_DATA) public input?: {
                  steps?: ExpenseStepTemplatePayload[];
            } 
      ) { }

      get stepCount(): number {
            return this.input?.steps?.length ?? 0;
      }

      ngOnInit(): void {
      }

      close(): void {
            this.dialogRef.close();
      }

      async saveTemplate(): Promise<void> {
            if(this.form.invalid || this.stepCount === 0 || this.submitting) return;

            this.submitting = true;
            try {
                  const steps = (this.input?.steps ?? []).map((s, i) => ({ ...s, order: i + 1 }));
                  const raw = this.form.getRawValue();
                  const payload: ExpenseWorkflowTemplatePayload = {
                        name: raw.name!.trim(),
                        version: raw.version,
                        steps
                  };

                  const result = await firstValueFrom(this.wftService.create(payload));
                  this.toastService.successRich('luồng duyệt đã được lưu');
                  this.dialogRef.close({ isSuccess: true, result });
            } catch(err) {
                  console.error('fail to save template: ', err);
                  this.toastService.errorRich('fail to save template');
            } finally {
                  this.submitting = false;
            }
      }

}