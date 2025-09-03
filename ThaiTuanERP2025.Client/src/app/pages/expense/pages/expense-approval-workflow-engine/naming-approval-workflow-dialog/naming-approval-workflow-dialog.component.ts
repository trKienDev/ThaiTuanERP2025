import { CommonModule } from "@angular/common";
import { Component, inject, Inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";

@Component({
      selector: 'expense-naming-approval-workflow-dialog',
      standalone: true,
      imports: [ CommonModule ,MatDialogModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule ],
      templateUrl: './naming-approval-workflow-dialog.component.html',
      styleUrl: './naming-approval-workflow-dialog.component.scss'
})
export class NamingExpenseApprovalWorkflowDialogComponent {
      private formBuilder = inject(FormBuilder);
      form = this.formBuilder.group({
            name: this.formBuilder.nonNullable.control<string>('', [Validators.required, Validators.maxLength(256)])
      });

      constructor(
            public dialogRef: MatDialogRef<NamingExpenseApprovalWorkflowDialogComponent, string | undefined>,
            @Inject(MAT_DIALOG_DATA) public data: { defaultName?: string } | null 
      ) {}

      confirm() {
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  return;
            }
            
            this.dialogRef.close(this.form.value.name!.trim());
      }
}