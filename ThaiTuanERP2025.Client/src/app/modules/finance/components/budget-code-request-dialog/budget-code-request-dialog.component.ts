import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatInputModule } from "@angular/material/input";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { firstValueFrom } from "rxjs";
import { BudgetCodeService } from "../../services/budget-code.service";
import { BudgetGroupService } from "../../services/budget-group.service";
import { CashoutCodeService } from "../../services/cashout-code.service";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { KitDropdownComponent, KitDropdownOption } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";
import { BudgetCodeRequest } from "../../models/budget-code.model";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { HttpErrorResponse } from "@angular/common/http";
import { ConfirmService } from "../../../../shared/components/confirm-dialog/confirm.service";

@Component({
      selector: 'add-budget-code-request',
      standalone: true,
      imports: [CommonModule, MatInputModule, FormsModule, ReactiveFormsModule, MatSnackBarModule, KitDropdownComponent, KitSpinnerButtonComponent],
      templateUrl: './budget-code-request-dialog.component.html',
}) 
export class BudgetCodeRequestDialogComponent implements OnInit {
      private readonly budgetCodeService = inject(BudgetCodeService);
      private readonly budgetGroupService = inject(BudgetGroupService);
      private readonly cashoutCodeService = inject(CashoutCodeService);
      private readonly ref = inject(MatDialogRef<BudgetCodeRequestDialogComponent>);
      private readonly formBuilder = inject(FormBuilder);
      private readonly toast = inject(ToastService);
      confirmService = inject(ConfirmService);

      budgetGroupOptions: KitDropdownOption[] = [];
      cashoutCodeOptions: KitDropdownOption[] = [];

      submitting = false;
      showErrors = false; 

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            code: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            budgetGroupId: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            cashoutCodeId: this.formBuilder.control<string | null>(null),
      })

      ngOnInit(): void {
            this.loadBudgetGroups();
            this.loadCashoutCodes();
      }

      loadBudgetGroups(): void {
            this.budgetGroupService.getAll().subscribe({
                  next: (budgetGroups) => {
                        this.budgetGroupOptions = budgetGroups.map(bg => ({
                              id: bg.id,
                              label: `${bg.code} - ${bg.name}`
                        }));
                  },
                  error: (err => handleHttpError(err))
            })
      }
      onBudgetGroupSelected(opt: KitDropdownOption) {
            this.form.patchValue({ budgetGroupId: opt.id });
            const c = this.form.get('budgetGroupId');
            c?.markAsDirty();
            c?.markAsTouched();
            c?.updateValueAndValidity({ onlySelf: true });
      }

      loadCashoutCodes(): void {
            this.cashoutCodeService.getAll().subscribe({
                  next: (cashoutCodes) => {
                        this.cashoutCodeOptions = cashoutCodes.map(co => ({
                              id: co.id,
                              label: co.name,
                        }))
                  }, 
                  error: (err => handleHttpError(err))
            })
      }
      onCashoutCodeSelected(opt: KitDropdownOption) {
            this.form.patchValue({ cashoutCodeId: opt.id });
      }

      async save(): Promise<void> {
            this.showErrors = true;
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich('Vui lòng điền đầy đủ thông tin');
                  return;
            }

            try {
                  this.submitting = true;
                  this.form.disable({ emitEvent: false });
                  
                  const payload: BudgetCodeRequest = this.form.getRawValue() as BudgetCodeRequest;
                  await firstValueFrom(this.budgetCodeService.create(payload));
                  this.toast.successRich('Thêm ngân sách thành công');
                  this.form.reset();
                  this.ref.close(true);
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
                        this.confirmService.error$(messages);
                        this.toast?.errorRich('Tạo mã ngân sách thất bại.');
                  }
            } finally {
                  this.form.enable({ emitEvent: false });
                  this.submitting = false;
                  this.showErrors = false;
            }
      }

      close(result: boolean = false): void {
            this.ref.close(result);
      }
}