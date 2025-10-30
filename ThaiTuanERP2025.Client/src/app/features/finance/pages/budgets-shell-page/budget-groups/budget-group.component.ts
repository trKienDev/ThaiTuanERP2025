import { CommonModule } from "@angular/common";
import { Component, ElementRef, ViewChild } from "@angular/core";
import { BudgetGroupModel } from "../../../models/budget-group.model";
import { BudgetGroupService } from "../../../services/budget-group.service";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { FormsModule } from "@angular/forms";

@Component({
      selector: 'budget-groups-panel',
      standalone: true,
      imports: [ CommonModule, FormsModule ],
      templateUrl: './budget-group.component.html'
})
export class BudgetGroupPanelComponent {
      newBudgetGroup = { code: '', name: '' };
      successMessage: string | null = null;
      errorMessages: string[] = [];
      budgetGroups: (BudgetGroupModel & { selected: boolean})[] = [];
      public submitting = false;
      
      @ViewChild('masterCheckbox', { static: false }) masterCheckbox!: ElementRef<HTMLInputElement>;
     
      constructor(private budgetGroupService: BudgetGroupService) {}

      ngOnInit(): void {
            this.loadBudgetGroups();
      }

      loadBudgetGroups(): void {
            this.budgetGroupService.getAll().subscribe({
                  next: (data) => {
                        this.budgetGroups = data.map(bg => ({ ...bg, selected: false }));
                        this.updateMasterCheckboxState?.();
                  },
                  error: err => {
                        this.errorMessages = handleHttpError(err);
                  }
            });
      }

      createBudgetGroup(): void {
            this.newBudgetGroup.code = this.newBudgetGroup.code.toUpperCase();
            this.budgetGroupService.create(this.newBudgetGroup).subscribe({
                  next: (data) => {
                        this.newBudgetGroup = { code: '', name: '' };
                        this.successMessage = 'Đã thêm nhóm ngân sách thành công';
                        this.loadBudgetGroups();
                        setTimeout(() => this.successMessage = null, 3000);
                  },
                  error: err => {
                        this.errorMessages = handleHttpError(err);
                  }
            });
      }

      toggleAll(event: Event): void {
            const checked = (event.target as HTMLInputElement).checked;
            this.budgetGroups.forEach(bg => bg.selected = checked);
            this.updateMasterCheckboxState();
      }
      updateMasterCheckboxState(): void {
            const allSelected = this.budgetGroups.every(bg => bg.selected);
            const noneSelected = this.budgetGroups.every(bg => !bg.selected);
            const checkbox = this.masterCheckbox?.nativeElement;
            if(checkbox) {
                  checkbox.indeterminate = !allSelected && !noneSelected;
                  checkbox.checked = allSelected;
            }
      }
      isdAllSelected(): boolean {
            return this.budgetGroups.length > 0 && this.budgetGroups.every(d => d.selected);
      }
}