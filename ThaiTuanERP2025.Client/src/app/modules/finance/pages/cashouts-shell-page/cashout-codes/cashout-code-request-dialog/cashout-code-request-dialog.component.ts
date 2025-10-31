import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { CashoutGroupService } from "../../../../services/cashout-group.service";
import { LedgerAccountService } from "../../../../services/ledger-account.service";
import { handleHttpError } from "../../../../../../shared/utils/handle-http-errors.util";
import { CashoutCodeRequest } from "../../../../models/cashout-code.model";
import { firstValueFrom } from "rxjs";
import { CashoutCodeService } from "../../../../services/cashout-code.service";
import { ToastService } from "../../../../../../shared/components/toast/toast.service";
import { KitDropdownOption, KitDropdownComponent } from "../../../../../../shared/components/kit-dropdown/kit-dropdown.component";


@Component({
      selector: 'cashout-code-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule,
    MatInputModule, MatCheckboxModule, MatButtonModule, MatSelectModule, KitDropdownComponent],
      templateUrl: './cashout-code-request-dialog.component.html'
})
export class CashoutCodeRequestDialogComponent implements OnInit {
      private formBuilder = inject(FormBuilder);
      private dialogRef = inject(MatDialogRef<CashoutCodeRequestDialogComponent>);
      private cashoutCodeService = inject(CashoutCodeService);
      private cashoutGroupService = inject(CashoutGroupService);
      private ledgerAccountService = inject(LedgerAccountService);
      private toast = inject(ToastService);

      private isSuccess: boolean = false;

      cashoutGroupOptions: KitDropdownOption[] = [];
      postingLedgerAccountOptions: KitDropdownOption[] = [];

      submitting = false;
      errorMessages: string[] = [];

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { validators: [Validators.required, Validators.maxLength(250)], updateOn: 'blur' }),
            cashoutGroupId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            cashoutGroupName: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            postingLedgerAccountId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            description: this.formBuilder.control<string>(''),
            isActive: [true]
      });

      ngOnInit(): void {
            this.loadCashoutGroupOptions();
            this.loadLedgerAccountOptions();
      }

      loadCashoutGroupOptions(): void {
            this.cashoutGroupService.getAll().subscribe({
                  next: (cashoutGroups) => {
                        this.cashoutGroupOptions = cashoutGroups.map(cg => ({
                              id: cg.id,
                              label: cg.name
                        }))
                  }, 
                  error: (err => {
                              const message = handleHttpError(err);
                              this.toast.errorRich('Lỗi tải nhóm dòng tiền ra');
                        }
                  )
            })
      }
      onCashoutGroupSelected(opt: KitDropdownOption) {
            this.form.patchValue({ cashoutGroupId: opt.id });
      }

      loadLedgerAccountOptions(): void {
            this.ledgerAccountService.getAll().subscribe({
                  next: (ledgerAccounts) => {
                        this.postingLedgerAccountOptions = ledgerAccounts.map(la => ({
                              id: la.id, 
                              label: la.name
                        }))
                  },
                  error: (err => handleHttpError(err))
            })
      }
      onLedgerAccountsSelected(opt: KitDropdownOption) {
            this.form.patchValue({ postingLedgerAccountId: opt.id })
      }

      async submit(): Promise<void> {
            this.errorMessages = [];
            // if(this.form.invalid) {
            //       this.form.markAllAsTouched();
            //       return;
            // }

            this.submitting = true;

            try {
                  const payload: CashoutCodeRequest = this.form.getRawValue() as CashoutCodeRequest;
                  const created = await firstValueFrom(this.cashoutCodeService.create(payload));
                  this.toast.successRich('Thêm dòng tiền ra thành công');
                  this.dialogRef.close(this.isSuccess = true);
            } catch(error) {
                  const msg = handleHttpError(error);
                  const message = Array.isArray(msg) ? msg.join('\n') : String(msg);
                  this.toast.errorRich('Lỗi thêm nhóm dòng tiền ra');
            } finally {
                  this.submitting = false;
            }
      }

      close(result: boolean): void {
            this.dialogRef.close(result);
      }
}