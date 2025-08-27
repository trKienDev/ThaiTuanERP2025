import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { AbstractControl, AsyncValidatorFn, FormBuilder, ReactiveFormsModule, Validators, FormsModule, FormControl } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { TaxService } from "../../../services/tax.service";
import { catchError, debounceTime, distinctUntilChanged, finalize, first, map, Observable, of, startWith, switchMap } from "rxjs";
import { CreateTaxRequest } from "../../../models/tax.model";
import { handleHttpError } from "../../../../../core/utils/handle-http-errors.util";
import { LedgerAccountService } from "../../../services/ledger-account.service";
import { LedgerAccountDto, LedgerAccountLookupDto } from "../../../models/ledger-account.model";
import { MatSelectModule } from "@angular/material/select";
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from "@angular/material/autocomplete";

@Component({
      selector: 'tax-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MatDialogModule,
            MatFormFieldModule, MatInputModule, MatCheckboxModule, MatButtonModule, MatSelectModule, MatAutocompleteModule, FormsModule, MatInputModule
      ],
      templateUrl: './tax-request-dialog.component.html',
      styleUrl: './tax-request-dialog.component.scss',
})
export class TaxRequestDialogComponent implements OnInit {
      private formBuilder = inject(FormBuilder);
      private dialogRef = inject(MatDialogRef<TaxRequestDialogComponent>);
      private taxService = inject(TaxService);
      private ledgerAccountService = inject(LedgerAccountService);

      saving = false;
      errorMessages: string[] = [];

      ledgerAccounts: LedgerAccountDto[] = [];
      filteredLedgerAccountts: LedgerAccountDto[] = [];
      ledgerAccountLoading = false;

      // Ô tìm kiếm cho autocomplete
      ledgerAccountSearchCtrl = new FormControl<string>('');
      ledgerAccountOptions: LedgerAccountLookupDto[] = [];
      ledgerAccountSearching = false;

      ngOnInit(): void {
            this.loadLedgerAccounts();
            this.bindingLedgerLookup();
      }

      // Async validator kiểm tra trùng policyName (optional nhưng nên có)
      private policyNameAvailableValidator: AsyncValidatorFn = (control: AbstractControl): Observable<any> => {
            const value = (control.value ?? '').trim();
            if(!value) return of(null);
            return of(value).pipe(
                  debounceTime(300),
                  distinctUntilChanged(),
                  switchMap(name => 
                        this.taxService.checkAvailable(name).pipe(
                              map(isAvailable => (isAvailable ? null : { nameTaken: true })),
                              catchError(() => of(null))  // không chặn form nếu API check lỗi
                        )
                  ),
                  first()
            );
      };

      private loadLedgerAccounts() {
            this.ledgerAccountLoading = true;
            this.ledgerAccountService.getAll().subscribe({
                  next: (list) => { 
                        this.ledgerAccounts = list; 
                        // seed options khi người dùng CHƯA gõ gì
                        this.ledgerAccountOptions = (list ?? [])
                              .slice(0, 20) // lấy trước 20 dòng cho gọn
                              .map(x => ({ id: x.id, number: x.number, name: x.name })); // map sang LookupDto nếu cần
                  },
                  error: (err) => { this.errorMessages = handleHttpError(err); },
                  complete: () => { this.ledgerAccountLoading = false; }
            })
      }
      private bindingLedgerLookup() {
            this.ledgerAccountSearchCtrl.valueChanges.pipe(
                  startWith(''),
                  debounceTime(250),
                  distinctUntilChanged(),
                  switchMap(keyword => {
                        const kw = (keyword ?? '').trim();
                        // 👉 Nếu chưa gõ gì: dùng dữ liệu đã seed từ getAll()
                        if (!kw) {
                              return of(this.ledgerAccounts
                                    .slice(0, 20)
                                    .map(x => ({ id: x.id, number: x.number, name: x.name })));
                        }
                        this.ledgerAccountSearching = true;
                        return this.ledgerAccountService.lookup(kw).pipe(
                              finalize(() => this.ledgerAccountSearching = false),
                              catchError(() => of<LedgerAccountLookupDto[]>([]))
                        );
                  })
            ).subscribe(list => {
                  this.ledgerAccountOptions = list;
            });
      }
      // Khi chọn 1 item trong autocomplete
      onLedgerAccountSelected(e: MatAutocompleteSelectedEvent) {
            const item = e.option.value as LedgerAccountLookupDto;
            this.form.patchValue({ postingLedgerAccountId: item.id });

            // hiển thị lại text đã chọn trong ô tìm kiếm
            this.ledgerAccountSearchCtrl.setValue(`${item.number} — ${item.name}`, { emitEvent: false });
      }
      // helper hiển thị text trong input từ item đã chọn (nếu bạn muốn bind lại)
      displayLedgerAccount = (id?: string) => {
            const found = this.ledgerAccountOptions.find(x => x.id === id);
            return found ? `${found.number} — ${found.name}` : '';
      };

      form = this.formBuilder.group({
            policyName: ['', {
                  validators: [Validators.required, Validators.maxLength(200)],
                  asyncValidators: [ this.policyNameAvailableValidator ],
                  updateOn: 'blur' // check khi rời field
            }],
            rate: [0, [Validators.required, Validators.min(0), Validators.max(1)]],
            postingLedgerAccountId: ['', [Validators.required]],
            description: [''],
            isActive: [true]
      });

      get taxRequestForm() { return this.form.controls; }

      submit(): void {
            this.errorMessages = [];
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  return;
            }

            const payload = this.form.getRawValue() as CreateTaxRequest;

            this.saving = true;
            this.taxService.create(payload).pipe(
                  catchError(err => {
                        this.errorMessages = handleHttpError(err);
                        this.saving = false;
                        return of(null);
                  })
            ).subscribe((created) => {
                  if(!created) return;
                  this.dialogRef.close('created');
            });
      }

      cancel(): void {
            this.dialogRef.close();
      }
}