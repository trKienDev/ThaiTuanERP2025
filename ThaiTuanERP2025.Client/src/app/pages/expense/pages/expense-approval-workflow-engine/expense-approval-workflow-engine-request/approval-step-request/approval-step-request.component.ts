import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { KitDropdownOption, KitDropdownComponent } from "../../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { handleHttpError } from "../../../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../../../shared/components/toast/toast.service";
import { FormBuilder, FormsModule, Validators, ReactiveFormsModule } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ApprovalStepRequest, FlowType } from "../../../../models/expense-approval-workflow.model";
import { UserOptionStore } from "../../../../../account/options/user-dropdown-options.store";

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

      constructor(
            @Inject(MAT_DIALOG_DATA) public data?: { step?: ApprovalStepRequest }
      ) {}

      formTitle: string = 'Thêm bước duyệt';
      submitting: boolean = false;
      userOptions$ = this.userOptionsStore.option$;

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            approverIds: this.formBuilder.control<string[]>([], { nonNullable: true, validators: [ Validators.required ]}),
            sla: this.formBuilder.control<number>(1, { nonNullable: true, validators: [ Validators.min(1) ]}),
            flowType: this.formBuilder.control<FlowType>('single', { nonNullable: true, validators: [ Validators.required ] }),
            order: this.formBuilder.control<number>(1, { nonNullable: true })
      });

      ngOnInit(): void {
            if(this.data?.step) {
                  this.formTitle = 'Sửa bước duyệt'
                  const s = this.data.step;
                  this.form.patchValue({
                        name: s.name,
                        approverIds: s.approverIds,
                        flowType: s.flowType,
                        sla: s.sla,
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

      async save(): Promise<void> {
            this.form.markAllAsTouched();
            if(this.form.invalid) return;

            this.submitting = true;
            try {
                  const payload: ApprovalStepRequest = this.form.getRawValue();
                  this.close({ isSuccess: true, step: payload });
            } catch(err) {
                  const messages = handleHttpError(err).join('\n');
                  this.toastService.errorRich(messages || 'Lỗi khi thêm bước duyệt');
            } finally {
                  this.submitting = false;
            }
      }

      close(result?: any): void {
            this.dialog.close(result);
      }
}