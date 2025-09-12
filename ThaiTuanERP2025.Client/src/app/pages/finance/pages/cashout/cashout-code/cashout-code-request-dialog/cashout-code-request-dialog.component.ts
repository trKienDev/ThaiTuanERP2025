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

      saving = false;
      errorMessages: string[] = [];

      cashoutGroupOptions: CashoutGroupDto[] = [];
      private cashoutGroupService = inject(CashoutGroupService);
      
      ledgerAccountOptions: LedgerAccountDto[] = [];
      private ledgerAccountService = inject(LedgerAccountService);

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
                  next: (data) => {
                        this.cashoutGroupOptions = data;
                  }, 
                  error: err => alert(handleHttpError(err).join('\n'))
            })
      }
      loadLedgerAccountOptions(): void {
            this.ledgerAccountService.getAll().subscribe({
                  next: (data) => {
                        this.ledgerAccountOptions = data;
                  }, 
                  error: err => alert(handleHttpError(err).join('\n'))
            })
      }

      submit(): void {
            this.errorMessages = [];
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  return;
            }

            const payload = this.form.getRawValue() as CreateCashoutCodeRequest;

            this.saving = true;
            this.cashoutCodeService.create(payload).pipe(
                  catchError(err => {
                        this.errorMessages = handleHttpError(err);
                        this.saving = false;
                        return of(null);
                  })
            ).subscribe((created) => {
                  if(!created) return;
                  this.dialogRef.close('created');
            })
      }

      cancel(): void {
            this.dialogRef.close();
      }
}