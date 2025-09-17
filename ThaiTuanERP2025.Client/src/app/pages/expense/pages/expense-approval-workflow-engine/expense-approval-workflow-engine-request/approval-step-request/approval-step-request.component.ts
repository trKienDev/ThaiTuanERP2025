import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { UserService } from "../../../../../account/services/user.service";
import { KitDropdownOption, KitDropdownComponent } from "../../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { resolveAvatarUrl } from "../../../../../../shared/utils/avatar.utils";
import { environment } from "../../../../../../../environments/environment";
import { handleHttpError } from "../../../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../../../shared/components/toast/toast.service";
import { FormBuilder, Validators } from "@angular/forms";

@Component({
      selector: 'approval-step-request-dialog',
      standalone: true,
      imports: [CommonModule, KitDropdownComponent],
      templateUrl: './approval-step-request.component.html'
})
export class ApprovalStepRequestDialog implements OnInit {
      private readonly userService = inject(UserService);
      private readonly baseUrl = environment.baseUrl;
      private readonly toastService = inject(ToastService);
      private formBuilder = inject(FormBuilder);

      submitting: boolean = false;
      userOptions: KitDropdownOption[] = [];

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            approverIds: this.formBuilder.control<string[]>([], { nonNullable: true, validators: [ Validators.required ]}),
            sla: this.formBuilder.control<number>(8),
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
            ctrl.markAsDirty();
            ctrl.updateValueAndValidity();
      }
}