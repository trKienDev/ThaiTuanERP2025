import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { DivisionService } from "../../../services/division.service";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { UserService } from "../../../services/user.service";
import { KitDropdownOption, KitDropdownComponent } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { MatInputModule } from "@angular/material/input";
import { CreateDivisionRequest } from "../../../models/division.model";
import { firstValueFrom } from "rxjs";

@Component({
      selector: 'create-division-request',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MatInputModule, KitDropdownComponent],
      templateUrl: './create-division-request.component.html'
})
export class CreateDivisionRequestComponent implements OnInit {
      private dialogRef = inject(MatDialogRef<CreateDivisionRequestComponent>);
      private formBuilder = inject(FormBuilder);
      private divisionService = inject(DivisionService);
      private userService = inject(UserService);
      private toastService = inject(ToastService);

      submitting = false;
      userOptions: KitDropdownOption[] = [];

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required, Validators.maxLength(256)]}),
            description: this.formBuilder.control<string>(''),
            headUserId: this.formBuilder.control<string>(''),
      });

      ngOnInit(): void {
            this.loadUsers();
      }

      loadUsers(): void {
            this.userService.getAllUsers().subscribe({
                  next: (users) => {
                        this.userOptions = users.map(u => ({
                              id: u.id,
                              label: u.fullName
                        }))
                  },
                  error: err => {
                        const message = handleHttpError(err).join('\n');
                        this.toastService.errorRich(message);
                  }
            })
      }
      onUserSelected(opt: KitDropdownOption) {
            this.form.patchValue({ headUserId: opt.id });
      }

      async save(): Promise<void> {
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toastService.warningRich('Vui lòng điền đầy đủ thông tin bắt buộc');
                  return;
            }
            this.submitting = true;

            try {
                  const payload = this.form.value;

                  const request: CreateDivisionRequest = {
                        name: payload.name!,
                        description: payload.description || '',
                        headUserId: payload.headUserId!,
                  };
                  const result = await firstValueFrom(this.divisionService.create(payload));
                  const divisionId = result.id;
            } catch(error) {
                  const message = handleHttpError(error).join('\n');
                  this.toastService.errorRich(message);
            } finally {
                   this.submitting = false;
            }
      }

      close(isSuccess: boolean = false): void {
            this.dialogRef.close(isSuccess);
      }
}