import { CommonModule } from "@angular/common";
import { Component, Input, OnDestroy, OnInit } from "@angular/core";
import { FormControl, FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { PartnerBankAccountDto, UpsertPartnerBankAccountRequest } from "../../models/partner-bank-account.model";
import { catchError, EMPTY, Subject, switchMap, takeUntil, tap } from "rxjs";
import { PartnerBankAccountService } from "../../services/partner-bank-account.service";
import { ActivatedRoute } from "@angular/router";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";
import { MatSnackBar } from "@angular/material/snack-bar";

type BankAccountForm = FormGroup<{
  accountNumber: FormControl<string>;
  bankName:      FormControl<string>;
  accountHolder: FormControl<string | null>;
  swiftCode:     FormControl<string | null>;
  branch:        FormControl<string | null>;
  note:          FormControl<string | null>;
  isActive:      FormControl<boolean>;
}>;

@Component({
  selector: 'expense-supplier-bank-account',
  standalone: true,
  imports: [ CommonModule, ReactiveFormsModule ],
  templateUrl: './partner-bank-account.component.html',
  styleUrls: ['./partner-bank-account.component.scss'], // <-- plural
})
export class PartnerBankAccountComponent implements OnInit, OnDestroy {
  @Input() set supplierId(value: string | undefined) {
    this._supplierId = value ?? null;
    if (this._supplierId) this.load(this._supplierId); // load khi nhận từ cha
  }
  get supplierId(): string | null { return this._supplierId; }
  private _supplierId: string | null = null;

  form!: BankAccountForm;
  isLoading = false;
  errorMessages: string[] = [];
  bankAccount: PartnerBankAccountDto | null = null;

  private destroy$ = new Subject<void>();

  successMessage = '';

  constructor(
    private formBuilder: NonNullableFormBuilder,
    private route: ActivatedRoute,
    private partnerBankAccountService: PartnerBankAccountService,
    private snack: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      accountNumber: this.formBuilder.control('', { validators: [Validators.required, Validators.maxLength(50)] }),
      bankName:      this.formBuilder.control('', { validators: [Validators.required, Validators.maxLength(150)] }),
      accountHolder: this.formBuilder.control<string | null>(null, { validators: [Validators.maxLength(150)] }),
      swiftCode:     this.formBuilder.control<string | null>(null, { validators: [Validators.pattern(/^[A-Z0-9]{8}([A-Z0-9]{3})?$/)] }),
      branch:        this.formBuilder.control<string | null>(null, { validators: [Validators.maxLength(150)] }),
      note:          this.formBuilder.control<string | null>(null, { validators: [Validators.maxLength(500)] }),
      isActive:      this.formBuilder.control(true),
    });

    // Fallback: nếu không truyền @Input thì đọc từ route (full-page)
    this.route.paramMap.pipe(
      takeUntil(this.destroy$),
      tap(pm => {
        if (!this._supplierId) {
          const id = pm.get('supplierId');
          if (id) { this._supplierId = id; this.load(id); }
        }
      }),
      switchMap(() => EMPTY)
    ).subscribe();
  }

  private load(id: string) {
    this.isLoading = true;
    this.partnerBankAccountService.get(id).pipe(
      tap(dto => {
      if (!dto) {
        // 404 → chưa có tài khoản → empty state + form tạo mới
        this.bankAccount = null;
        this.form.reset({
          accountNumber: '',
          bankName: '',
          accountHolder: null,
          swiftCode: null,
          note: null,
          isActive: true,
        });
        this.isLoading = false;
        return;
      }
      // Có dữ liệu → patch form
      this.bankAccount = dto;
      this.form.patchValue({
        accountNumber: dto.accountNumber,
        bankName: dto.bankName,
        accountHolder: dto.accountHolder ?? null,
        swiftCode: dto.swiftCode ?? null,
        note: dto.note ?? null,
        isActive: dto.isActive,
      });
      this.isLoading = false;
    }),
      catchError(err => {
        this.isLoading = false;
        this.errorMessages = handleHttpError(err);
        return EMPTY;
      })
    ).subscribe();
  }

  save(): void {
    if (this.form.invalid || !this._supplierId) {
      this.form.markAllAsTouched();
      return;
    }
    this.isLoading = true;
    const raw = this.form.getRawValue();
    const payload: UpsertPartnerBankAccountRequest = {
      accountNumber: raw.accountNumber,
      bankName: raw.bankName,
      isActive: raw.isActive,
      ...(raw.accountHolder ? { accountHolder: raw.accountHolder } : {}),
      ...(raw.swiftCode ? { swiftCode: raw.swiftCode } : {}),
      ...(raw.branch ? { branch: raw.branch } : {}),
      ...(raw.note ? { note: raw.note } : {}),
    };

    this.partnerBankAccountService.upsert(this._supplierId, payload).pipe(
      tap(dto => { 
        this.bankAccount = dto; 
        this.isLoading = false; 

        this.errorMessages = [];
        this.successMessage = 'Đã lưu tài khoản ngân hàng';
        this.snack.open('Đã lưu tài khoản ngân hàng', 'Đóng', { duration: 3000 });
        setTimeout(() => (this.successMessage = ''), 3000);

        this.form.markAsPristine();

      }),
      catchError(err => { 
        this.isLoading = false; 
        this.errorMessages = handleHttpError(err); 
        this.snack.open('Lưu thất bại', 'Đóng', { duration: 3000 });
        return EMPTY; })
    ).subscribe();
  }

  remove(): void {
    if (!this._supplierId) return;
     if (!confirm('Xóa tài khoản ngân hàng của nhà cung cấp này?')) return; // +++ xác nhận

    this.isLoading = true;
    this.partnerBankAccountService.delete(this._supplierId).pipe(
      tap(() => {
        this.bankAccount = null;
        this.form.reset({
          accountNumber: '',
          bankName: '',
          accountHolder: '',
          swiftCode: '',
          branch: '',
          note: '',
          isActive: true
        });
        this.isLoading = false;

        this.errorMessages = [];
        this.successMessage = 'Đã xóa tài khoản ngân hàng';
        this.snack.open('Đã xóa tài khoản ngân hàng', 'Đóng', { duration: 3000 });
        setTimeout(() => (this.successMessage = ''), 3000);
      }),
      catchError(err => { 
        this.isLoading = false; 
        this.errorMessages = handleHttpError(err); 
        this.snack.open('Xóa thất bại', 'Đóng', { duration: 3000 });
        return EMPTY; })
    ).subscribe();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
