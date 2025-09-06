import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { KitDropdownComponent, KitDropdownOption } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { SupplierService } from "../../../services/supplier.service";
import { SupplierDto } from "../../../models/supplier.model";
import { UserService } from "../../../../account/services/user.service";
import { UserDto } from "../../../../account/models/user.model";

@Component({
      selector: 'expense-payment',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MatInputModule, MatFormFieldModule,
            KitDropdownComponent

      ],
      templateUrl: './expense-payment.component.html',
      styleUrl: './expense-payment.component.scss',
})
export class ExpensePaymentComponent implements OnInit {
      private formBuilder = inject(FormBuilder);
      
      supplierOptions: KitDropdownOption[] = [];
      userOptions: KitDropdownOption[] = [];

      constructor(
            private supplierService: SupplierService,
            private userService: UserService,
      ) {}

      ngOnInit(): void {
            this.loadSuppliers();
            this.loadUsers();
      }

      loadSuppliers(): void {
            this.supplierService.getAll().subscribe({
                  next: (suppliers) =>  {
                        this.supplierOptions = suppliers.map(s => ({
                              id: s.id,
                              label: s.name,
                        }));
                  }
            })
      }
      onSupplierSelected(opt: KitDropdownOption) {
            alert(`Bạn đã chọn: ${opt.label} (id = ${opt.id})`);
      }

      loadUsers(): void {
            this.userService.getAllUsers().subscribe({
                  next: (users) => {
                        this.userOptions = users.map(s => ({
                              id: s.id,
                              label: s.fullName,
                        }));
                  }
            })
      }
      onUserSelected(opt: KitDropdownOption) {
            alert(`Bạn đã chọn: ${opt.label} (id = ${opt.id})`);
      }

      // reactive form
      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
      })

      payeeOptions: KitDropdownOption[] = [
            { id: 'supplier', label: 'Nhà cung cấp' },
            { id: 'employee', label: 'Nhân viên' },
      ];
      selectedPayee: 'supplier' | 'employee' | null = null;
      onPayeeSelected(opt: KitDropdownOption) {
            this.selectedPayee = opt.id === 'supplier' ? 'supplier' : 'employee';
      }

      
}
