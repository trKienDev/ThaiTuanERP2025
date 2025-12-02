import { CommonModule } from "@angular/common";
import { Component, inject, OnDestroy, OnInit } from "@angular/core";
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
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
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { provideMondayFirstDateAdapter } from "../../../../../shared/date/provide-monday-first-date-adapter";
import { MoneyFormatDirective } from "../../../../../shared/directives/money/money-format.directive";
import { UserFacade } from "../../../../account/facades/user.facade";
import { UserDto } from "../../../../account/models/user.model";
import { UserOptionStore } from "../../../../account/options/user-dropdown.option";
import { ManagerOptionStore } from "../../../../account/options/user-manager.option";
import { SupplierFacade } from "../../../facades/supplier.facade";
import { BankAccountDto } from "../../../models/bank-account.model";
import { PayeeType, ExpensePaymentPayload } from "../../../models/expense-payment.model";
import { SupplierOptionStore } from "../../../options/supplier-dropdown-option.store";
import { BankAccountApiService } from "../../../services/bank-account.service";
import { ExpensePaymentApiService } from "../../../services/api/expense-payment.service";
import { KitFileUploaderComponent } from "../../../../../shared/components/kit-file-uploader/kit-file-uploader.component";
import { TextareaNoSpellcheckDirective } from "../../../../../shared/directives/textarea/textarea-no-spellcheck.directive";
import { KitSpinnerButtonComponent } from "../../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { KitOverlaySpinnerComponent } from "../../../../../shared/components/kit-overlay-spinner/kit-overlay-spinner.component";
import { Router } from "@angular/router";
import { SupplierRequestDialogComponent } from "../../../components/supplier-request-dialog/supplier-request-dialog.component";
import { AvailableBudgetPlansDialogComponent } from "../../../components/available-budget-plans-dialog/available-budget-plans-dialog.component";
import { FileImagePreviewDialog } from "../../../../../shared/components/file-preview/file-image-preview-dialog.component";
import { FilePdfPreviewDialog } from "../../../../../shared/components/file-preview/file-pdf-preview-dialog.component";
import { AmountToWordsPipe } from "../../../../../shared/pipes/amount-to-words.pipe";
import { FileService } from "../../../../../shared/services/file.service";
import { ExpensePaymentItemPayload } from "../../../models/expense-payment-item.model";
import { HttpErrorHandlerService } from "../../../../../core/services/http-errror-handler.service";
import { SupplierApiService } from "../../../services/api/supplier.service";

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
      uploadedInvoiceFile: FormControl<File | null>;
      uploadedInvoicePreviewUrl: FormControl<string | null>;
      invoiceStoredFileId: FormControl<string | null>;
      quantity: FormControl<number | null>;
      unitPrice: FormControl<number | null>;
      taxRate: FormControl<number>;
      amount: FormControl<number>; // readonly
      taxRatePercent: FormControl<string>;
      taxAmount: FormControl<number>; // readonly
      totalWithTax: FormControl<number>; // readonly
      budgetPlanDetailId: FormControl<string | null>;
};

@Component({
      selector: 'new-expense-payment-request',
      templateUrl: './expense-payment-request.component.html',
      imports: [CommonModule, ReactiveFormsModule, MatInputModule, MatFormFieldModule, KitDropdownComponent, MatDialogModule, MoneyFormatDirective, OverlayModule, MatSnackBarModule, MatDatepickerModule, HttpClientModule, KitFileUploaderComponent, TextareaNoSpellcheckDirective, KitSpinnerButtonComponent, KitOverlaySpinnerComponent, AmountToWordsPipe],
      styleUrls: ['./expense-payment-request.component.scss'],
      standalone: true,
      providers: [...provideMondayFirstDateAdapter()]
})
export class ExpensePaymentRequestPanelComponent implements OnInit, OnDestroy {
      private readonly destroy$ = new Subject<void>();
      private readonly formBuilder = inject(FormBuilder);
      private readonly dialog = inject(MatDialog);
      private readonly toast = inject(ToastService);
      private readonly managerOptionStore = inject(ManagerOptionStore);
      private readonly userFacade = inject(UserFacade);
      private readonly expensePaymentService = inject(ExpensePaymentApiService);
      private readonly supplierApi = inject(SupplierApiService);
      private readonly router = inject(Router);
      private readonly confirm = inject(ConfirmService);
      private readonly bankAccountApi = inject(BankAccountApiService);
      private readonly fileService = inject(FileService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);

