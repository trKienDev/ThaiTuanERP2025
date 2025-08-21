import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges, inject } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { LedgerAccountKind, LedgerAccountTypeDto } from '../../../models/ledger-account-type.dto';
import { LedgerAccountTypeService } from '../../../services/ledger-account-type.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ledger-account-type-form-drawer',
  imports: [ CommonModule, FormsModule, ReactiveFormsModule ],
  templateUrl: './ledger-account-type-form-drawer.component.html',
  styleUrls: ['./ledger-account-type-form-drawer.component.scss']
})
export class LedgerAccountTypeFormDrawerComponent implements OnChanges {
  @Input() open = false;
  @Input() mode: 'create' | 'edit' = 'create';
  @Input() data: LedgerAccountTypeDto | null = null;

  @Output() closed = new EventEmitter<void>();
  @Output() saved = new EventEmitter<void>();

  loading = false;

    private fb = inject(FormBuilder);

  readonly kindOptions = [
    { label: 'Tài sản', value: 'Asset' },
    { label: 'Nợ phải trả', value: 'Liability' },
    { label: 'Vốn chủ sở hữu', value: 'Equity' },
    { label: 'Doanh thu', value: 'Revenue' },
    { label: 'Chi phí', value: 'Expense' },
  ];

  form = this.fb.nonNullable.group({
    id: [''],
    code: ['', [Validators.required, Validators.maxLength(64)]],
    name: ['', [Validators.required, Validators.maxLength(250)]],
    kind: ['Asset' as LedgerAccountKind, [Validators.required]],
    description: ['']
  });


  constructor(
    private service: LedgerAccountTypeService
  ) {}

  get fc() {
    return this.form.controls;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['open'] && this.open) {
      if (this.mode === 'edit' && !this.data) return;
      this.prepareForm();
    }
  }

  prepareForm() {
    if (this.mode === 'edit') {
      if(!this.data) return;

      this.form.setValue({
        id: this.data.id,
        code: this.data.code,
        name: this.data.name,
        kind: this.data.kind,
        description: this.data.description ?? '',
      });
    } else {
      this.form.reset({
        id: '',
        code: '',
        name: '',
        kind: 'Asset',
      });
    }
  }

  cancel() {
    this.closed.emit();
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const v = this.form.getRawValue();
    this.loading = true;

    const payload = {
      code: v.code.trim(),
      name: v.name.trim(),
      kind: v.kind,
      description: v.description?.trim() ?? null
    };

    const req = this.mode === 'edit'
      ? this.service.update(v.id, payload)
      : this.service.create(payload);

    req.subscribe({
      next: () => {
        this.loading = false;
        this.saved.emit();
        this.closed.emit();
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}
