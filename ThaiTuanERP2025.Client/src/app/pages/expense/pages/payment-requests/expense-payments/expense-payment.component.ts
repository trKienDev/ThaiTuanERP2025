import { CommonModule } from "@angular/common";
import { Component, inject, OnDestroy, OnInit } from "@angular/core";
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
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
import { MoneyFormatDirective } from "../../../../../shared/directives/money/money-format.directive";
import { TaxService } from "../../../../finance/services/tax.service";
import { handleHttpError } from "../../../../../core/utils/handle-http-errors.util";
import { BudgetCodeService } from "../../../../finance/services/budget-code.service";
import { CashoutCodeService } from "../../../../finance/services/cashout-code.service";
import { MiniInvoiceRequestDialogComponent } from "../../invoices/invoice-request/mini-invoice-request-dialog/mini-invoice-request-dialog.component";
import { ConnectedPosition, OverlayModule } from "@angular/cdk/overlay";
import { MyInvoicesDialogComponent } from "../../invoices/my-invoices-dialog/my-invoices-dialog.component";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { ConfirmService } from "../../../../../shared/services/confirm.service";
import { ExpenseBudgetCodeDialogComponent } from "./expense-budget-code/expense-budget-code.component";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { DateAdapter } from "@angular/material/core";
import { provideMondayFirstDateAdapter } from "../../../../../shared/date/provide-monday-first-date-adapter";

type PaymentItem = {
      itemName: FormControl<string>;
      invoiceId: FormControl<string | null>;
      quantity: FormControl<number | null>;
      unitPrice: FormControl<number | null>;
      taxRate: FormControl<number>;
      amount: FormControl<number>; // readonly
      taxAmount: FormControl<number>; // readonly
      totalWithTax: FormControl<number>; // readonly
      budgetCodeId: FormControl<string | null>; 
      cashoutCodeId: FormControl<string | null>;
};

@Component({
      selector: 'expense-payment',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MatInputModule, MatFormFieldModule,
            KitDropdownComponent, MatDialogModule, MoneyFormatDirective, OverlayModule, MatSnackBarModule,
            MatDatepickerModule
      ],
      templateUrl: './expense-payment.component.html',
      styleUrl: './expense-payment.component.scss',
      providers: [...provideMondayFirstDateAdapter() ]
})
export class ExpensePaymentComponent implements OnInit, OnDestroy {
      private destroy$ = new Subject<void>();
      private formBuilder = inject(FormBuilder);
      private taxRateById: Record<string, number> = {};
      private dialog = inject(MatDialog);
      private toast = inject(ToastService);
      private adapter = inject<DateAdapter<Date>>(DateAdapter as any);
      
      supplierOptions: KitDropdownOption[] = [];
      userOptions: KitDropdownOption[] = [];
      currencyOptions: KitDropdownOption[] = [];
      taxOptions: KitDropdownOption[] = [];
      budgetCodeOptiopns: KitDropdownOption[] = [];
      cashoutCodeOptions: KitDropdownOption[] = [];

      supplierBankAccounts: BankAccountDto[] = [];
      selectedBankAccount: BankAccountDto | null = null;

      constructor(
            private supplierService: SupplierService,
            private userService: UserService,
            private bankAccountService: BankAccountService,
            private taxService: TaxService,
            private budegetCodeService: BudgetCodeService,
            private cashoutCodeService: CashoutCodeService,
            private confirmService: ConfirmService,
      ) {}

