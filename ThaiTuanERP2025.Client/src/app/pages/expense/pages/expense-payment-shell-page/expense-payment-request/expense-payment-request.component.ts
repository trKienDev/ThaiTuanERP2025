import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ConnectedPosition, OverlayModule } from "@angular/cdk/overlay";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { Subject, Observable, firstValueFrom, startWith, switchMap, of, takeUntil } from "rxjs";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { HttpClientModule } from "@angular/common/http";
import { ConfirmService } from "../../../../../shared/components/confirm-dialog/confirm.service";
import { KitDropdownComponent, KitDropdownOption } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { provideMondayFirstDateAdapter } from "../../../../../shared/date/provide-monday-first-date-adapter";
import { MoneyFormatDirective } from "../../../../../shared/directives/money/money-format.directive";
import { FileService } from "../../../../../shared/services/file.service";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { UserFacade } from "../../../../account/facades/user.facade";
import { UserDto } from "../../../../account/models/user.model";
import { UserOptionStore } from "../../../../account/options/user-dropdown-options.store";
import { ManagerOptionStore } from "../../../../account/options/user-manager-option.store";
import { BudgetCodeService } from "../../../../finance/services/budget-code.service";
import { CashoutCodeService } from "../../../../finance/services/cashout-code.service";
import { SupplierFacade } from "../../../facades/supplier.facade";
import { BankAccountDto } from "../../../models/bank-account.model";
import { PayeeType, ExpensePaymentRequest } from "../../../models/expense-payment.model";
import { SupplierOptionStore } from "../../../options/supplier-dropdown-option.store";
import { BankAccountService } from "../../../services/bank-account.service";
import { ExpensePaymentService } from "../../../services/expense-payment.service";
import { MiniInvoiceRequestDialogComponent } from "../../invoices/invoice-request/mini-invoice-request-dialog/mini-invoice-request-dialog.component";
import { MyInvoicesDialogComponent } from "../../invoices/my-invoices-dialog/my-invoices-dialog.component";
import { ExpenseBudgetCodeDialogComponent } from "../../../../finance/pages/budgets-shell-page/budget-codes/expense-budget-code/expense-budget-code.component";
import { SupplierRequestDialogComponent } from "../../suppliers/supplier-request-dialog/supplier-request-dialog.component";
import { KitFileUploaderComponent } from "../../../../../shared/components/kit-file-uploader/kit-file-uploader.component";

type UploadStatus = 'queued' | 'uploading' | 'done' | 'error';
type UploadItem = {
      file: File;
      name: string;
      size: number;
      progress: number;    // 0..100
      status: UploadStatus;
      objectKey?: string;  // trả về từ server khi thành công
      fileId?: string;
      url?: string;
};

type PaymentItem = {
      itemName: FormControl<string>;
      invoiceId: FormControl<string | null>;
      quantity: FormControl<number | null>;
      unitPrice: FormControl<number | null>;
      taxRate: FormControl<number>;
      amount: FormControl<number>; // readonly
      taxRatePercent: FormControl<string>;
      taxAmount: FormControl<number>; // readonly
      totalWithTax: FormControl<number>; // readonly
      budgetCodeId: FormControl<string | null>; 
      cashoutCodeId: FormControl<string | null>;
};

@Component({
      selector: 'new-expense-payment-request',
      templateUrl: './expense-payment-request.component.html',
      imports: [CommonModule, ReactiveFormsModule, MatInputModule, MatFormFieldModule,
      KitDropdownComponent, MatDialogModule, MoneyFormatDirective, OverlayModule, MatSnackBarModule,
      MatDatepickerModule, HttpClientModule, KitFileUploaderComponent],
      styleUrls: ['./expense-payment-request.component.scss'],
      standalone: true,
      providers: [...provideMondayFirstDateAdapter() ]
})
export class ExpensePaymentRequestPanelComponent {
      private destroy$ = new Subject<void>();
      private formBuilder = inject(FormBuilder);
      private taxRateById: Record<string, number> = {};
      private dialog = inject(MatDialog);
      private toast = inject(ToastService);
      private fileService = inject(FileService);
      private userOptionsStore = inject(UserOptionStore);
      private supplierOptionStore = inject(SupplierOptionStore);
      private managerOptionStore = inject(ManagerOptionStore);
      private userFacade = inject(UserFacade);
      private supplierFacade = inject(SupplierFacade);
      public submitting = false;
      private readonly expensePaymentService = inject(ExpensePaymentService);

      // private wait(ms: number) { return new Promise(res => setTimeout(res, ms)); } // demo
      // private readonly DEMO_MIN_LOADING_MS = 300000000; // demo

      public readonly uploadMeta = {
            module: 'expense',
            entity: 'payment-attachment',
            entityId: undefined as string | undefined,
            isPublic: false
      }
      public uploads: UploadItem[] = [];

