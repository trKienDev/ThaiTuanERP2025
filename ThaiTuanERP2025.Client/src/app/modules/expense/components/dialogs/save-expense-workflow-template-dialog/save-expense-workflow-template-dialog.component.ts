import { CommonModule } from "@angular/common";
import { Component, Inject, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { ExpenseWorkflowTemplateApiService } from "../../../services/api/expense-workflow-template.service";
import { ExpenseStepTemplatePayload } from "../../../models/expense-step-template.model";
import { KitSpinnerButtonComponent } from "../../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { ExpenseWorkflowTemplatePayload } from "../../../models/expense-workflow-template.model";
import { HttpErrorHandlerService } from "../../../../../core/services/http-errror-handler.service";
import { firstValueFrom } from "rxjs";

@Component({
      selector: 'save-expense-workflow-template-dialog',
      standalone: true,
      imports: [CommonModule, KitSpinnerButtonComponent, ReactiveFormsModule],
      templateUrl: './save-expense-workflow-template-dialog.component.html'
})
export class SaveExpenseWorkflowTemplateDialogComponent {
      private readonly dialog = inject(MatDialogRef<SaveExpenseWorkflowTemplateDialogComponent>);
      private readonly formBuilder = inject(FormBuilder);
      private readonly toast = inject(ToastService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly expenseWorkflowTemplateApi = inject(ExpenseWorkflowTemplateApiService);


      public submitting: boolean = false;
      public showErrors: boolean = false;

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            version: this.formBuilder.control<number>(1, { nonNullable: true, validators: [Validators.required] }),
            steps: this.formBuilder.control<ExpenseStepTemplatePayload[]>([], { nonNullable: true, validators: [Validators.required] })
      });

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: ExpenseStepTemplatePayload[]
      ) {
            this.form.patchValue({ steps: data });
      }

      async save() {
            this.showErrors = true;

            try {
                  this.submitting = true;

                  const { name, version, steps } = this.form.getRawValue();

                  const rawSteps: ExpenseStepTemplatePayload[] =
                        (steps as any)?.data ?? steps ?? [];

                  // Chuẩn hoá approverIds
                  const normalizedSteps = rawSteps.map(step => {
                        let approverIds: string[] = [];

                        if (Array.isArray(step.approverIds)) {
                              approverIds = step.approverIds;
                        } else if (step.approverIds) {
                              approverIds = [step.approverIds];
                        }

                        return {
                              ...step,
                              approverIds
                        };
                  });

                  // Build final payload
                  const payload: ExpenseWorkflowTemplatePayload = {
                        name,
                        version,
                        steps: normalizedSteps
                  };
                  console.log('payload: ', payload);
                  await firstValueFrom(this.expenseWorkflowTemplateApi.create(payload));
                  this.toast.successRich("Lưu luồng duyệt thành công");
                  this.showErrors = false;
                  this.close(true);
            } catch (error) {
                  this.httpErrorHandler.handle(error, "Lưu luồng duyệt thất bại");
            } finally {
                  this.submitting = false;
            }
      }

      close(isSuccess: boolean = false) {
            this.dialog.close(isSuccess);
      }
}