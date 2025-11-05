import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { CashoutGroupDto, CashoutGroupRequest } from "../../../../models/cashout-group.model";
import { catchError, firstValueFrom, of } from "rxjs";
import { handleHttpError } from "../../../../../../shared/utils/handle-http-errors.util";
import { MatSelectModule } from "@angular/material/select";
import { CashoutGroupService } from "../../../../services/cashout-group.service";
import { KitDropdownOption, KitDropdownComponent } from "../../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { ToastService } from "../../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";

@Component({
      selector: 'cashout-group-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule,
    MatInputModule, MatCheckboxModule, MatButtonModule, MatSelectModule, KitDropdownComponent],
      templateUrl: './cashout-group-request-dialog.component.html',
})
export class CashoutGroupRequestDialogComponent implements OnInit {
      private formBuilder = inject(FormBuilder);
      private dialogRef = inject(MatDialogRef<CashoutGroupRequestDialogComponent>);
      private cashoutGroupService = inject(CashoutGroupService);
      private toast = inject(ToastService);

      saving = false;
      errorMessages: string[] = [];

      parentOptions: KitDropdownOption[] = [];

      ngOnInit(): void {
            this.loadCashoutGroups();
      }


      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { validators: [Validators.required, Validators.maxLength(200)], updateOn: 'blur' }),
            description: this.formBuilder.control<string>(''),
            isActive: [true],
            parentId: this.formBuilder.control<string | null>(null),
      });

      get cashoutGroupRequestForm() {
            return this.form.controls;
      }

      loadCashoutGroups(): void {
            this.cashoutGroupService.getAll().subscribe({
                  next: (parents) => {
                        this.parentOptions = parents.map(p => ({
                              id: p.id,
                              label: p.name
                        }));
                  }, 
                  error: (err => {
                        const message = handleHttpError(err);
                        this.toast.errorRich('Lỗi khi tải nhóm cha');
                  })
            })
      }
      onParentsSelected(opt: KitDropdownOption) {
            this.form.patchValue({ parentId: opt?.id ?? null });
      }


      async submit(): Promise<void> {
            this.errorMessages = [];
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  return;
            }

            this.saving = true;
            
            try {
                  const raw = this.form.getRawValue(); // { name, description, isActive, parentId }
                  const payload = {
                        ...raw,
                        parentId: raw.parentId || null, // quan trọng: '' -> null
                  } as CashoutGroupRequest;
                  const created = await firstValueFrom(this.cashoutGroupService.create(payload));
                  this.toast.successRich('Thêm nhóm dòng tiền ra thành công');
                  this.dialogRef.close(true);
            } catch(error) {  
                  const msg = handleHttpError(error);
                  const message = Array.isArray(msg) ? msg.join('\n') : String(msg);
                  this.toast.errorRich(message);
            } finally {
                  this.saving = false;
            }
      }

      close(): void {
            this.dialogRef.close();
      }
}