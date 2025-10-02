import { CommonModule } from '@angular/common';
import { Component, ChangeDetectionStrategy, signal, computed, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormControl } from '@angular/forms';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { debounceTime, distinctUntilChanged, startWith, takeUntil } from 'rxjs';
import { Subject } from 'rxjs';
import { LedgerAccountRequest, LedgerAccountBalanceTypeEnum, LedgerAccountDto, LedgerAccountRow } from '../../../models/ledger-account.model';
import { LedgerAccountService } from '../../../services/ledger-account.service';
import { LedgerAccountTypeService } from '../../../services/ledger-account-type.service';

@Component({
      selector: 'finance-ledger-account',
      standalone: true,
      imports: [CommonModule, FormsModule, ReactiveFormsModule, ScrollingModule],
      templateUrl: './ledger-account.component.html',
      styleUrls: ['./ledger-account.component.scss'],
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class LedgerAccountComponent implements OnInit {
      private destroy$ = new Subject<void>();

      // ======== STATE ========
      private rows$ = signal<LedgerAccountRow[]>([]);
      searchCtl = new FormControl<string>('', { nonNullable: true });

      // Drawer state
      drawerOpen = signal(false);
      isEditMode = signal(false);
      editingId = signal<string | null>(null);

      // Form model
      form = signal<LedgerAccountRequest>({
            number: '',
            name: '',
            ledgerAccountTypeId: '' as any,
            ledgerAccountBalanceType: 1,
            parentLedgerAccountId: null,
            description: '',
            isActive: true,
      });

      // Autocomplete parent input & suggestions
      parentKeywordCtl = new FormControl<string>('', { nonNullable: true });
      parentSuggestions = signal<{ id: string; number: string; name: string; level: number }[]>([]);

      // Select Loại tài khoản
      ledgerAccountTypes = signal<{ id: string; name: string }[]>([]);

      // Mapping BalanceType
      private readonly strToNum: Record<string, LedgerAccountBalanceTypeEnum> = {
            Debit: 1, Credit: 2, Both: 3, None: 4
      };
      private readonly numToLabel: Record<number, string> = {
            1: 'Dư Nợ',
            2: 'Dư Có',
            3: 'Lưỡng tính',
            4: 'Không xác định',
      };

      // danh sách sau filter (render vào virtual scroll)
      private _filteredRows = signal<LedgerAccountRow[]>([]);
      filteredRows = computed(() => this._filteredRows());

      constructor(
            private service: LedgerAccountService,
            private typeService: LedgerAccountTypeService
      ) {}

      ngOnInit(): void {
            this.loadAll();

            // load loại tài khoản cho select
            this.typeService.getAll().subscribe(types => {
                  // nếu service của bạn có hàm khác tên, sửa lại cho khớp
                  const opts = (types || []).map(t => ({ id: (t as any).id, name: (t as any).name }));
                  this.ledgerAccountTypes.set(opts);
            });

            const search$ = this.searchCtl.valueChanges.pipe(
            startWith(''),
            debounceTime(300),
            distinctUntilChanged()
      );


      this.searchCtl.valueChanges
      .pipe(startWith(''), debounceTime(300), distinctUntilChanged(), takeUntil(this.destroy$))
      .subscribe((kw) => {
      const keyword = (kw ?? '').trim().toLowerCase();
      const rows: LedgerAccountRow[] = this.rows$();  // lấy từ signal

      if (!keyword) {
            this._filteredRows.set(rows);
            return;
      }

      this._filteredRows.set(
            rows.filter((r: LedgerAccountRow) => r.name.toLowerCase().includes(keyword))
      );
      });

      // Autocomplete parent – lọc toàn bộ list (không theo loại)
      this.parentKeywordCtl.valueChanges
            .pipe(startWith(''), debounceTime(200), distinctUntilChanged(), takeUntil(this.destroy$))
            .subscribe(q => {
            const kw = (q ?? '').trim().toLowerCase();
            const excludeId = this.editingId();
            const all = this.rows$();
            const result = all
            .filter(r =>
                  (!excludeId || r.id !== excludeId) &&
                  (r.name.toLowerCase().includes(kw) || r.number.toLowerCase().includes(kw))
            )
            .slice(0, 20)
            .map(r => ({ id: r.id, number: r.number, name: r.name, level: r.level }));
            this.parentSuggestions.set(result);
            });
      }

      ngOnDestroy() {
            this.destroy$.next();
            this.destroy$.complete();
      }

      /** Load toàn bộ ledger-accounts (phẳng) rồi build rows + childrenCount */
      private loadAll() {
            this.service.getAll().subscribe(list => {
                  const rows = this.toRows(list || []);
                  console.log('rows: ', rows);
                  this.rows$.set(rows);
                  this._filteredRows.set(rows);
            });
      }

      /** Convert danh sách phẳng -> rows giữ thứ tự theo path/level + tính childrenCount */
      private toRows(list: LedgerAccountDto[]): LedgerAccountRow[] {
            // group children count
            const childCount = new Map<string, number>();
            for (const a of list) {
                  const pid = ((a as any).parentLedgerAccountId) as string | null | undefined;
                  if (pid) childCount.set(pid, (childCount.get(pid) || 0) + 1);
            }

            // sort theo path đã có từ BE (đã đúng cha trước con)
            const ordered = [...list].sort((a, b) => a.path.localeCompare(b.path));

            const rows: LedgerAccountRow[] = ordered.map(a => {
                  const pid = ((a as any).parentLedgerAccountId) as string | null | undefined;
                  return {
                        id: a.id,
                        number: a.number,
                        name: a.name,
                        typeId: a.ledgerAccountTypeId,
                        typeName: a.ledgerAccountTypeName ?? null,
                        balanceType: a.ledgerAccountBalanceType,
                        description: a.description ?? '',
                        level: a.level,
                        hasChildren: (childCount.get(a.id) || 0) > 0,
                        childrenCount: childCount.get(a.id) || 0,
                        isActive: a.isActive,
                        parentId: pid ?? null,
                  };
            });

            return rows;
      }

      // ====== BalanceType helpers ======
      private toBalanceNum(v: number | string | undefined): LedgerAccountBalanceTypeEnum {
            if (v == null) return 1;
            if (typeof v === 'number') return (v as LedgerAccountBalanceTypeEnum);
            const n = Number(v);
            if (!Number.isNaN(n)) return (n as LedgerAccountBalanceTypeEnum);
            return this.strToNum[v] ?? 1;
      }

      balanceLabel(v: number | string | undefined) {
            return this.numToLabel[this.toBalanceNum(v)] ?? '';
      }

      // ====== Toggle IsActive (optimistic) ======
      toggleActive(row: LedgerAccountRow, ev: Event) {
            const input = ev.target as HTMLInputElement;
            const next = input.checked;
            const prev = row.isActive;
            row.isActive = next; // optimistic
            this.service.toggleActive(row.id, next).subscribe({
                  error: _ => { row.isActive = prev; } // rollback on error
            });
      }

      // ====== Drawer – Create / Edit ======
      openCreate() {
            this.isEditMode.set(false);
            this.editingId.set(null);
            this.form.set({
                  number: '',
                  name: '',
                  ledgerAccountTypeId: this.ledgerAccountTypes()[0]?.id ?? '' as any,
                  ledgerAccountBalanceType: 1,
                  parentLedgerAccountId: null,
                  description: '',
                  isActive: true,
            });
            this.parentKeywordCtl.setValue('');
            this.drawerOpen.set(true);
      }

      openEdit(row: LedgerAccountRow) {
            this.isEditMode.set(true);
            this.editingId.set(row.id);
            this.form.set({
                  number: row.number,
                  name: row.name,
                  ledgerAccountTypeId: (row.typeId ?? '') as any,
                  ledgerAccountBalanceType: this.toBalanceNum(row.balanceType),
                  parentLedgerAccountId: row.parentId ?? null,
                  description: row.description ?? '',
                  isActive: row.isActive,
            });
            this.parentKeywordCtl.setValue('');
            this.drawerOpen.set(true);
      }

      closeDrawer() { this.drawerOpen.set(false); }

      pickParent(suggestId: string) {
            this.form.update(f => ({ ...f, parentLedgerAccountId: suggestId }));
      }
      clearParent() {
            this.form.update(f => ({ ...f, parentLedgerAccountId: null }));
      }

      submit() {
            const model = this.form();
            if (!model.number?.trim() || !model.name?.trim() || !model.ledgerAccountTypeId) return;

            if (!this.isEditMode()) {
                  const payload = { ...model };
                  this.service.create(payload).subscribe({
                        next: _ => { 
                              this.closeDrawer(); 
                              this.loadAll(); 
                        }
                  });
            } else {
                  const id = this.editingId();
                  if (!id) return;
                  const payload: LedgerAccountRequest = { ...model };
                  this.service.update(id, payload).subscribe({
                        next: _ => { 
                              this.closeDrawer(); 
                              this.loadAll(); 
                        }
                  });
            }
      }

      delete(row: LedgerAccountRow) {
            if (row.hasChildren) {
                  if (!confirm('Tài khoản này có tài khoản con. Bạn vẫn muốn xóa?')) return;
            } else {
                  if (!confirm('Xóa tài khoản này?')) return;
            }
            this.service.delete(row.id).subscribe({ next: _ => this.loadAll() });
      }

      // TrackBy cho virtual scroll
      trackById = (_: number, r: LedgerAccountRow) => r.id;
}