      userOptions$ = this.userOptionsStore.option$; 
      supplierOptions$ = this.supplierOptionStore.option$;
      suppliers$ = this.supplierFacade.suppliers$;
      currentUser$ = this.userFacade.currentUser$;
      currentUser: UserDto | null = null;
      managerOptions$!: Observable<KitDropdownOption[]>;
      supplierOptions: KitDropdownOption[] = [];
      currencyOptions: KitDropdownOption[] = [];
      budgetCodeOptiopns: KitDropdownOption[] = [];
      cashoutCodeOptions: KitDropdownOption[] = [];

      supplierBankAccounts: BankAccountDto[] = [];
      selectedBankAccount: BankAccountDto | null = null;     

      constructor(
            private bankAccountService: BankAccountService,
            private budegetCodeService: BudgetCodeService,
            private cashoutCodeService: CashoutCodeService,
            private confirmService: ConfirmService,
      ) {

      }

      // reactive form
      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            supplierId: this.formBuilder.control<string | null>(null),
            bankName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            accountNumber: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            beneficiaryName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            description: this.formBuilder.control<string>(''),
            items: this.formBuilder.array<FormGroup<PaymentItem>>([ this.newItemGroup() ]),
            totalAmount: this.formBuilder.nonNullable.control<number>(0),
            totalTax: this.formBuilder.nonNullable.control<number>(0),
            totalWithTax: this.formBuilder.nonNullable.control<number>(0),
            paymentDate: this.formBuilder.nonNullable.control<Date>(new Date(), { validators: [Validators.required] }),
            hasGoodsReceipt: this.formBuilder.nonNullable.control<boolean>(false),
            followerIds: this.formBuilder.nonNullable.control<string[]>([]),
            managerApproverId: this.formBuilder.nonNullable.control<string>('', { validators: Validators.required })
      });

      async ngOnInit(): Promise<void> {
            this.currentUser = await firstValueFrom(this.currentUser$);
            this.managerOptions$ = this.managerOptionStore.getManagerOptions$(this.currentUser.id);

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


      onSupplierSelected(opt: KitDropdownOption) {
            this.form.patchValue({ supplierId: opt.id });
      }

      onUserSelected(opt: KitDropdownOption) {
            alert(`Bạn đã chọn: ${opt.label} (id = ${opt.id})`);
      }
      onFollowerSelected(opt: KitDropdownOption) {
            const id = typeof opt === 'string' ? opt : opt.id;
            const ctrl = this.form.controls.followerIds;
            const current = ctrl.getRawValue() ?? [];
            if (!current.includes(id)) ctrl.setValue([...current, id]);

            ctrl.markAsDirty();
            ctrl.updateValueAndValidity();
      }
      onManagerApproverSelected(opt: KitDropdownOption) {
            this.form.patchValue({ managerApproverId: opt.id });
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

                  const ref = this.dialog.open(MiniInvoiceRequestDialogComponent);

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

                  const ref = this.dialog.open(MyInvoicesDialogComponent);

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
      selectedPayee: PayeeType | null = null;
      onPayeeSelected(opt: KitDropdownOption) {
            this.selectedPayee = opt.id === 'supplier' ? PayeeType.supplier : PayeeType.employee;
            if (this.selectedPayee === PayeeType.employee) {
                  this.form.patchValue({ supplierId: null, bankName:'', accountNumber:'', beneficiaryName:'' }, { emitEvent:false });
                  this.supplierBankAccounts = [];
                  this.selectedBankAccount = null;
            }
      }

      goodsReceiptOptions: KitDropdownOption[] = [
            { id: 'true', label: 'Có nhập kho' },
            { id: 'false', label: 'Không nhập kho' },
      ]
      onGoodsReceiptSelected(opt: KitDropdownOption) {
            if(opt.id === 'true') {
                  this.form.patchValue({ hasGoodsReceipt: true });
            } else {
                  this.form.patchValue({ hasGoodsReceipt: false });
            }
      } 

      openCreateSupplierDialog(): void {
            const dialogRef = this.dialog.open(SupplierRequestDialogComponent);

            dialogRef.afterClosed().subscribe((created) => {
                  if(created?.id) {
                        this.selectedPayee = PayeeType.supplier;
                        this.form.patchValue({ supplierId: created.id });
                  }
            })
      }

      openExpenseBudgetCodeDialog(rowIndex: number): void {
            const row = this.items.at(rowIndex);
            const selectedId = row.get('budgetCodeId')?.value;

            const dialogRef = this.dialog.open(ExpenseBudgetCodeDialogComponent, {
                  data: { selectedBudgetCodeId: selectedId },
            });

            dialogRef.afterClosed().subscribe((result: { success?: boolean, budgetCodeId?: string, budgetAmount?: number } | undefined) => {
                  if(!result?.success || !result.budgetCodeId || !result.budgetAmount) return;
                  
                  const budgetAmount = Number(result.budgetAmount);
                  const totalWithTax = Number(row.get('totalWithTax')?.value ?? 0);

                  // Cho phép vượt 5%
                  const tolerance = 0.05; // 5%
                  const allowed = Math.round(budgetAmount * (1 + tolerance));

                  if (totalWithTax > allowed) {
                        this.confirmService.validateBudgetLimit$({
                              message: `Khoản thanh toán vượt quá ngân sách cho phép`
                        }).subscribe(ok => {
                              if (!ok) return;
                        });
                        return; 
                  }

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
                  taxRatePercent: this.formBuilder.nonNullable.control<string>('0'),  
                  amount: this.formBuilder.nonNullable.control<number>(0),
                  taxAmount: this.formBuilder.nonNullable.control<number>(0),
                  totalWithTax: this.formBuilder.nonNullable.control<number>(0),
                  budgetCodeId: this.formBuilder.control<string | null>(null),
                  cashoutCodeId: this.formBuilder.control<string | null>(null),
            });

            // đồng bộ: khi user gõ %
            group.controls.taxRatePercent.valueChanges.subscribe(txt => {
                  if (txt == null || txt.trim() === '') {
                        group.controls.taxRate.setValue(0, { emitEvent: false });
                        return;
                  }
                  const raw = Number(txt);
                  if (isNaN(raw)) return;
                  const frac = Math.max(0, Math.min(1, raw / 100));
                  group.controls.taxRate.setValue(frac, { emitEvent: true });
            });

            group.controls.taxRate.valueChanges.subscribe(frac => {
                  const percent = Math.round(frac * 100 * 100) / 100; // 2 số thập phân
                  group.controls.taxRatePercent.setValue(String(percent), { emitEvent: false });
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
            const row = this.items.at(i);
            const current = row.get('invoiceId')!.value;

            if (!current) {
                  this.toast.info?.('Dòng này chưa liên kết hóa đơn');    // ToastService đã inject sẵn :contentReference[oaicite:1]{index=1}
                  return;
            }
            row.patchValue({ invoiceId: null }, { emitEvent: true });

            this.onMenuClosed?.(); 
            this.toast.successRich?.('Đã gỡ liên kết hóa đơn');  
      }

      async Submit(): Promise<void> {
            // chặn double-click & chặn khi vẫn còn upload file
            if (this.isBusy) return;

            if(this.form.invalid) {
                  this.toast.errorRich('Vui lòng nhập đầy đủ thông tin');
                  return;
            }

            const raw = this.form.getRawValue();
            const items = (raw.items ?? []).map(it => ({
                  itemName: it.itemName,
                  invoiceId: it.invoiceId ?? undefined,
                  budgetCodeId: it.budgetCodeId ?? undefined,
                  cashoutCodeId: it.cashoutCodeId ?? undefined,
                  quantity: Number(it.quantity ?? 0),
                  unitPrice: Number(it.unitPrice ?? 0),
                  taxRate: Number(it.taxRate ?? 0),
                  amount: Number(it.amount ?? 0),
                  taxAmount: Number(it.taxAmount ?? 0),
                  totalWithTax: Number(it.totalWithTax ?? 0),
            }));

            const attachments = this.uploads.filter(u => u.status === 'done' && (u.objectKey || u.fileId))
                  .map(u => ({
                        fileId: u.fileId,
                        objectKey: u.objectKey!,
                        fileName: u.name,
                        size: u.size,
                        url: u.url,
                  }));
            
            const payload: ExpensePaymentRequest = {
                  name: raw.name,
                  payeeType: this.selectedPayee ?? PayeeType.supplier,
                  supplierId: this.selectedPayee === PayeeType.supplier ? raw.supplierId ?? undefined : undefined,
                  bankName: raw.bankName,
                  accountNumber: raw.accountNumber,
                  description: raw.description ?? '',
                  beneficiaryName: raw.beneficiaryName,
                  paymentDate: raw.paymentDate,
                  hasGoodsReceipt: raw.hasGoodsReceipt ?? false,
                  totalAmount: Number(raw.totalAmount ?? 0),
                  totalTax: Number(raw.totalTax ?? 0),
                  totalWithTax: Number(raw.totalWithTax ?? 0),
                  status: 1, // ExpensePaymentStatus.submitted
                  items,
                  attachments,
                  followerIds: raw.followerIds,
                  managerApproverId: raw.managerApproverId,
            }

            this.submitting = true;
            try {
                  const result = await firstValueFrom(this.expensePaymentService.create(payload));

                  // demo
                  // await Promise.all([
                  //       firstValueFrom(this.expensePaymentService.create(payload)),
                  //       this.wait(this.DEMO_MIN_LOADING_MS)   // đảm bảo chờ ít nhất 3s
                  // ]);

                  this.toast.successRich('Gửi phê duyệt thành công');
            } catch(error) {
                  console.error('Gửi phê duyệt thất bại', error);
                  this.toast.errorRich('Gửi phê duyệt thất bại');
            } finally {
                  // demo 
                  // await this.wait(800);
                  this.submitting = false;
            }
      }
      get isUploading() {
            return this.uploads?.some(u => u.status === 'uploading');
      }
      // 3. trạng thái bận chung
      get isBusy() {

            return this.submitting || this.isUploading;
      }
}