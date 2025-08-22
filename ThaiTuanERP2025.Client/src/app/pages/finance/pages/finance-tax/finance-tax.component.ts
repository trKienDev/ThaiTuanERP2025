import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, inject, OnInit } from "@angular/core";
import { AbstractControl, AsyncValidatorFn, FormBuilder, FormControl, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { debounceTime, distinctUntilChanged, firstValueFrom, map, of, range, switchMap, timer } from "rxjs";
import { TaxService } from "../../services/tax.service";
import { CreateTaxRequest } from "../../models/tax.dto";
import { LedgerAccountService } from "../../services/ledger-account.service";
import { LedgerAccountLookupDto } from "../../models/ledger-account.model";

function rateBetweenZeroAndOne(ctrl: AbstractControl) {
      const value = ctrl.value;
      if(value === null || value === undefined || value === "") 
            return null; // let required handle empties
      const num = Number(value);
      if(Number.isNaN(num))
            return { number: true };
      return num >= 0 && num <= 1 ? null : { range: true }
}

function uniquePolicyNameValidator(taxService: TaxService, getExcludeId?: () => string | null): AsyncValidatorFn {
      return (ctrl: AbstractControl) => {
            const name = (ctrl.value || "").trim();
            if(!name) return of(null);
            // debounce a bit to avoid spamming API while typing
            return timer(300).pipe(
                  // NOTE: backend supports optional excludeId, useful when editing. For create, omit.
                  // Service method fixed below to hit /taxes/check-available
                  switchMap(() => taxService.checkAvailable(name, getExcludeId?.() || undefined)),
                  map((available) => (available ? null : { policyTaken: true }))
            );
      };
}

@Component({
      selector: 'finance-tax',
      standalone: true,
      imports: [ CommonModule, ReactiveFormsModule ],
      templateUrl: './finance-tax.component.html',
      styleUrl: './finance-tax.component.scss',
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class FinanceTaxComponent implements OnInit {
      private fb = inject(FormBuilder);
      private taxService = inject(TaxService);
      private ledgerAccountService = inject(LedgerAccountService);

      submitting = false;
      serverMessage: string | null = null;

      // control để gõ tìm kiếm
      plaQuery = new FormControl<string>('', { nonNullable: true });

      // Dữ liệu dropdown
      options: LedgerAccountLookupDto[] = [];
      showDropdown = false;

      form = this.fb.group({
            policyName: this.fb.nonNullable.control("", {
                  validators: [Validators.required, Validators.maxLength(128)],
                  asyncValidators: [uniquePolicyNameValidator(this.taxService)],
                  updateOn: "blur", // run async validator when field loses focus
            }),
            rate: this.fb.nonNullable.control(0, [Validators.required, rateBetweenZeroAndOne]), // 0..1
            postingLedgerAccountId: this.fb.nonNullable.control("", [Validators.required]),
            description: this.fb.control<string | null>(null, [Validators.maxLength(512)]),
            isActive: this.fb.nonNullable.control(true),
      });

      ngOnInit(): void {
            this.plaQuery.valueChanges.pipe(debounceTime(250), distinctUntilChanged(), switchMap(q => {
                  const keyword = (q || '').trim();
                  if(!keyword) {
                        this.options = [];
                        return of<LedgerAccountLookupDto[]>([]);
                  }
                  return this.ledgerAccountService.lookup(keyword);
            })).subscribe(list => {
                  this.options = list;
                  this.showDropdown = list.length > 0;
            })
      }

      
      // Chọn 1 option → set form control postingLedgerAccountId
      selectLedgerAccount(opt: LedgerAccountLookupDto) {
            this.form.controls.postingLedgerAccountId.setValue(opt.id);
            // Hiển thị lại nhãn tại ô search
            this.plaQuery.setValue(`${opt.number} - ${opt.name}`);
            this.showDropdown = false;
      }

      // xóa chọn
      clearSelectedLedger() {
            this.form.controls.postingLedgerAccountId.setValue('');
            this.plaQuery.setValue('');
            this.options = [];
            this.showDropdown = false;
      }


      async submit() {
            this.serverMessage = null;
            if (this.form.invalid) {
                  this.form.markAllAsTouched();
                  return;
            }

            this.submitting = true;
            try {
                  const raw = this.form.getRawValue();
                  const payload: CreateTaxRequest = {
                        policyName: raw.policyName,
                        rate: raw.rate,
                        postingLedgerAccountId: raw.postingLedgerAccountId,
                        description: raw.description ?? null,
                        isActive: raw.isActive,
                  };
                  const created = await firstValueFrom(this.taxService.create(payload));
                  this.serverMessage = `Đã tạo chính sách thuế “${created.policyName}” thành công.`;
                  this.form.reset({ isActive: true });
            } catch (err: any) {
                  // You may already have a global error handler; keep this minimal
                  this.serverMessage = err?.error?.message || "Không thể tạo thuế. Vui lòng thử lại.";
            } finally {
                  this.submitting = false;
            }
      }
}