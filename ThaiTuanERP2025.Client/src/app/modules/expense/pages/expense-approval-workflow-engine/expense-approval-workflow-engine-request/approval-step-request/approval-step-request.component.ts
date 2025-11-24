import { CommonModule } from "@angular/common";
import { Component, Inject, inject, Input, OnInit } from "@angular/core";
import { KitDropdownOption, KitDropdownComponent } from "../../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { ToastService } from "../../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { FormBuilder, FormsModule, Validators, ReactiveFormsModule } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { UserOptionStore } from "../../../../../account/options/user-dropdown.option";
import { logFormErrors } from "../../../../../../shared/utils/form.utils";
import { ExpenseApproveMode, ExpenseFlowType, ExpenseStepTemplatePayload } from "../../../../models/expense-step-template.model";

@Component({
      selector: 'approval-step-request-dialog',
      standalone: true,
      imports: [CommonModule, FormsModule, KitDropdownComponent, ReactiveFormsModule],
      templateUrl: './approval-step-request.component.html'
})
export class ApprovalStepRequestDialog implements OnInit {
      private readonly toastService = inject(ToastService);
      private readonly formBuilder = inject(FormBuilder);
      private readonly dialog = inject(MatDialogRef<ApprovalStepRequestDialog>);
      private readonly userOptionsStore = inject(UserOptionStore);

      @Input() approverMode: 'standard' | 'condition' = 'standard';

      constructor(
            @Inject(MAT_DIALOG_DATA) public data?: { 
                  step?: ExpenseStepTemplatePayload;
                  approverMode?: 'standard' | 'condition'; 
            }
      ) {}

      formTitle: string = 'Thêm bước duyệt';
      submitting: boolean = false;
      userOptions$ = this.userOptionsStore.option$;

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            approveMode: this.formBuilder.control<string>('Standard' as ExpenseApproveMode, Validators.required ),
            approverIds: this.formBuilder.control<string[]>([]),
            slaHours: this.formBuilder.control<number>(8, { nonNullable: true, validators: [ Validators.min(1) ]}),
            flowType: this.formBuilder.control<ExpenseFlowType>('Single', { nonNullable: true, validators: [ Validators.required ] }),
            order: this.formBuilder.control<number>(1, { nonNullable: true }),
            allowOverride: this.formBuilder.control<boolean>(true),
            resolverKey: this.formBuilder.control<string>(''),
            resolverParams: this.formBuilder.control<object | null>(null),
      });

      ngOnInit(): void {
            if(this.data?.approverMode) {
                  this.approverMode = this.data.approverMode;
                  this.form.patchValue({ approveMode: this.data.approverMode });
            }

            if(this.data?.step) {
                  this.formTitle = 'Sửa bước duyệt'
                  const s = this.data.step;     
                  this.form.patchValue({
                        name: s.name,
                        approverIds: s.approverIds ?? [],
                        flowType: s.flowType,
                        slaHours: s.slaHours,
                        order: s.order ?? 1,
                  });
            }
      }

      onApproverSelected(opt: KitDropdownOption) {
            const id = typeof opt === 'string' ? opt : opt.id;
            const ctrl = this.form.controls.approverIds;
            const current = ctrl.getRawValue() ?? [];
            if (!current.includes(id)) ctrl.setValue([...current, id]);

            if(this.form.controls.flowType.value === 'Single') {
                  ctrl.setValue([id]);
            } else {
                  if(!current.includes(id)) ctrl.setValue([...current, id]);
            }

            ctrl.markAsDirty();
            ctrl.updateValueAndValidity();
      }

      flowTypeOptions: KitDropdownOption[] = [
            { id: 'Single', label: '1 người duyệt' },
            { id: 'OneOfN', label: '1 trong nhiều người duyệt' }
      ];
      onFlowTypeSelected(opt: KitDropdownOption) {
            if(opt.id === 'Single') {
                  this.form.patchValue({ flowType: 'Single' });
            } else {
                  this.form.patchValue({ flowType: 'OneOfN'});
            }
      }

      approverTypeSelection: KitDropdownOption[] = [
            { id: 'manager-department', label: 'Chọn quản lý theo phòng ban' }
      ];
      onApproverTypeSelected(opt: KitDropdownOption) {
            this.form.patchValue({ resolverKey: opt.id });
      }

      async save(): Promise<void> {
            this.form.markAllAsTouched();
            if(this.form.invalid) {
                  logFormErrors(this.form);
                  return;
            }

            const value = this.form.getRawValue();
            const flowType = value.flowType === 'Single' ? 'Single' : 'OneOfN';
            const approverMode = value.approveMode === 'Standard' ? 'Standard' : 'Condition';

            const payload: ExpenseStepTemplatePayload = {
                  name: value.name!.trim(),
                  order: value.order ?? 1,
                  flowType,
                  slaHours: Number(value.slaHours) || 0,
                  approveMode: approverMode,
                  approverIds:  approverMode === 'Standard' ? (value.approverIds ?? []) : null,
                  resolverKey: approverMode === 'Condition' ? (value.resolverKey || null) : null,
                  resolverParams: approverMode === 'Condition' ? (value.resolverParams ?? null) : null,
            };

            this.dialog.close({ isSuccess: true, step: payload });
      }

      close(result?: any): void {
            this.dialog.close(result);
      }
}