import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Output } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { BudgetGroupModel } from "../../models/budget-group.model";
import { BudgetGroupService } from "../../services/budget-group.service";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";
import { CreateBudgetCodeRequest } from "../../models/budget-code.model";

@Component({
      selector: 'add-budget-code-modal',
      standalone: true,
      imports: [CommonModule, FormsModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule,
            MatAutocompleteModule
      ],
      templateUrl: './add-budget-code-modal.component.html',
      styleUrl: './add-budget-code-modal.component.scss',
})
export class AddBudgetCodeModalComponent {
      @Output() close = new EventEmitter<void>();
      @Output() save = new EventEmitter<{
            budgetCode: CreateBudgetCodeRequest,
            callback: (ok: boolean, message?: string) => void
      }>();

      budgetCode: CreateBudgetCodeRequest = {
            code: '',
            name: '',
            budgetGroupId: '',
      }
      budgetGroups: BudgetGroupModel[] = [];
      filteredBudgetGroups: BudgetGroupModel[] = [];

      constructor(private budgetGroupService: BudgetGroupService) {
            this.loadBudgetGroups();
      }

      loadBudgetGroups(): void {
            this.budgetGroupService.getAll().subscribe({
                  next: (data) => {
                        this.budgetGroups = data;
                        this.filteredBudgetGroups = data;
                  },
                  error: err => alert(handleHttpError(err).join('\n'))
            })
      }

      onBudgetGroupInputChange(value: string) {
            const lowerValue = value.toLowerCase();
            this.filteredBudgetGroups = this.budgetGroups.filter(bg => 
                  bg.name.toLowerCase().includes(lowerValue) || 
                  bg.code.toLowerCase().includes(lowerValue)
            );
      }
      onBudgetGroupSelected(budgetGroup: string) {
            this.budgetCode.budgetGroupId = budgetGroup
      }
      displayBudgetGroupFn = (budgetGroupId: string): string => {
            const budgetGroup = this.budgetGroups.find(bg => bg.id === budgetGroupId);
            return budgetGroup ? `${budgetGroup.code} - ${budgetGroup.name}` : '';
      }

      onSubmit() {
            this.save.emit({
                  budgetCode: this.budgetCode,
                  callback: (ok, message?: string) => {
                        if(ok) {
                              alert('Tạo ngân sách thành công');
                              this.onClose();
                        }
                        else alert(message || 'Lỗi tạo ngân sách');
                  }
            });
      }

      onClose() {
            this.close.emit();
      }
}