import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild, Directive, HostListener, Self, Optional } from '@angular/core';
import { FormsModule, NgControl } from '@angular/forms';
import {
  FormControl, FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { EMPTY, catchError, debounceTime, distinctUntilChanged } from 'rxjs';

import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatSidenavModule, MatDrawer } from '@angular/material/sidenav';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSortModule, Sort } from '@angular/material/sort';

import { SupplierService } from '../../services/supplier.service';
import { SupplierDto, CreateSupplierRequest, UpdateSupplierRequest } from '../../models/supplier.model';
import { handleHttpError } from '../../../../core/utils/handle-http-errors.util';
import { PartnerBankAccountComponent } from '../partner-bank-account/partner-bank-account.component';

type SupplierForm = FormGroup<{
  code: FormControl<string>;
  name: FormControl<string>;
  defaultCurrency: FormControl<string>;
  paymentTermDays: FormControl<number>;
  shortName: FormControl<string | null>;
  email: FormControl<string | null>;
  phone: FormControl<string | null>;
  isActive: FormControl<boolean>;
}>;

/** Standalone directive: tự upper-case input */
@Directive({
  selector: '[appUppercase]',
  standalone: true,
})
export class UppercaseDirective {
  constructor(@Self() @Optional() private ngControl: NgControl) {}
  @HostListener('input') onInput() {
    const ctrl = this.ngControl?.control as FormControl<string> | undefined;
    if (!ctrl) return;
    const v = (ctrl.value ?? '').toString().toUpperCase();
    ctrl.setValue(v, { emitEvent: false });
  }
}

@Component({
  selector: 'expense-supplier',
  standalone: true,
  imports: [
    CommonModule, FormsModule, ReactiveFormsModule, RouterModule,
    MatTableModule, MatPaginatorModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatButtonModule, MatIconModule, MatChipsModule,
    MatDividerModule, MatSidenavModule, MatProgressSpinnerModule,
    MatCheckboxModule, MatSortModule,UppercaseDirective,
    PartnerBankAccountComponent,
  ],
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.scss'],
})
export class SupplierComponent implements OnInit {
  // data
  suppliers: SupplierDto[] = [];
  total = 0;
  page = 1;
  pageSize = 10;

  // filters
  filters = {
    keyword: '',
    isActive: '' as '' | 'true' | 'false',
    currency: ''
  };
  keywordCtrl = new FormControl<string>('');

  // sort (chuẩn bị cho server-side sort)
  sortField: string | null = null;
  sortDir: 'asc' | 'desc' | '' = '';

  // ui state
  displayedColumns: string[] = ['code', 'name', 'currency', 'term', 'status', 'actions'];
  errorMessages: string[] = [];
  loading = false;
  editingId: string | null = null;

  // form
  form!: SupplierForm;

  // hybrid drawer state
  drawerMode: 'supplier' | 'bank' = 'supplier';
  selectedSupplierId: string | null = null;

  @ViewChild('drawer', { static: true }) drawer!: MatDrawer;

  constructor(
    private supplierService: SupplierService,
    private fb: NonNullableFormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.buildForm();
    // debounce search
    this.keywordCtrl.valueChanges
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe(v => {
        this.filters.keyword = v || '';
        this.load(1);
      });

    this.load(1);

    this.route.queryParamMap.subscribe(q => {
      const panel = q.get('panel');
      const supplierId = q.get('supplierId');
      if(panel === 'bank' && supplierId) {
            this.drawerMode = 'bank';
            this.selectedSupplierId = supplierId;
            this.drawer.open();
      }
    });
  }

  private buildForm() {
    this.form = this.fb.group({
      code: this.fb.control('', {
    // ⬇️ bỏ Validators.required
    validators: [Validators.maxLength(50), Validators.pattern(/^[A-Z0-9_\-\.]+$/)]
  }),
      name: this.fb.control('', { validators: [Validators.required, Validators.maxLength(200)] }),
      defaultCurrency: this.fb.control('VND', { validators: [Validators.required, Validators.pattern(/^[A-Z]{3}$/)] }),
      paymentTermDays: this.fb.control(30, { validators: [Validators.min(0), Validators.max(365)] }),
      shortName: this.fb.control<string | null>(null, { validators: [Validators.maxLength(50)] }),
      email: this.fb.control<string | null>(null),
      phone: this.fb.control<string | null>(null),
      isActive: this.fb.control(true),
    });
  }

  load(page = this.page): void {
    this.loading = true;
    this.page = page;

    const isActive =
      this.filters.isActive === '' ? undefined : this.filters.isActive === 'true';

    this.supplierService.getAll({
      keyword: this.filters.keyword || undefined,
      isActive,
      currency: this.filters.currency || undefined,
      page: this.page,
      pageSize: this.pageSize
      // Khi backend hỗ trợ sort, thêm:
      // , sortField: this.sortField || undefined
      // , sortDir: this.sortDir || undefined
    })
    .pipe(
      catchError(err => {
        this.errorMessages = handleHttpError(err);
        this.loading = false;
        return EMPTY;
      })
    )
    .subscribe(res => {
      this.suppliers = res.items;
      this.total = res.totalCount; // đảm bảo dùng 'total'
      this.loading = false;
    });
  }

