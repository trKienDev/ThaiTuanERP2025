import { CommonModule } from "@angular/common";
import { Component, Inject, inject} from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { combineLatest, firstValueFrom, map, startWith } from "rxjs";
import { KitDropdownComponent, KitDropdownOption } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";
import { DepartmentFacade } from "../../facades/department.facade";
import { DepartmentDto, SetDepartmentManagerPayload } from "../../models/department.model";
import { UserOptionStore } from "../../options/user-dropdown.option";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { ConfirmService } from "../../../../shared/components/confirm-dialog/confirm.service";
import { HttpErrorResponse } from "@angular/common/http";

@Component({
      selector: 'department-manager-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitDropdownComponent, KitSpinnerButtonComponent],
      templateUrl: './department-manager-dialog.component.html'
})
export class DepartmentManagerDialogComponent {
      private readonly toast = inject(ToastService);
      private readonly confirm = inject(ConfirmService);
      private readonly dialogRef = inject(MatDialogRef<DepartmentManagerDialogComponent>);
      private readonly formBuilder = inject(FormBuilder);
      private readonly departmentFacade = inject(DepartmentFacade);

      depatment!: DepartmentDto;

      private readonly  userOptionStore = inject(UserOptionStore);
      managerOptions$ = this.userOptionStore.option$;

      public submitting = false;
      public showErrors = false;

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: DepartmentDto 
      ) {
            if(this.data) {
                  this.depatment = this.data;
                  if(this.depatment.primaryManager) {
                        this.form.patchValue({ primaryManagerId: this.depatment.primaryManager.id });
                  }
                  if (this.depatment.viceManagers?.length) {
                        this.form.patchValue({ viceManagerIds: this.depatment.viceManagers.map(vm => vm.id) });
                  }
            }
      }

      form = this.formBuilder.group({
            primaryManagerId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ] }),
            viceManagerIds: this.formBuilder.control<string[]>([], { nonNullable: false, validators: [ Validators.required ] }),
            cascadeToMembers: this.formBuilder.control<boolean>(false, { nonNullable: true }),
            replaceMode: this.formBuilder.control<boolean>(true, { nonNullable: true })
      }, {
            validators: [(fg) => {
                  const primary = fg.get('primaryManagerId')?.value;
                  const vices = fg.get('viceManagerIds')?.value ?? [];
                  return primary && Array.isArray(vices) && vices.includes(primary) ? { overlap: true } : null;
            }]
      });

      viceOptions$ = combineLatest([
            this.managerOptions$,
            this.form.controls.primaryManagerId.valueChanges.pipe(
                  startWith(this.form.controls.primaryManagerId.value) // lấy cả giá trị khởi tạo
            )
      ]).pipe(
            map(([opts, primary]) => (opts ?? []).filter(o => o.id !== primary))
      );

      onPrimaryManagerSelected(opt: KitDropdownOption) {
            const id = typeof opt === 'string' ? opt : opt?.id;
            this.form.patchValue({ primaryManagerId: id ?? '' });

            // Nếu id này đang nằm trong viceManagerIds thì loại bỏ
            const ctrl = this.form.controls.viceManagerIds;
            const current = ctrl.getRawValue() ?? [];
            if (id && current.includes(id)) {
                  ctrl.setValue(current.filter(x => x !== id));
                  ctrl.markAsDirty();
                  ctrl.updateValueAndValidity();
            }
      }

      onViceManagersSelected(opt: KitDropdownOption) {
            const primaryId = this.form.controls.primaryManagerId.getRawValue();
            const picked = Array.isArray(opt) ? opt : (opt ? [opt] : []);
            const ids = picked.map(o => typeof o === 'string' ? o : o.id).filter(Boolean) as string[];

            // Loại bỏ bất kỳ id nào trùng với primary
            const filtered = ids.filter(id => id !== primaryId);

            const ctrl = this.form.controls.viceManagerIds;
            const current = ctrl.getRawValue() ?? [];
            const next = Array.from(new Set([...current, ...filtered])); // unique

            if (next.length !== current.length) {
                  ctrl.setValue(next);
                  ctrl.markAsDirty();
                  ctrl.updateValueAndValidity();
            }

            // Nếu user cố chọn đúng primary → cảnh báo nhẹ
            if (ids.some(id => id === primaryId)) {
                  this.toast?.warningRich('Người quản lý chính không thể là quản lý phụ.');
            }
      }

      async submit(): Promise<void> {
            this.showErrors = true;
            this.form.markAllAsTouched();
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich('Vui lòng điền đẩy đủ thông tin');
                  return;
            }

            try {
                  this.submitting = true;
                  this.form.disable({ emitEvent: false });

                  const raw = this.form.getRawValue();
                  const payload: SetDepartmentManagerPayload = {
                        primaryManagerId: raw.primaryManagerId,
                        viceManagerIds: raw.viceManagerIds,
                        cascadeToMembers: raw.cascadeToMembers,
                        replaceMode: raw.replaceMode
                  };
                  console.log('payload: ', payload);
                  const result = await firstValueFrom(this.departmentFacade.setManager(this.depatment.id, payload));
                  this.toast.successRich("Thiết lập quản lý thành công");
                  this.dialogRef.close({ isSuccess: true, response: result });
            } catch(error) {
                  if (error instanceof HttpErrorResponse && [401,403,500,0].includes(error.status ?? 0)) {
                        return;
                  }
                  if (error instanceof HttpErrorResponse && error.status === 404) {
                        this.toast?.warningRich('Không tìm thấy dữ liệu để tạo mã ngân sách.');
                  } else if (error instanceof HttpErrorResponse && error.status === 400) {
                        this.toast?.errorRich(error.error?.message || 'Dữ liệu không hợp lệ.');
                  } else {
                        const messages = handleHttpError(error).join('\n');
                        this.confirm.error$(messages);
                        this.toast?.errorRich('Tạo mã ngân sách thất bại.');
                  }
            } finally {
                  this.submitting = false;
            }
      }

      close(): void {
            this.dialogRef.close();
      }
}