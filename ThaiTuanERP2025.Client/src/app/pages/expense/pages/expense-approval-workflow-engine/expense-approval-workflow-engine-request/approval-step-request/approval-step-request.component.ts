import { CommonModule } from "@angular/common";
import { Component, Inject, inject, Input, OnInit } from "@angular/core";
import { KitDropdownOption, KitDropdownComponent } from "../../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { ToastService } from "../../../../../../shared/components/toast/toast.service";
import { FormBuilder, FormsModule, Validators, ReactiveFormsModule } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { UserOptionStore } from "../../../../../account/options/user-dropdown-options.store";
import { ApproverMode, CreateApprovalStepTemplateRequest, FlowType } from "../../../../models/approval-step-template.model";
import { logFormErrors } from "../../../../../../shared/utils/form.utils";

@Component({
      selector: 'approval-step-request-dialog',
      standalone: true,
      imports: [CommonModule, FormsModule, KitDropdownComponent, ReactiveFormsModule],
      templateUrl: './approval-step-request.component.html'
})
export class ApprovalStepRequestDialog implements OnInit {
      private readonly toastService = inject(ToastService);
      private formBuilder = inject(FormBuilder);
      private dialog = inject(MatDialogRef<ApprovalStepRequestDialog>);
      private userOptionsStore = inject(UserOptionStore);

      @Input() approverMode: 'standard' | 'condition' = 'standard';

      constructor(
            @Inject(MAT_DIALOG_DATA) public data?: { 
                  step?: CreateApprovalStepTemplateRequest;
                  approverMode?: 'standard' | 'condition'; 
            }
      ) {}

      formTitle: string = 'Thêm bước duyệt';
      submitting: boolean = false;
      userOptions$ = this.userOptionsStore.option$;

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            approverMode: this.formBuilder.control<string>('standard' as ApproverMode, Validators.required ),
            approverIds: this.formBuilder.control<string[]>([]),
            slaHours: this.formBuilder.control<number>(1, { nonNullable: true, validators: [ Validators.min(1) ]}),
            flowType: this.formBuilder.control<FlowType>('single', { nonNullable: true, validators: [ Validators.required ] }),
            order: this.formBuilder.control<number>(1, { nonNullable: true }),
            allowOverride: this.formBuilder.control<boolean>(true),
            resolverKey: this.formBuilder.control<string>(''),
            resolverParams: this.formBuilder.control<any | null>(null),
      });

      ngOnInit(): void {
            if(this.data?.approverMode) {
                  this.approverMode = this.data.approverMode;
                  this.form.patchValue({ approverMode: this.data.approverMode });
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

            if(this.form.controls.flowType.value === 'single') {
                  ctrl.setValue([id]);
            } else {
                  if(!current.includes(id)) ctrl.setValue([...current, id]);
            }

            ctrl.markAsDirty();
            ctrl.updateValueAndValidity();
      }

      flowTypeOptions: KitDropdownOption[] = [
            { id: 'single', label: '1 người duyệt' },
            { id: 'one-of-n', label: '1 trong nhiều người duyệt' }
      ];
      onFlowTypeSelected(opt: KitDropdownOption) {
            if(opt.id === 'single') {
                  this.form.patchValue({ flowType: 'single' });
            } else {
                  this.form.patchValue({ flowType: 'one-of-n'});
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
                  console.log('form: ', this.form.getRawValue());
                  return;
            }

            const value = this.form.getRawValue();
            const flowType = value.flowType === 'single' ? 'single' : 'one-of-n';
            console.log('approverMode: ', value.approverMode);
            const approverMode = value.approverMode === 'standard' ? 'standard' : 'condition';

            const payload: CreateApprovalStepTemplateRequest = {
                  name: value.name!.trim(),
                  order: value.order ?? 1,
                  flowType,
                  slaHours: Number(value.slaHours) || 0,
                  approverMode,
                  approverIds:  approverMode === 'standard' ? (value.approverIds ?? []) : null,
                  resolverKey: approverMode === 'condition' ? (value.resolverKey || null) : null,
                  resolverParams: approverMode === 'condition' ? (value.resolverParams ?? null) : null,
                  allowOverride: approverMode === 'condition' ? !!value.allowOverride : false,
            };

            this.dialog.close({ isSuccess: true, step: payload });
      }

      close(result?: any): void {
            this.dialog.close(result);
      }
}