  onSortChange(e: Sort) {
    this.sortField = e.active || null;
    this.sortDir = (e.direction as 'asc' | 'desc' | '') || '';
    this.load(1);
  }

  onPageChange(e: PageEvent) {
    this.page = e.pageIndex + 1;
    this.pageSize = e.pageSize;
    this.load(this.page);
  }

  resetFilters() {
    this.filters = { keyword: '', isActive: '', currency: '' };
    this.keywordCtrl.setValue('', { emitEvent: false });
    this.load(1);
  }

  openCreate() {
    this.editingId = null;
    this.form.reset({
      code: '',
      name: '',
      defaultCurrency: 'VND',
      paymentTermDays: 30,
      shortName: null,
      email: null,
      phone: null,
      isActive: true
    });
    this.form.controls.code.enable();
    this.drawer.open();

    // dọn query param
    this.router.navigate([], { queryParams: { panel: null, supplierId: null }, queryParamsHandling: 'merge' });
  }

  openEdit(item: SupplierDto) {
    this.editingId = item.id;
    this.form.reset({
      code: item.code,
      name: item.name,
      defaultCurrency: item.defaultCurrency,
      paymentTermDays: item.paymentTermDays,
      shortName: item.shortName ?? null,
      email: item.email ?? null,
      phone: item.phone ?? null,
      isActive: item.isActive
    });
    this.form.controls.code.disable();
    this.drawer.open();
    this.router.navigate([], { queryParams: { panel: null, supplierId: null }, queryParamsHandling: 'merge' });
  }

  openBank(item: SupplierDto) {
      this.drawerMode = 'bank';
      this.selectedSupplierId = item.id;
      this.drawer.open();

      this.router.navigate([], {
            queryParams: { panel: 'bank', supplierId: item.id },
      queryParamsHandling: 'merge'
      });
  }

  onDrawerClosed() {
      this.drawerMode = 'supplier';
      this.selectedSupplierId = null;
      // xóa panel/supplierId khỏi URL
      this.router.navigate([], {
            queryParams: { panel: null, supplierId: null },
            queryParamsHandling: 'merge'
      });
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();

    if (!this.editingId) {
      const payload: CreateSupplierRequest = {
        code: raw.code ? raw.code.toUpperCase() : undefined,
        name: raw.name,
        defaultCurrency: raw.defaultCurrency.toUpperCase(),
        paymentTermDays: raw.paymentTermDays,
        shortName: raw.shortName || undefined,
        email: raw.email || undefined,
        phone: raw.phone || undefined,
        taxCode: undefined,
        withholdingTaxType: undefined,
        withholdingTaxRate: undefined,
        postingProfileId: undefined,
        supplierGroupId: undefined,
        addressLine1: undefined,
        addressLine2: undefined,
        city: undefined,
        stateOrProvince: undefined,
        postalCode: undefined,
        country: undefined,
        note: undefined
      };

      this.loading = true;
      this.supplierService.create(payload)
        .pipe(catchError(err => { this.errorMessages = handleHttpError(err); this.loading = false; return EMPTY; }))
        .subscribe(() => { this.drawer.close(); this.load(1); });
    } else {
      const payload: UpdateSupplierRequest = {
        name: raw.name,
        defaultCurrency: raw.defaultCurrency.toUpperCase(),
        paymentTermDays: raw.paymentTermDays,
        shortName: raw.shortName || undefined,
        email: raw.email || undefined,
        phone: raw.phone || undefined,
        taxCode: undefined,
        withholdingTaxType: undefined,
        withholdingTaxRate: undefined,
        postingProfileId: undefined,
        supplierGroupId: undefined,
        addressLine1: undefined,
        addressLine2: undefined,
        city: undefined,
        stateOrProvince: undefined,
        postalCode: undefined,
        country: undefined,
        isActive: raw.isActive,
        note: undefined
      };

      this.loading = true;
      this.supplierService.update(this.editingId, payload)
        .pipe(catchError(err => { this.errorMessages = handleHttpError(err); this.loading = false; return EMPTY; }))
        .subscribe(() => { this.drawer.close(); this.load(this.page); });
    }
  }

  toggle(item: SupplierDto) {
    this.supplierService.toggleStatus(item.id, !item.isActive)
      .pipe(catchError(err => { this.errorMessages = handleHttpError(err); return EMPTY; }))
      .subscribe(updated => { item.isActive = updated.isActive; });
  }

  trackById = (_: number, x: SupplierDto) => x.id;
}
