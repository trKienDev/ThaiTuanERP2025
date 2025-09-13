import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { CashoutGroupDto } from "../../../../models/cashout-group.model";
import { LedgerAccountDto } from "../../../../models/ledger-account.model";
import { CashoutGroupService } from "../../../../services/cashout-group.service";
import { LedgerAccountService } from "../../../../services/ledger-account.service";
import { handleHttpError } from "../../../../../../shared/utils/handle-http-errors.util";
import { CreateCashoutCodeRequest } from "../../../../models/cashout-code.model";
import { catchError, of } from "rxjs";
import { CashoutCodeService } from "../../../../services/cashout-code.service";
import { ToastService } from "../../../../../../shared/components/toast/toast.service";
import { KitDropdownOption } from "../../../../../../shared/components/kit-dropdown/kit-dropdown.component";


@Component({
      selector: 'cashout-code-request-dialog',
      standalone: true,
      imports: [ CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule,
            MatInputModule, MatCheckboxModule, MatButtonModule, MatSelectModule ],
      templateUrl: './cashout-code-request-dialog.component.html',
      styleUrl: './cashout-code-request-dialog.component.scss',
})
export class CashoutCodeRequestDialogComponent implements OnInit {
      private formBuilder = inject(FormBuilder);
      private dialogRef = inject(MatDialogRef<CashoutCodeRequestDialogComponent>);
      private cashoutCodeService = inject(CashoutCodeService);
      private cashoutGroupService = inject(CashoutGroupService);
      private ledgerAccountService = inject(LedgerAccountService);
      private toast = inject(ToastService);

      cashoutGroupOptions: KitDropdownOption[] = [];
      postingLedgerAccountOptions: KitDropdownOption[] = [];

      submitting = false;

      form = this.formBuilder.group({
            name: ['', { validators: [Validators.required, Validators.maxLength(250)], updateOn: 'blur' }],
            cashoutGroupId: [''],
            cashoutGroupName: [''],
            postingLedgerAccountId: [''],
            description: [''],
            isActive: [true]
      });
      get cashoutCodeRequestForm() { return this.form.controls };

      ngOnInit(): void {
            this.loadCashoutGroupOptions();
            this.loadLedgerAccountOptions();
      }

      loadCashoutGroupOptions(): void {
            this.cashoutGroupService.getAll().subscribe({
                  next: (cashoutGroups) => {
                        this.cashoutGroupOptions = cashoutGroups.map(cg => ({
                              id: cg.id,
                              label: `${cg.code} - ${cg.name}`
                        }))
                  }, 
                  error: (err => handleHttpError(err))
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

      async save(): 

      cancel(): void {
            this.dialogRef.close();
      }
}