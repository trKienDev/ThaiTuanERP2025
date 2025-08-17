import { CommonModule } from "@angular/common";
import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, FormsModule, NonNullableFormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { PartnerBankAccountDto, UpsertPartnerBankAccountRequest } from "../../models/partner-bank-account.model";
import { catchError, EMPTY, Subject, switchMap, takeUntil, tap } from "rxjs";
import { PartnerBankAccountService } from "../../services/partner-bank-account.service";
import { ActivatedRoute } from "@angular/router";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";

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
      styleUrl: './partner-bank-account.component.scss',
})
export class PartnerBankAccountComponent implements OnInit, OnDestroy {
      supplierId!: string;
      form!: BankAccountForm;
      isLoading = false;
      errorMessages: string[] = [];
      bankAccount: PartnerBankAccountDto | null = null;

      private destroy$ = new Subject<void>();

      constructor(
            private formBuilder: NonNullableFormBuilder,
            private route: ActivatedRoute,
            private partnerBankAccountService: PartnerBankAccountService,
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

            this.route.paramMap.pipe(
                  takeUntil(this.destroy$),
                  tap(pm => (this.supplierId = pm.get('supplierId')!)),
                  switchMap(() => {
                        if(!this.supplierId) return EMPTY;
                        this.isLoading = true;
                        return this.partnerBankAccountService.get(this.supplierId).pipe(
                              tap(dto => {
                                    this.bankAccount = dto;
                                    this.form.patchValue({
                                          accountNumber: dto.accountNumber,
                                          bankName: dto.bankName,
                                          accountHolder: dto.accountHolder,
                                          swiftCode: dto.swiftCode,
                                          note: dto.note,
                                          isActive: dto.isActive
                                    });
                                    this.isLoading = false;
                              }),
                              catchError(err => {
                                    this.isLoading = false;
                                    this.errorMessages = handleHttpError(err);
                                    return EMPTY;
                              })
                        );
                  })
            ).subscribe();
      }

      save(): void {
            if(this.form.invalid || !this.supplierId) {
                  this.form.markAllAsTouched();
                  return;
            }

            this.isLoading = true;const raw = this.form.getRawValue(); // trả đúng shape, không Partial
  const payload: UpsertPartnerBankAccountRequest = {
    accountNumber: raw.accountNumber,
    bankName: raw.bankName,
    isActive: raw.isActive,
    ...(raw.accountHolder ? { accountHolder: raw.accountHolder } : {}),
    ...(raw.swiftCode     ? { swiftCode: raw.swiftCode }         : {}),
    ...(raw.branch        ? { branch: raw.branch }               : {}),
    ...(raw.note          ? { note: raw.note }                   : {})
  };

            this.partnerBankAccountService.upsert(this.supplierId, payload).pipe(
                  tap(dto => {
                        this.bankAccount = dto;
                        this.isLoading = false;
                  }),
                  catchError(err => {
                        this.isLoading = false;
                        this.errorMessages = handleHttpError(err);
                        return EMPTY;
                  })
            ).subscribe();
      }

      remove(): void { 
            if(!this.supplierId) return;
            this.isLoading = true;
            this.partnerBankAccountService.delete(this.supplierId).pipe(
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
                  }), 
                  catchError(err => {
                        this.isLoading = false;
                        this.errorMessages = handleHttpError(err);
                        return EMPTY;
                  })
            ).subscribe();
      }

      ngOnDestroy(): void {
            this.destroy$.next();
            this.destroy$.complete();
      }
}