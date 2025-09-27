import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { CreateApprovalStepTemplateRequest } from "../../../../models/approval-step-template.model";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { ToastService } from "../../../../../../shared/components/toast/toast.service";
import { CreateApprovalWorkflowTemplateRequest } from "../../../../models/approval-workflow-template.model";
import { firstValueFrom } from "rxjs";
import { ApprovalWorkflowTemplateService } from "../../../../services/approval-workflow-template.service";

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
      private readonly wftService = inject(ApprovalWorkflowTemplateService);

      submitting: boolean = false;
      dialogTitle: string = 'Lưu luồng duyệt 2';

      stepTemplateRequest: CreateApprovalStepTemplateRequest[] = [];

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            version: this.formBuilder.control<number>(0, { nonNullable: true }),
      });

      constructor(
            @Inject(MAT_DIALOG_DATA) public input?: {
                  steps?: CreateApprovalStepTemplateRequest[];
            } 
      ) { }

      get stepCount(): number {
            return this.input?.steps?.length ?? 0;
      }

      ngOnInit(): void {
            console.log('steps: ', this.input?.steps);
      }

      close(): void {
            this.dialogRef.close();
      }

      async saveTemplate(): Promise<void> {
            alert('save template');
            if(this.form.invalid || this.stepCount === 0 || this.submitting) return;

            this.submitting = true;
            try {
                  const steps = (this.input?.steps ?? []).map((s, i) => ({ ...s, order: i + 1 }));
                  const raw = this.form.getRawValue();
                  const payload: CreateApprovalWorkflowTemplateRequest = {
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