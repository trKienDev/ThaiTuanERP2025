import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { UserService } from "../../../../../account/services/user.service";
import { KitDropdownOption, KitDropdownComponent } from "../../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { resolveAvatarUrl } from "../../../../../../shared/utils/avatar.utils";
import { environment } from "../../../../../../../environments/environment";
import { handleHttpError } from "../../../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../../../shared/components/toast/toast.service";
import { FormBuilder, FormsModule, Validators, ReactiveFormsModule } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";

@Component({
      selector: 'approval-step-request-dialog',
      standalone: true,
      imports: [CommonModule, FormsModule, KitDropdownComponent, ReactiveFormsModule],
      templateUrl: './approval-step-request.component.html'
})
export class ApprovalStepRequestDialog implements OnInit {
      private readonly baseUrl = environment.baseUrl;
      private readonly userService = inject(UserService);
      private readonly toastService = inject(ToastService);
      private formBuilder = inject(FormBuilder);
      private dialog = inject(MatDialogRef<ApprovalStepRequestDialog>);

      submitting: boolean = false;
      userOptions: KitDropdownOption[] = [];

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            approverIds: this.formBuilder.control<string[]>([], { nonNullable: true, validators: [ Validators.required ]}),
            sla: this.formBuilder.control<number>(8, { validators: [ Validators.min(1)]}),
            flowType: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ] }),
      })

      ngOnInit(): void {
            this.loadUsers();
      }

      loadUsers(): void {
            this.userService.getAllUsers().subscribe({
                  next: (users) => {
                        this.userOptions = users.map(u => ({
                              id: u.id,
                              label: u.fullName,
                              imgUrl: resolveAvatarUrl(this.baseUrl, u)
                        }));
                  },
                  error: (err) => {
                        const messages = handleHttpError(err).join('\n');
                        this.toastService.errorRich(messages || 'Tải danh sách user thất bại');
                  }
            })
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

                  // nếu đã chọn >1 người thì cắt xuống còn 1
                  const approvers = this.form.controls.approverIds.getRawValue() ?? [];
                  if(approvers.length > 1) {
                        this.form.controls.approverIds.setValue([approvers[approvers.length - 1]]);
                  }
            } else {
                  this.form.patchValue({ flowType: 'one-of-n'});
            }
      }

      async save(): Promise<void> {
            this.form.markAllAsTouched();
            if(this.form.invalid) return;

            this.submitting = true;
            try {
                  const payload = this.form.getRawValue();
                  console.log('payload: ', payload);
                  this.close(true);
            } catch(err) {
                  const messages = handleHttpError(err).join('\n');
                  this.toastService.errorRich(messages || 'Lỗi khi thêm bước duyệt');
            } finally {
                  this.submitting = false;
            }
      }

      close(isSuccess: boolean = false): void {
            this.dialog.close(isSuccess);
      }
}