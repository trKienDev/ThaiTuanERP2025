import { Component, EventEmitter, inject, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { LedgerAccountTreeDto } from '../../../models/ledger-account-tree.dto';
import { LedgerAccountService } from '../../../services/ledger-account.service';
import { CreateLedgerAccountRequest, LedgerAccountBalanceType, UpdateLedgerAccountRequest } from '../../../models/ledger-account.dto';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ledger-account-form-drawer',
  imports: [ CommonModule, ReactiveFormsModule ],
  templateUrl: './ledger-account-form-drawer.component.html',
  styleUrls: ['./ledger-account-form-drawer.component.scss'],
})
export class LedgerAccountFormDrawerComponent implements OnChanges {
  @Input() open = false;
  @Input() mode: 'create' | 'edit' = 'create';
  @Input() ledgerAccountTypeId!: string;
  @Input() currentNode: LedgerAccountTreeDto | null = null;

  @Output() closed = new EventEmitter<void>();
  @Output() saved = new EventEmitter<void>();
   private fb = inject(FormBuilder); 

  loading = false;

  form = this.fb.nonNullable.group({
    id: [''],
    code: ['', [Validators.required, Validators.maxLength(64)]],
    name: ['', [Validators.required, Validators.maxLength(250)]],
    description: ['' as string | null, [Validators.maxLength(500)]],
    isActive: [true],
    balanceType: ['Debit' as LedgerAccountBalanceType, [Validators.required]] 
  });

  constructor(private service: LedgerAccountService) {}

  get fc() {
    return this.form.controls;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['open'] && this.open) {
      this.prepareForm();
    }
  }

  prepareForm() {
    if (this.mode === 'edit' && this.currentNode) {
      this.form.setValue({
        id: this.currentNode.id,
        code: this.currentNode.code,
        name: this.currentNode.name,
        description: this.currentNode.description ?? '',
        isActive: this.currentNode.isActive,
        balanceType: this.currentNode.balanceType ?? 'Debit'
      });
    } else {
      this.form.reset({
        id: '',
        code: '',
        name: '',
        description: '',
        isActive: true,
        balanceType: 'Debit',
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

    this.loading = true;

    const v = this.form.getRawValue();

    const payloadBase = {
      code: v.code.trim(),
      name: v.name.trim(),
      description: (v.description || '').trim() || null,
      typeId: this.ledgerAccountTypeId,
      parentId: this.mode === 'create' && this.currentNode ? this.currentNode.id : null,
      balanceType: v.balanceType
    };

    const req = this.mode === 'create'
      ? this.service.create(payloadBase as CreateLedgerAccountRequest)
      : this.service.update(v.id, payloadBase as UpdateLedgerAccountRequest);

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
