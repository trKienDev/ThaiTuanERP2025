import { CommonModule } from "@angular/common";
import { Component, inject, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, NonNullableFormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { KitDropdownComponent, KitDropdownOption } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { SupplierService } from "../../../services/supplier.service";
import { UserService } from "../../../../account/services/user.service";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { SupplierRequestDialogComponent } from "../../suppliers/supplier-request-dialog/supplier-request-dialog.component";
import { startWith, switchMap, takeUntil } from "rxjs/operators"; // <-- thêm
import { of, Subject } from "rxjs";
import { BankAccountService } from "../../../services/bank-account.service";
import { BankAccountDto } from "../../../models/bank-account.model";

@Component({
      selector: 'expense-payment',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MatInputModule, MatFormFieldModule,
            KitDropdownComponent, MatDialogModule

      ],
      templateUrl: './expense-payment.component.html',
      styleUrl: './expense-payment.component.scss',
})
export class ExpensePaymentComponent implements OnInit, OnDestroy {
      private destroy$ = new Subject<void>();
      private formBuilder = inject(FormBuilder);
      
      supplierOptions: KitDropdownOption[] = [];
      userOptions: KitDropdownOption[] = [];

      supplierBankAccounts: BankAccountDto[] = [];
      selectedBankAccount: BankAccountDto | null = null;

      constructor(
            private supplierService: SupplierService,
            private userService: UserService,
            private bankAccountService: BankAccountService,
            private dialog: MatDialog,
      ) {}

      // reactive form
      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            supplierId: this.formBuilder.control<string | null>(null),
            bankName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            accountNumber: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            beneficiaryName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
      })

      ngOnInit(): void {
            this.loadSuppliers();
            this.loadUsers();

            // Khi supplierId đổi → load bank accounts → chọn mặc định → patch vào form
            this.form.get('supplierId')!.valueChanges.pipe(
                  startWith(this.form.get('supplierId')!.value), 
                  switchMap(id => id ? this.bankAccountService.listBySupplier(id) : of([])),
                  takeUntil(this.destroy$)
            ).subscribe(accounts => {
                  this.supplierBankAccounts = accounts ?? [];
                  this.selectedBankAccount = this.pickDefulatBankAccount(this.supplierBankAccounts);
                  this.applyBankAccountToForm(this.selectedBankAccount);
            });
      }

      // Ưu tiên account đang active, không có thì lấy cái đầu
      private pickDefulatBankAccount(list: Array<{ isActive?: boolean } & any>): any | null {
            return list.find(a => !!a.isActive) ?? list[0] ?? null;
      }
      // Patch 3 control theo selectedBankAccount (hoặc clear nếu null)
      private applyBankAccountToForm(acc: BankAccountDto | null) {
            this.form.patchValue({
                  bankName: acc?.bankName ?? '',
                  accountNumber: acc?.accountNumber ?? '',
                  beneficiaryName: acc?.beneficiaryName ?? '',
            }, { emitEvent: false }); // tránh vòng lặp valueChanges không cần thiết
      }

      ngOnDestroy(): void {
            this.destroy$.next();
            this.destroy$.complete();
      }

      loadSuppliers(): void {
            this.supplierService.getAll().subscribe({
                  next: (suppliers) =>  {
                        this.supplierOptions = suppliers.map(s => ({
                              id: s.id,
                              label: s.name,
                        }));
                  }
            });
      }
      onSupplierSelected(opt: KitDropdownOption) {
            this.form.patchValue({ supplierId: opt.id });
      }

      loadUsers(): void {
            this.userService.getAllUsers().subscribe({
                  next: (users) => {
                        this.userOptions = users.map(s => ({
                              id: s.id,
                              label: s.fullName,
                        }));
                  }
            })
      }
      onUserSelected(opt: KitDropdownOption) {
            alert(`Bạn đã chọn: ${opt.label} (id = ${opt.id})`);
      }



      payeeOptions: KitDropdownOption[] = [
            { id: 'supplier', label: 'Nhà cung cấp' },
            { id: 'employee', label: 'Nhân viên' },
      ];
      selectedPayee: 'supplier' | 'employee' | null = null;
      onPayeeSelected(opt: KitDropdownOption) {
            this.selectedPayee = opt.id === 'supplier' ? 'supplier' : 'employee';
            if (this.selectedPayee === 'employee') {
                  this.form.patchValue({
                        supplierId: null,
                        bankName: '',
                        accountNumber: '',
                        beneficiaryName: ''
                  }, { emitEvent: false });
                  this.supplierBankAccounts = [];
                  this.selectedBankAccount = null;
            }
      }

      openCreateSupplierDialog(): void {
            const dialogRef = this.dialog.open(SupplierRequestDialogComponent, {
                  width: '520px',
                  disableClose: true,
            });

            dialogRef.afterClosed().subscribe((created) => {
                  if(created?.id) {
                        this.selectedPayee = 'supplier';
                        this.form.patchValue({ supplierId: created.id });
                        this.loadSuppliers();
                  }
            })
      }
}
