import { CommonModule } from "@angular/common";
import { Component, Inject, inject, Input, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { UserOptionStore } from "../../../account/options/user-dropdown.option";
import { ExpenseApproveMode, ExpenseFlowType, ExpenseStepTemplatePayload } from "../../models/expense-step-template.model";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { KitDropdownComponent, KitDropdownOption } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";

@Component({
      selector: 'expense-step-template-request-dialog',
      standalone: true,
      imports: [CommonModule, KitSpinnerButtonComponent, ReactiveFormsModule, KitDropdownComponent],
      templateUrl: './expense-step-template-request-dialog.component.html'
})
export class ExpenseStepTemplateRequestDialogComponent implements OnInit {
      private readonly dialogRef = inject(MatDialogRef<ExpenseStepTemplateRequestDialogComponent>);
      private readonly toast = inject(ToastService);
      private readonly formBuilder = inject(FormBuilder);

      public userOptions$ = inject(UserOptionStore).option$;
      public dialogTitle: string = 'Thêm bước duyệt';
      public showErrors: boolean = false;
      public submitting: boolean = false;
      
      @Input() approveMode: 'Standard' | 'Condition' = 'Standard';

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            approveMode: this.formBuilder.control<ExpenseApproveMode>('Standard', { nonNullable: true, validators: Validators.required }),
            approverIds: this.formBuilder.control<string[] | null>(null),
            slaHours: this.formBuilder.control<number>(8, { nonNullable: true, validators: [ Validators.min(1) ]}),
            flowType: this.formBuilder.control<ExpenseFlowType>('Single', { nonNullable: true, validators: [ Validators.required ] }),
            order: this.formBuilder.control<number>(1, { nonNullable: true }),
            resolverKey: this.formBuilder.control<string | null>(null),
            resolverParams: this.formBuilder.control<object | null>(null),
      });

      constructor(
            @Inject(MAT_DIALOG_DATA) public data?: { 
                  step?: ExpenseStepTemplatePayload;
                  approveMode?: 'Standard' | 'Condition'; 
            }
      ) {

      }

      ngOnInit(): void {
            console.log('approveMode: ', this.data?.approveMode);
            if(this.data?.approveMode) {
                  this.approveMode = this.data.approveMode;
                  this.form.patchValue({ approveMode: this.data.approveMode });
            }

            if(this.data?.step) {
                  this.dialogTitle = 'Sửa bước duyệt'
                  const s = this.data.step;     
                  console.log('step: ', s);
                  this.form.patchValue({
                        name: s.name,
                        approverIds: s.approverIds ?? [],
                        approveMode: s.approveMode,
                        resolverKey: s.resolverKey,
                        flowType: s.flowType,
                        slaHours: s.slaHours,
                        order: s.order ?? 1,
                  });

                  console.log('form: ', this.form.getRawValue());
            }

            this.form.get('approverMode')?.valueChanges.subscribe(mode => {
                  this.updateApproverValidation(mode);
                  this.approveMode = mode; // update UI cho *ngIf
            });

            // chạy 1 lần ban đầu
            this.updateApproverValidation(this.form.get('approverMode')!.value);
      }

      // === Approver Validation ====
      updateApproverValidation(mode: ExpenseApproveMode) {
            const approverIdsControl = this.form.get('approverIds');

            if (!approverIdsControl) return;

            if (mode === 'Standard') {
                  approverIdsControl.addValidators([Validators.required]);
            } else {
                  approverIdsControl.clearValidators();
            }

            approverIdsControl.updateValueAndValidity();
      }

      // === Flow Type Options ===
      get isMultiApprover(): boolean {
            return this.form.get('flowType')?.value === 'OneOfN';
      }
      flowTypeOptions: KitDropdownOption[] = [
            { id: 'Single', label: '1 người duyệt' },
            { id: 'OneOfN', label: '1 trong nhiều người duyệt' }
      ];

      // === Approver Type - condition ====
      approverTypeSelection: KitDropdownOption[] = [
            { id: 'manager-department', label: 'Chọn quản lý theo phòng ban' }
      ];

      create() {
            this.showErrors = true;
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich("Vui lòng điền đầy đủ thông tin");
                  return;
            }

            const payload: ExpenseStepTemplatePayload = this.form.getRawValue();
            console.log('payload: ', payload);
            this.close(payload);
            this.showErrors = false;

      }

      close(step?: ExpenseStepTemplatePayload): void {
            this.dialogRef.close(step);
      }
}