      // reactive form
      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            supplierId: this.formBuilder.control<string | null>(null),
            bankName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            accountNumber: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            beneficiaryName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            items: this.formBuilder.array<FormGroup<PaymentItem>>([ this.newItemGroup() ]),
            totalAmount: this.formBuilder.nonNullable.control<number>(0),
            totalTax: this.formBuilder.nonNullable.control<number>(0),
            totalWithTax: this.formBuilder.nonNullable.control<number>(0),
            paymentDate: [ null as unknown as string | null, [ Validators.required ]],
      });

      ngOnInit(): void {
            this.loadSuppliers();
            this.loadUsers();
            this.loadTaxes();
            this.loadBudgetCodes();
            this.loadCashoutCodeOptions();

            // Khi supplierId đổi → load bank accounts → chọn mặc định → patch vào form
            this.form.get('totalAmount')!.disable({ emitEvent: false });
            this.form.get('totalTax')!.disable({ emitEvent: false });
            this.form.get('totalWithTax')!.disable({ emitEvent: false });
            
            this.form.get('supplierId')!.valueChanges.pipe(
                  startWith(this.form.get('supplierId')!.value), 
                  switchMap(id => id ? this.bankAccountService.listBySupplier(id) : of([])),
                  takeUntil(this.destroy$)
            ).subscribe(accounts => {
                  this.supplierBankAccounts = accounts ?? [];
                  this.selectedBankAccount = this.pickDefulatBankAccount(this.supplierBankAccounts);
                  this.applyBankAccountToForm(this.selectedBankAccount);
            });
            

            this.form.get('items')!.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(() => {
                  console.log('change');
                  const totalAmount = this.totalAmount;
                  this.form.get('totalAmount')!.setValue(totalAmount, { emitEvent: false })

                  const totalTax = this.totalTax;
                  this.form.get('totalTax')!.setValue(totalTax, { emitEvent: false });
                  
                  const totalWithTax = this.totalWithTax;
                  this.form.get('totalWithTax')!.setValue(totalWithTax, { emitEvent: false });
            })
            
            this.currencyOptions = [
                  { id: 'vnd', label: 'VND' }
            ]
            
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
                        this.userOptions = users.map(u => ({
                              id: u.id,
                              label: u.fullName,
                        }));
                  }
            })
      }
      onUserSelected(opt: KitDropdownOption) {
            alert(`Bạn đã chọn: ${opt.label} (id = ${opt.id})`);
      }

      loadTaxes(): void {
            this.taxService.getAll().subscribe({
                  next: (taxes) => {
                        this.taxOptions = taxes.map(t => ({ id: t.id, label: t.policyName, }));
                        this.taxRateById = Object.fromEntries(taxes.map(t => [t.id, t.rate ]));
                  }, 
                  error: (err => handleHttpError(err))
            })
      }
      onTaxSelected(opt: KitDropdownOption, rowIndex: number) {
           const rate = this.taxRateById[opt.id];
           if(rate === undefined) return;
           
           const row = this.items.at(rowIndex);
           row.get('taxRate')!.setValue(rate);
      }

      loadBudgetCodes(): void {
            this.budegetCodeService.getAll().subscribe({
                  next: (budgetCodes) => {
                        this.budgetCodeOptiopns = budgetCodes.map(bc => ({
                              id: bc.id,
                              label: `${bc.code} - ${bc.name}`
                        }));
                  }, 
                  error: (err => handleHttpError(err))
            })
      }

      loadCashoutCodeOptions(): void {
            this.cashoutCodeService.getAll().subscribe({
                  next: (cashoutCodes) => {
                        this.cashoutCodeOptions = cashoutCodes.map(co => ({
                              id: co.id,
                              label: co.name
                        }));
                  },
                  error: (err => handleHttpError(err))
            })
      }

      labelOf = (opts: KitDropdownOption[], id: string | null): string => {
            if(!id) return '';
            return opts.find(o => o.id === id)?.label ?? '';
      }

      async openMiniInvoiceRequestDialog(rowIndex: number) {
            const row = this.items.at(rowIndex);
            const oldId = row.get('invoiceId')!.value;

            this.confirmService.confirmReplaceInvoice$(!!oldId).subscribe(ok => {
                  if (!ok) return;

                  const ref = this.dialog.open(MiniInvoiceRequestDialogComponent, {
                        width: 'fit-content',
                        height: 'fit-content',
                        maxWidth: '90vw',
                        maxHeight: '80vh',
                        disableClose: true,
                  });

                  ref.afterClosed().subscribe((result?: { success?: boolean; invoiceId?: string }) => {
                        if (result?.success && result.invoiceId) {
                              if (oldId === result.invoiceId) return;
                              row.patchValue({ invoiceId: result.invoiceId }, { emitEvent: true });
                        }
                  });
            });
      }

      async openMyInvoicesDialog(rowIndex: number) {
            const row = this.items.at(rowIndex);
            const oldId = row.get('invoiceId')!.value;

            this.confirmService.confirmReplaceInvoice$(!!oldId).subscribe(ok => {
                  if(!ok) return;

                  const ref = this.dialog.open(MyInvoicesDialogComponent, {
                        width: 'fit-content',
                        height: 'fit-content',
                        maxWidth: '90vw',   
                        maxHeight: '80vh',
                        disableClose: true,
                  });

                  ref.afterClosed().subscribe((result: { success?: boolean; invoiceId?: string } | undefined) => {
                        if (!result?.success || !result.invoiceId) return;
                        
                        if (oldId === result.invoiceId) return;
                        row.patchValue({ invoiceId: result.invoiceId }, { emitEvent: true });
                        this.toast.successRich('Đã chọn hóa đơn');
                  });
            });

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

      openExpenseBudgetCodeDialog(rowIndex: number): void {
            const row = this.items.at(rowIndex);
            const selectedId = row.get('budgetCodeId')?.value;

            const dialogRef = this.dialog.open(ExpenseBudgetCodeDialogComponent, {
                  width: 'fit-content',
                  height: 'fit-content',
                  maxWidth: '90vw',   
                  maxHeight: '80vh',
                  disableClose: true,
                  data: { selectedBudgetCodeId: selectedId },
            });

            dialogRef.afterClosed().subscribe((result: { success?: boolean, budgetCodeId?: string } | undefined) => {
                  if(!result?.success || !result.budgetCodeId) return;
                  row.patchValue({ budgetCodeId: result.budgetCodeId }, { emitEvent: true });
                  this.toast.successRich('Đã chọn mã ngân sách');
            });
      }

      newItemGroup(): FormGroup<PaymentItem> {
            const group = this.formBuilder.group<PaymentItem>({
                  itemName: this.formBuilder.nonNullable.control<string>('', [Validators.required, Validators.maxLength(256)]),
                  invoiceId: this.formBuilder.control<string | null>(null),
                  quantity: this.formBuilder.control<number | null>(null, { validators: [Validators.required, Validators.pattern('^[0-9]+$'), Validators.min(1)] }),
                  unitPrice: this.formBuilder.control<number | null>(null, { validators: [Validators.required, Validators.min(0)] }),
                  taxRate: this.formBuilder.nonNullable.control<number>(0), // mặc định 10%
                  amount: this.formBuilder.nonNullable.control<number>(0),
                  taxAmount: this.formBuilder.nonNullable.control<number>(0),
                  totalWithTax: this.formBuilder.nonNullable.control<number>(0),
                  budgetCodeId: this.formBuilder.control<string | null>(null),
                  cashoutCodeId: this.formBuilder.control<string | null>(null),
            });

            // Tính tự động nhưng KHÔNG đè giá trị user đã sửa (dựa theo .dirty)
            group.valueChanges.subscribe(v => {
                  const quantity = Number(v.quantity ?? 0);
                  const price = Number(v.unitPrice ?? 0);
                  const rate = Number(v.taxRate ?? 0);

                  const amount = quantity * price;
                  // const suggestedTax = amount * rate;
                  const suggestedTax = Math.round(amount * rate); 

                  group.controls.amount.setValue(amount, { emitEvent: false });

                  // Nếu người dùng CHƯA sửa taxAmount (control chưa dirty) → cập nhật theo gợi ý
                  const taxCtrl = group.controls.taxAmount;
                  if (!taxCtrl.dirty && Number(taxCtrl.value ?? 0) !== suggestedTax) {
                        // emitEvent: true để appMoney bắt valueChanges và format ngay
                        taxCtrl.setValue(suggestedTax, { emitEvent: true });
                  }

                  // totalWithTax = amount + taxAmount (dù là auto hay user sửa tay)
                  const taxVal = Number(taxCtrl.value ?? 0);
                  group.controls.totalWithTax.setValue(amount + taxVal, { emitEvent: false });
            });

            // các cột tính toán chỉ hiển thị (không cho nhập)
            group.controls.amount.disable({ emitEvent: false });
            group.controls.totalWithTax.disable({ emitEvent: false });

            return group;
      }

      get items(): FormArray<FormGroup<PaymentItem>> {
            return this.form.controls.items;
      }
      get totalAmount(): number {
            return this.items.controls.reduce((s, g) => s + (Number(g.get('amount')?.value) || 0), 0);
      }
      get totalTax(): number {
            return this.items.controls.reduce((s, g) => s + (Number(g.get('taxAmount')?.value) || 0), 0);
      }
      get totalWithTax(): number {
            return this.items.controls.reduce((s, g) => s + (Number(g.get('totalWithTax')?.value) || 0), 0);
      }

      addItem(): void {
            this.items.push(this.newItemGroup());
      }
      removeItem(i: number): void {
            this.items.removeAt(i);
      }
      trackByIndex = (_: number, __: any) => _;


      invoiceMenuOpenIndex: number | null = null;
      invoiceMenuOverlayPosition: ConnectedPosition[] = [
            { originX: 'center', originY: 'bottom', overlayX: 'center', overlayY: 'top', offsetY: 8 },
            { originX: 'end', originY: 'top',    overlayX: 'end',    overlayY: 'bottom', offsetY: -8 },
      ]
      toggleInvoiceMenu(i: number, ev: MouseEvent) {
      ev.stopPropagation();
            this.invoiceMenuOpenIndex = (this.invoiceMenuOpenIndex === i) ? null : i;
      }

      onMenuClosed() {
            this.invoiceMenuOpenIndex = null;
      }

      unlinkInvoice(i: number) {
            console.log('unlink Invoice');
            const row = this.items.at(i);
            const current = row.get('invoiceId')!.value;

            if (!current) {
                  console.log('dòng này chưa liên kết hóa đơn');
                  this.toast.info?.('Dòng này chưa liên kết hóa đơn');    // ToastService đã inject sẵn :contentReference[oaicite:1]{index=1}
                  return;
            }
            row.patchValue({ invoiceId: null }, { emitEvent: true });

            this.onMenuClosed?.(); 
            this.toast.successRich?.('Đã gỡ liên kết hóa đơn');  
      }
}
