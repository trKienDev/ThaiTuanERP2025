import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { TaxService } from "../../services/tax.service";
import { HttpClient } from "@angular/common/http";
import { CreateTaxRequest, TaxDto, UpdateTaxRequest } from "../../models/tax.dto";

interface LedgerAccountOption {
      id: string; 
      code: string;
      name: string;
}

@Component({
      selector: 'finance-tax',
      standalone: true,
      imports: [ CommonModule, FormsModule, ReactiveFormsModule ],
      templateUrl: './finance-tax.component.html',
      styleUrl: './finance-tax.component.scss'
})
export class FinanceTaxComponent {
      private formBuilder = inject(FormBuilder);
      private taxService = inject(TaxService);
      private http = inject(HttpClient);

      loading = false;
      saving = false;

      data: TaxDto[] = [];
      view: TaxDto[] = [];
      selected = new Set<string>();

      ledgerAccountOptions: LedgerAccountOption[] = [];

      // lọc & sort
      filters = { q: '', status: 'all' as 'all' | 'active' | 'inactive' };
      sort = { field: 'policyName' as keyof TaxDto, dir: 'asc' as 'asc' | 'desc' };

      // drawer + form
      drawer = { open: false, mode: 'create' as 'create' | 'edit' };
      form = this.formBuilder.nonNullable.group({
            id: [''],
            policyName: ['', [Validators.required, Validators.maxLength(50)]],
            // UI nhập theo % → bind vào ratePercent, khi save sẽ chuyển về 0..1
            ratePercent: [10, [Validators.required, Validators.min(0), Validators.max(100)]],
            postingLedgerAccountId: ['', Validators.required],
            isActive: [true],
            description: ['' as string | null],
      });
      get fc() { return this.form.controls; }

      // preview thuế cơ bản (exclusive)
      preview = { base: 10000, tax: 0, total: 0 };

      ngOnInit() {
            this.reload();
            this.loadLedgerAccountOptions();
      }

      // ===== UI actions =====
      toggleSelet(id: string, ev: Event) {
            const checked = (ev.target as HTMLInputElement).checked;
            checked ? this.selected.add(id) : this.selected.delete(id);
      }
      allChecked() {
            return this.view.length > 0 && this.view.every(x => this.selected.has(x.id));
      }
      toggleAll(ev: Event) {
            const checked = (ev.target as HTMLInputElement).checked;
            checked ? this.view.forEach(x => this.selected.add(x.id)) : this.view.forEach(x => this.selected.delete(x.id));
      }
      
      hasFilter() {
            return !!this.filters.q || this.filters.status !== 'all';
      }
      clearFilters() {
            this.filters = { q: '', status: 'all' };
            this.applyFilter();
      }
      sortBy(field: keyof TaxDto) {
            if(this.sort.field === field) 
                  this.sort.dir = this.sort.dir === 'asc' ? 'desc' : 'asc';
            else { 
                  this.sort.field = field;
                  this.sort.dir = 'asc'
            }
      }

      applyFilter() {
            const q = this.filters.q.trim().toLowerCase();
            let rows = [...this.data];

            if(q) rows = rows.filter(x => 
                  (x.policyName || '').toLowerCase().includes(q) ||
                  (x.postingLedgerAccountCode || '').toLowerCase().includes(q) || 
                  (x.postingLedgerAccountName || '').toLowerCase().includes(q)
            );
            if(this.filters.status !== 'all') rows = rows.filter(x => this.filters.status === 'active' ? x.isActive : !x.isActive);

            rows.sort((a: any, b: any) => {
                  const va = (a[this.sort.field] ?? '').toString().toLowerCase();
                  const vb = (b[this.sort.field] ?? '').toString().toLowerCase();
                  if(va < vb) return this.sort.dir === 'asc' ? -1 : 1;
                  if(va > vb) return this.sort.dir === 'asc' ? 1 : -1;
                  return 0;
            });

            this.view = rows;
      }

      openCreate() {
            this.drawer = { open: true, mode: 'create' };
            this.form.reset({
                  id: '',
                  policyName: '',
                  ratePercent: 10,
                  postingLedgerAccountId: '',
                  isActive: true,
                  description: '',
            });
            setTimeout(() => document.querySelector<HTMLInputElement>('input[formControlName="policyName"]')?.focus());
            this.recalcPreview();
      }

      openEdit(row: TaxDto) {
            this.drawer = { open: true, mode: 'edit' };
            this.form.reset({
                  id: row.id,
                  policyName: row.policyName,
                  ratePercent: row.rate * 100, // chuyển 0..1 → %
                  postingLedgerAccountId: row.postingLedgerAccountId,
                  isActive: row.isActive,
                  description: row.description ?? '',
            });
            this.recalcPreview();
      }

      closeDrawer() { this.drawer.open = false; }

      save() {
            if (this.form.invalid) { this.form.markAllAsTouched(); return; }
            this.saving = true;

            const v = this.form.getRawValue();
            const payloadBase = {
                  id: v.id,
                  policyName: v.policyName.trim(),
                  rate: (v.ratePercent ?? 0) / 100, // % → 0..1
                  postingLedgerAccountId: v.postingLedgerAccountId,
                  isActive: !!v.isActive,
                  description: (v.description || '').trim() || null,
            };

            const req = this.drawer.mode === 'create'
                  ? this.taxService.create(payloadBase as CreateTaxRequest)
                  : this.taxService.update(payloadBase.id!, payloadBase as UpdateTaxRequest);

            req.subscribe({
                  next: () => {
                        this.saving = false;
                        this.closeDrawer();
                        this.reload();
                  },
                  error: () => (this.saving = false),
            });
      }

      toggleStatus(row: TaxDto, ev: Event) {
            const isActive = (ev.target as HTMLInputElement).checked;
            this.taxService.toggleActive(row.id, isActive).subscribe({
                  next: () => (row.isActive = isActive),
                  error: () => ((ev.target as HTMLInputElement).checked = !isActive),
            });
      }

      export() {
            const blob = new Blob([JSON.stringify(this.view, null, 2)], { type: 'application/json' });
            const url = URL.createObjectURL(blob);
            const a = document.createElement('a'); a.href = url; a.download = 'taxes.json'; a.click();
            URL.revokeObjectURL(url);
      }

      recalcPreview() {
            const ratePct = this.form.value.ratePercent ?? 0;
            const base = +this.preview.base || 0;
            const tax = base * (ratePct / 100);
            this.preview.tax = Math.round(tax);
            this.preview.total = Math.round(base + tax);
      }

      reload() {
            this.loading = true;
            this.taxService.getAll().subscribe({
                  next: (res: any) => {
                        // nếu API bọc ApiResponse<T>, lấy res.data; nếu trả thẳng mảng, lấy res
                        const rows = (res && 'data' in res) ? (res.data as TaxDto[]) : (res as TaxDto[]);
                        this.data = rows ?? [];
                        this.applyFilter();
                        this.selected.clear();
                        this.loading = false;
                  },
                  error: () => (this.loading = false),
            });
      }

      private loadLedgerAccountOptions() {
            // TODO: đổi endpoint cho đúng API của bạn (ví dụ: /api/ledger-accounts/brief)
            this.http.get<LedgerAccountOption[]>('/api/ledger-accounts/brief').subscribe({
                  next: (rows) => this.ledgerAccountOptions = rows ?? [],
                  error: () => this.ledgerAccountOptions = [],
            });
      }
}