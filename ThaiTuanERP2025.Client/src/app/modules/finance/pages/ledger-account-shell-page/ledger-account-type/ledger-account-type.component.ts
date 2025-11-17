import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { LedgerAccountTypeDto, LedgerAccountTypeRequest } from '../../../models/ledger-account-type.dto';
import { LedgerAccountTypeApiService } from '../../../services/api/ledger-account-type-api.service';

@Component({
      selector: 'finance-ledger-account-type',
      standalone: true,
      imports: [CommonModule, FormsModule],
      templateUrl: './ledger-account-type.component.html',
      styleUrls: ['./ledger-account-type.component.scss']
})
export class LedgerAccountTypeComponent implements OnInit {
      @Output() typeSelected = new EventEmitter<string>();

      // listing
      types$!: Observable<LedgerAccountTypeDto[]>;

      // create
      createOpen = false;
      createForm: LedgerAccountTypeRequest = { 
            name: '', 
            code: '',
            ledgerAccountTypeKind: 1,
            description: '' 
      };

      // edit
      editOpen = false;
      editingId: string | null = null;
      editForm: LedgerAccountTypeRequest = { 
            name: '', 
            code: '', 
            ledgerAccountTypeKind: 1,
            description: '' 
      };

      readonly kindOptions = [
            { value: 1, label: 'Tài sản' },
            { value: 2, label: 'Nợ phải trả' },
            { value: 3, label: 'Vốn CSH' },
            { value: 4, label: 'Doanh thu' },
            { value: 5, label: 'Chi phí' },
      ];

      // map string -> number khi API trả "Asset" thay vì 1
      private readonly strToNum: Record<string, number> = {
            Asset: 1, Liability: 2, Equity: 3, Revenue: 4, Expense: 5,
      };

      constructor(private service: LedgerAccountTypeApiService) {}

      ngOnInit(): void {
            this.reload();
      }

      /** load danh sách */
      reload() {
            this.types$ = this.service.getAll();
      }

      // chọn 1 dòng -> báo về shell
      selectRow(id: string) {
            this.typeSelected.emit(id);
      }

      /** ==== CREATE ==== */
      openCreate() {
            this.createForm = { name: '', code: '', ledgerAccountTypeKind: 1, description: '' };
            this.createOpen = true;
      }

      cancelCreate() { this.createOpen = false; }

      submitCreate() {
            // gửi số 1..5
            this.createForm.ledgerAccountTypeKind = this.toNumberKind(this.createForm.ledgerAccountTypeKind);
            this.service.create(this.createForm).subscribe({
                  next: () => { this.createOpen = false; this.reload(); }
            });
      }

      /** ==== EDIT ==== */
      openEdit(item: LedgerAccountTypeDto) {
            this.editingId = item.id;
                  this.editForm = {
                  name: item.name,
                  code: item.code,
                  // chuẩn hóa về số nếu API trả "Asset"/"1"
                  ledgerAccountTypeKind: this.toNumberKind((item as any).ledgerAccountTypeKind),
                  description: item.description ?? ''
            };
            this.editOpen = true;
      }

      cancelEdit() { this.editOpen = false; this.editingId = null; }

      submitEdit() {
            if (!this.editingId) return;
            this.editForm.ledgerAccountTypeKind = this.toNumberKind(this.editForm.ledgerAccountTypeKind);
            this.service.update(this.editingId, this.editForm).subscribe({
                  next: () => { this.editOpen = false; this.editingId = null; this.reload(); }
            });
      }

      /** ==== DELETE ==== */
      delete(id: string) {
            if (!confirm('Bạn chắc chắn muốn xóa loại tài khoản này?')) return;
            this.service.delete(id).subscribe({ next: _ => this.reload() });
      }

      /** Chuẩn hóa giá trị enum: "Asset" | 1 | "1" -> 1..5 */
  toNumberKind(value: number | string | undefined): number {
    if (value == null) return 1;
    if (typeof value === 'number') return value;
    const n = Number(value);
    if (!Number.isNaN(n)) return n as 1|2|3|4|5;
    return this.strToNum[value] ?? 1;
  }

  /** Map 1..5 (hoặc "Asset") -> nhãn tiếng Việt */
  kindLabel(kind: number | string | undefined) {
    const n = this.toNumberKind(kind);
    const map: Record<number, string> = {
      1: 'Tài sản',
      2: 'Nợ phải trả',
      3: 'Vốn CSH',
      4: 'Doanh thu',
      5: 'Chi phí',
    };
    return map[n] ?? '';
  }
}
