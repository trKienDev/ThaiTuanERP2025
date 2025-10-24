import { CommonModule } from "@angular/common";
import { Component, EventEmitter, inject, Input, Output } from "@angular/core";
import { FormBuilder, Validators, FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BankAccountService } from "../../../services/bank-account.service";
import { catchError, of } from "rxjs";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";

@Component({
      selector: 'bank-account-request',
      standalone: true,
      imports: [CommonModule, FormsModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule,
            MatButtonModule
      ],
      templateUrl: './bank-account-request-drawer.component.html',
      styleUrl: './bank-account-request-drawer.component.scss',
})
export class BankAccountRequestDrawerComponent {
      private formBuilder = inject(FormBuilder);
      private bankAccountService = inject(BankAccountService);

      @Input({ required: true }) ownerType!: 'user' | 'supplier';
      @Input({ required: true }) ownerId!: string;
      @Output() closed = new EventEmitter<'created' | 'cancel'>();

      saving = false;
      errorMessages: string[] = [];

      form = this.formBuilder.group({
            bankName: ['', [Validators.required, Validators.maxLength(128)]],
            accountNumber: ['', [Validators.required, Validators.maxLength(64) ]],
            beneficiaryName: ['', [Validators.required, Validators.maxLength(128) ]],
      });
      get bankAccountForm() { return this.form.controls; }

      get title(): string {
            return this.ownerType === 'user' ? 'Thêm tài khoản ngân hàng cho người dùng' : 'Thêm tài khoản ngân hàng cho nhà cung cấp';
      }

      submit() {
            this.errorMessages = [];
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  return;
            }

            const body = {
                  bankName: this.form.value.bankName!,
                  accountNumber: this.form.value.accountNumber!,
                  beneficiaryName: this.form.value.beneficiaryName!,
            };

            this.saving = true;
            
            const req$ = this.ownerType === 'user' ? this.bankAccountService.createForUser({ userId: this.ownerId, ...body })
                  : this.bankAccountService.createForSupplier({ supplierId: this.ownerId, ...body });
            req$.pipe(
                  catchError((err) => {
                        this.errorMessages = this.normalizeErrors(err);
                        this.saving = false;
                        return of(null);
                  })
            ).subscribe((res) => {
                  if(!res) return;
                  this.saving = false;
                  this.closed.emit('created')
            });
      }

      cancel() {
            this.closed.emit('cancel');
      }

      private normalizeErrors(err: any): string[] {
            const msgs: string[] = [];
            const message = err?.error?.message || err?.message || 'Đã có lỗi xảy ra';
            if(message) msgs.push(message);
            const errors = err?.error?.errors as string[] | undefined;
            if(Array.isArray(errors)) msgs.push(...errors);
            return msgs;
      }


}