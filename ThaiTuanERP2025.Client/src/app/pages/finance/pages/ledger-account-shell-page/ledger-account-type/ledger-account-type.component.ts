import { Component, EventEmitter, Input, Output } from '@angular/core';
import { LedgerAccountTypeDto } from '../../../models/ledger-account-type.dto';
import { LedgerAccountTypeService } from '../../../services/ledger-account-type.service';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LedgerAccountTypeFormDrawerComponent } from '../ledger-account-type-form-drawer/ledger-account-type-form-drawer.component';

@Component({
  selector: 'app-ledger-account-type',
  imports: [ CommonModule, FormsModule, ReactiveFormsModule, LedgerAccountTypeFormDrawerComponent],
  templateUrl: './ledger-account-type.component.html',
  styleUrls: ['./ledger-account-type.component.scss']
})
export class LedgerAccountTypeComponent {
  @Input() selectedTypeId: string | null = null;
  @Output() typeSelected = new EventEmitter<string>();

  types: LedgerAccountTypeDto[] = [];
  loading = false;
  searchTerm = '';

  drawerOpen = false;
  drawerMode: 'create' | 'edit' = 'create';
  selectedType: LedgerAccountTypeDto | null = null;

  constructor(private service: LedgerAccountTypeService) {}

  ngOnInit(): void {
    console.log('drawerOpen on init:', this.drawerOpen);
    this.loadTypes();
  }

  loadTypes() {
    this.loading = true;
    this.service.getAll().subscribe({
      next: (res) => {
        this.types = res;
        this.loading = false;

        // Nếu chưa có loại nào được chọn → auto chọn cái đầu tiên
        if (!this.selectedTypeId && this.types.length) {
          this.onSelect(this.types[0].id);
        }
      },
      error: () => {
        this.types = [];
        this.loading = false;
      }
    });
  }

  filtered(): LedgerAccountTypeDto[] {
    const keyword = this.searchTerm.toLowerCase();
    return this.types.filter(t =>
      t.code.toLowerCase().includes(keyword) || t.name.toLowerCase().includes(keyword)
    );
  }

  onSelect(id: string) {
    this.typeSelected.emit(id);
  }

  openCreate() {
    this.drawerMode = 'create';
    this.selectedType = null;
    this.drawerOpen = true;
  }

  openEdit(type: LedgerAccountTypeDto) {
    this.drawerMode = 'edit';
    this.selectedType = type;
    this.drawerOpen = true;
  }

  closeDrawer() {
    this.drawerOpen = false;
  }

 onSaved() {
    this.loadTypes();
  }


}