      submitting = false;
      showErrors = false;
      today = new Date();

      public readonly uploadMeta = {
            module: 'expense',
            entity: 'payment-attachment',
            entityId: undefined as string | undefined,
            isPublic: false
      }
      public uploads: UploadItem[] = [];


      userOptions$ = inject(UserOptionStore).option$;
      supplierOptions$ = inject(SupplierOptionStore).option$;
      suppliers$ = inject(SupplierFacade).suppliers$;
      currentUser$ = this.userFacade.currentUser$;
      currentUser: UserDto | null = null;
      managerOptions$!: Observable<KitDropdownOption[]>;
      supplierOptions: KitDropdownOption[] = [];
      currencyOptions: KitDropdownOption[] = [];
      budgetCodeOptiopns: KitDropdownOption[] = [];
      cashoutCodeOptions: KitDropdownOption[] = [];
      supplierBankAccounts: BankAccountDto[] = [];
      selectedBankAccount: BankAccountDto | null = null;

      // reactive form
      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            supplierId: this.formBuilder.control<string | null>(null),
            bankName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            accountNumber: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            beneficiaryName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            description: this.formBuilder.control<string | null>(null),
            items: this.formBuilder.array<FormGroup<PaymentItem>>([this.newItemGroup()]),
            totalAmount: this.formBuilder.nonNullable.control<number>(0),
            totalTax: this.formBuilder.nonNullable.control<number>(0),
            totalWithTax: this.formBuilder.nonNullable.control<number>(0),
            dueDate: this.formBuilder.nonNullable.control<Date>(new Date(), { validators: [Validators.required] }),
            hasGoodsReceipt: this.formBuilder.nonNullable.control<boolean>(false),
            followerIds: this.formBuilder.nonNullable.control<string[]>([]),
            managerApproverId: this.formBuilder.nonNullable.control<string>('', { validators: Validators.required })
      });

      async ngOnInit(): Promise<void> {
            this.managerOptions$ = this.managerOptionStore.getManagerOptions$();

            // Khi supplierId đổi → load bank accounts → chọn mặc định → patch vào form
            this.form.get('totalAmount')!.disable({ emitEvent: false });
            this.form.get('totalTax')!.disable({ emitEvent: false });
            this.form.get('totalWithTax')!.disable({ emitEvent: false });

            this.form.get('supplierId')!.valueChanges.pipe(
                  startWith(this.form.get('supplierId')!.value),
                  switchMap(id => id ? this.bankAccountApi.listBySupplier(id) : of([])),
                  takeUntil(this.destroy$)
            ).subscribe(accounts => {
                  this.supplierBankAccounts = accounts ?? [];
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

      async onSupplierSelected(opt: KitDropdownOption) {
            this.form.patchValue({ supplierId: opt.id });
            const supplierBeneficiary = await firstValueFrom(this.supplierApi.getBeneficiaryById(opt.id));
            console.log('supplier beneficiary: ', supplierBeneficiary);
            if (supplierBeneficiary) {
                  this.form.patchValue({
                        bankName: supplierBeneficiary.beneficiaryBankName ?? '',
                        accountNumber: supplierBeneficiary.beneficiaryAccountNumber ?? '',
                        beneficiaryName: supplierBeneficiary.beneficiaryName ?? '',
                  });
            }
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

      labelOf = (opts: KitDropdownOption[], id: string | null): string => {
            if (!id) return '';
            return opts.find(o => o.id === id)?.label ?? '';
      }

      payeeOptions: KitDropdownOption[] = [
            { id: 'supplier', label: 'Nhà cung cấp' },
            { id: 'employee', label: 'Nhân viên' },
      ];
      selectedPayee: PayeeType | null = null;
      onPayeeSelected(opt: KitDropdownOption) {
            this.selectedPayee = opt.id === 'supplier' ? PayeeType.supplier : PayeeType.employee;
            if (this.selectedPayee === PayeeType.employee) {
                  this.form.patchValue({ supplierId: null, bankName: '', accountNumber: '', beneficiaryName: '' }, { emitEvent: false });
                  this.supplierBankAccounts = [];
                  this.selectedBankAccount = null;
            }
      }

      goodsReceiptOptions: KitDropdownOption[] = [
            { id: 'true', label: 'Có nhập kho' },
            { id: 'false', label: 'Không nhập kho' },
      ]
      onGoodsReceiptSelected(opt: KitDropdownOption) {
            if (opt.id === 'true') {
                  this.form.patchValue({ hasGoodsReceipt: true });
            } else {
                  this.form.patchValue({ hasGoodsReceipt: false });
            }
      }

      openCreateSupplierDialog(): void {
            const dialogRef = this.dialog.open(SupplierRequestDialogComponent);

            dialogRef.afterClosed().subscribe((createdSupplierId) => {
                  if (createdSupplierId?.id) {
                        this.selectedPayee = PayeeType.supplier;
                        this.form.patchValue({ supplierId: createdSupplierId });
                  }
            })
      }
      openAvailableBudgetPlanDetails(rowIndex: number): void {
            const row = this.items.at(rowIndex);
            const selectedId = row.get('budgetPlanDetailId')?.value;

            const dialogRef = this.dialog.open(AvailableBudgetPlansDialogComponent, {
                  data: selectedId,
            });

            dialogRef.afterClosed().subscribe((result: { detailId?: string, amount?: number } | undefined) => {
                  if (!result?.detailId || !result.amount) return;

                  const amount = Number(result.amount);
                  const totalWithTax = Number(row.get('totalWithTax')?.value ?? 0);

                  // Cho phép vượt 5%
                  const tolerance = 0.05; // 5%
                  const allowed = Math.round(amount * (1 + tolerance));

                  if (totalWithTax > allowed) {
                        this.confirm.validateBudgetLimit$({
                              message: `Khoản thanh toán vượt quá ngân sách cho phép`
                        }).subscribe(ok => {
                              if (!ok) return;
                        });
                        return;
                  }

                  row.patchValue({ budgetPlanDetailId: result.detailId }, { emitEvent: true });
                  this.toast.successRich('Đã chọn mã ngân sách');
            });
      }

      newItemGroup(): FormGroup<PaymentItem> {
            const group = this.formBuilder.group<PaymentItem>({
                  itemName: this.formBuilder.nonNullable.control<string>('', [Validators.required, Validators.maxLength(256)]),
                  uploadedInvoiceFile: this.formBuilder.control<File | null>(null),
                  uploadedInvoicePreviewUrl: this.formBuilder.control<string | null>(null),
                  invoiceStoredFileId: this.formBuilder.control<string | null>(null),
                  quantity: this.formBuilder.control<number | null>(null, { validators: [Validators.required, Validators.pattern('^[0-9]+$'), Validators.min(1)] }),
                  unitPrice: this.formBuilder.control<number | null>(null, { validators: [Validators.required, Validators.min(0)] }),
                  taxRate: this.formBuilder.nonNullable.control<number>(0), // mặc định 10%
                  taxRatePercent: this.formBuilder.nonNullable.control<string>('0'),
                  amount: this.formBuilder.nonNullable.control<number>(0),
                  taxAmount: this.formBuilder.nonNullable.control<number>(0),
                  totalWithTax: this.formBuilder.nonNullable.control<number>(0),
                  budgetPlanDetailId: this.formBuilder.control<string | null>(null),
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
            { originX: 'end', originY: 'top', overlayX: 'end', overlayY: 'bottom', offsetY: -8 },
      ]
      toggleInvoiceMenu(i: number, ev: MouseEvent) {
            ev.stopPropagation();
            this.invoiceMenuOpenIndex = (this.invoiceMenuOpenIndex === i) ? null : i;
      }
      onMenuClosed() {
            this.invoiceMenuOpenIndex = null;
      }

      async Submit(): Promise<void> {
            // chặn double-click & chặn khi vẫn còn upload file
            if (this.isBusy) return;

            if (this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich('Vui lòng nhập đầy đủ thông tin');

                  const invalidControls = Object.entries(this.form.controls)
                        .filter(([_, control]) => control.invalid)
                        .map(([name, control]) => ({
                              field: name,
                              errors: control.errors
                        }));

                  console.group('⚠️ Form invalid');
                  console.table(invalidControls);
                  console.groupEnd();

                  // Scroll đến control đầu tiên bị lỗi
                  const firstInvalidControl = document.querySelector('.ng-invalid[formControlName]') as HTMLElement;
                  if (firstInvalidControl) {
                        firstInvalidControl.scrollIntoView({ behavior: 'smooth', block: 'center' });
                        firstInvalidControl.focus();
                  }

                  return;
            }


            this.submitting = true;
            try {
                  const raw = this.form.getRawValue();

                  const uploadedInvoiceResults: (string | undefined)[] = [];

                  for (const [i, item] of raw.items.entries()) {
                        if (item.uploadedInvoiceFile) {
                              const file = item.uploadedInvoiceFile;

                              const result = await firstValueFrom(this.fileService.uploadFile(file, 'expense', 'invoice', undefined, false));

                              uploadedInvoiceResults[i] = result.data?.id; // StoredFileId
                        }
                  }

                  const items: ExpensePaymentItemPayload[] = (raw.items ?? []).map((it, i) => ({
                        itemName: it.itemName,
                        budgetPlanDetailId: it.budgetPlanDetailId!,
                        invoiceStoredFileId: uploadedInvoiceResults[i] ?? undefined,
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

                  const payload: ExpensePaymentPayload = {
                        name: raw.name,
                        payeeType: this.selectedPayee ?? PayeeType.supplier,
                        supplierId: this.selectedPayee === PayeeType.supplier ? raw.supplierId ?? undefined : undefined,
                        bankName: raw.bankName,
                        accountNumber: raw.accountNumber,
                        description: raw.description ?? '',
                        beneficiaryName: raw.beneficiaryName,
                        dueDate: raw.dueDate,
                        hasGoodsReceipt: raw.hasGoodsReceipt ?? false,
                        items,
                        // attachments,
                        followerIds: raw.followerIds,
                        managerApproverId: raw.managerApproverId,
                  }


                  console.log('payload: ', payload);
                  const result = await firstValueFrom(this.expensePaymentService.create(payload));
                  // this.router.navigate(
                  //       ['/expense/expense-payment-shell/following-payments'],
                  //       { queryParams: { paymentId: result } }
                  // );
                  this.toast.successRich('Gửi phê duyệt thành công');
            } catch (error) {
                  this.httpErrorHandler.handle(error, "Gửi phê duyệt thất bại");
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

      // === PREVIEW INVOICE ===
      onInvoiceFileSelected(event: Event, rowIndex: number) {
            console.log('run invocie file selected');
            const input = event.target as HTMLInputElement;
            if (!input.files || input.files.length === 0) return;

            const file = input.files[0];

            // Kiểm tra loại file
            const allowedTypes = [
                  'image/png', 'image/jpeg', 'image/jpg',
                  'application/pdf',
                  'application/msword',
                  'application/vnd.openxmlformats-officedocument.wordprocessingml.document'
            ];

            if (!allowedTypes.includes(file.type)) {
                  this.toast.errorRich('File không hợp lệ. Chỉ chấp nhận hình ảnh, PDF, Word.');
                  return;
            }

            const row = this.items.at(rowIndex);

            row.patchValue({
                  uploadedInvoiceFile: file,
                  uploadedInvoicePreviewUrl: file.type.startsWith('image/')
                        ? URL.createObjectURL(file)
                        : null
            });
      }

      previewInvoice(rowIndex: number) {
            const row = this.items.at(rowIndex);

            const file = row.get('uploadedInvoiceFile')?.value;
            const previewUrl = row.get('uploadedInvoicePreviewUrl')?.value;

            if (!file) {
                  this.toast.warningRich('Chưa có hóa đơn nào được tải lên');
                  return;
            }

            // file ảnh → dùng previewUrl
            if (file.type.startsWith('image/') && previewUrl) {
                  this.dialog.open(FileImagePreviewDialog, {
                        data: { src: previewUrl }
                  });
                  return;
            }

            // file PDF → mở dialog PDF viewer
            if (file.type === 'application/pdf') {
                  const pdfUrl = URL.createObjectURL(file);
                  this.dialog.open(FilePdfPreviewDialog, {
                        data: { src: pdfUrl }
                  });
                  return;
            }

            // file Word, docx → tải về (trình duyệt không preview)
            this.toast.info('File Word không thể preview, hệ thống sẽ tự tải xuống');
            const url = URL.createObjectURL(file);
            const a = document.createElement('a');
            a.href = url;
            a.download = file.name;
            a.click();
      }

      canPreviewInvoice(index: number): boolean {
            const row = this.items.at(index);
            const file = row.get('uploadedInvoiceFile')?.value;

            if (!file) return false;

            const type = file.type ?? '';
            return type.startsWith('image/') || type === 'application/pdf';
      }

      canUnlinkInvoice(index: number): boolean {
            const row = this.items.at(index);
            const file = row.get('uploadedInvoiceFile')?.value;
            return !!file; // enable nếu đã có file
      }
      unlinkInvoice(i: number) {
            const row = this.items.at(i);
            const file = row.get('uploadedInvoiceFile')?.value;

            if (!file) {
                  this.toast.infoRich('Dòng này chưa có hóa đơn nào để gỡ');
                  return;
            }

            // Clear file
            row.patchValue({
                  uploadedInvoiceFile: null,
                  uploadedInvoicePreviewUrl: null
            });

            this.toast.successRich('Đã gỡ hóa đơn đã liên kết');

            // Đóng menu nếu đang mở
            this.onMenuClosed();
      }

      onUploadInvoiceClicked(rowIndex: number, fileInput: HTMLInputElement) {
            const row = this.items.at(rowIndex);
            const alreadyHasInvoice = !!row.get('uploadedInvoiceFile')?.value;

            // Nếu chưa có file → mở bình thường
            if (!alreadyHasInvoice) {
                  fileInput.click();
                  return;
            }

            // Nếu đã có → hỏi confirm thay thế
            this.confirm.warn$(
                  'Dòng này đã có hoá đơn. Bạn có muốn tải lên hoá đơn thay thế?',
                  'Thay thế hoá đơn',
                  'Thay thế',
                  'Giữ nguyên'
            ).subscribe(ok => {
                  if (ok) {
                        fileInput.click();
                  }
            });
      }

      private minDateValidator(min: Date = new Date()) {
            min.setHours(0, 0, 0, 0);
            return (control: AbstractControl) => {
                  const value = control.value;
                  if (!value) return null;
                  return value < min ? { minDate: true } : null;
            };
      }

      // === DESTROY ===
      ngOnDestroy(): void {
            this.destroy$.next();
            this.destroy$.complete();
      }
}