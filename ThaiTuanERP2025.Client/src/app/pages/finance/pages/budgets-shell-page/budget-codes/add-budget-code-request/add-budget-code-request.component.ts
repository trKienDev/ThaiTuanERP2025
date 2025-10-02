import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatInputModule } from "@angular/material/input";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { firstValueFrom } from "rxjs";
import { BudgetCodeService } from "../../../../services/budget-code.service";
import { BudgetGroupService } from "../../../../services/budget-group.service";
import { CashoutCodeService } from "../../../../services/cashout-code.service";
import { ToastService } from "../../../../../../shared/components/toast/toast.service";
import { KitDropdownComponent, KitDropdownOption } from "../../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { handleHttpError } from "../../../../../../shared/utils/handle-http-errors.util";
import { BudgetCodeRequest } from "../../../../models/budget-code.model";

@Component({
      selector: 'add-budget-code-request',
      standalone: true,
      imports: [CommonModule, MatInputModule, FormsModule, ReactiveFormsModule, MatSnackBarModule, KitDropdownComponent],
      templateUrl: './add-budget-code-request.component.html',
}) 
export class AddBudgetCodeRequestDialogComponent implements OnInit {
      private budgetCodeService = inject(BudgetCodeService);
      private budgetGroupService = inject(BudgetGroupService);
      private cashoutCodeService = inject(CashoutCodeService);
      private ref = inject(MatDialogRef<AddBudgetCodeRequestDialogComponent>);
      private formBuilder = inject(FormBuilder);
      private toast = inject(ToastService);

      budgetGroupOptions: KitDropdownOption[] = [];
      cashoutCodeOptions: KitDropdownOption[] = [];

      submitting = false;

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            code: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            budgetGroupId: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            cashoutCodeId: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
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
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warning('Vui lòng điền đầy đủ thông tin');
                  return;
            }
            this.submitting = true;

            try {
                  const payload: BudgetCodeRequest = this.form.getRawValue() as BudgetCodeRequest;

                  const created = await firstValueFrom(this.budgetCodeService.create(payload));
                  this.toast.successRich('Thêm ngân sách thành công');
                  this.ref.close(true);
            } catch(error) {
                  const messages = handleHttpError(error);
                  this.toast.errorRich('Thêm ngân sách thất bại');
            } finally {
                  this.submitting = false;
            }
      }

      close(result: boolean = false): void {
            this.ref.close(result);
      }
}