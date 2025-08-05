import { CommonModule } from "@angular/common";
import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { BudgetGroupDto } from "../../dtos/budget-group.dto";
import { BudgetGroupService } from "../../services/budget-group.service";
import { FormsModule } from "@angular/forms";

@Component({
      selector: 'finance-budget-group',
      standalone: true,
      imports: [ CommonModule, FormsModule ],
      templateUrl: './budget-group.component.html',
      styleUrl: './budget-group.component.scss',
})
export class BudgetGroupComponent implements OnInit {
      newBudgetGroup = { code: '', name: '' };
      successMessage: string | null = null;
      errorMessages: string[] = [];
      budgetGroups: (BudgetGroupDto & { selected: boolean})[] = [];
      
      @ViewChild('masterCheckbox', { static: false }) masterCheckbox!: ElementRef<HTMLInputElement>;
     
      constructor(private budgetGroupService: BudgetGroupService) {}

      ngOnInit(): void {
            this.loadBudgetGroups();
      }

      loadBudgetGroups(): void {
            this.budgetGroupService.getAll().subscribe({
                  next: (res) => {
                        if(res.isSuccess && res.data) {
                              this.budgetGroups = res.data.map(bg => ({ ...bg, selected: false }),),
                              this.updateMasterCheckboxState();
                        } else {
                              this.errorMessages = res.errors ?? [res.message ?? 'Tải nhóm ngân sách thất bại'];
                        }
                  },
                  error: (err) => alert(err.message)
            })
      }

      createBudgetGroup(): void {
            this.budgetGroupService.create(this.newBudgetGroup).subscribe({
                  next: (res) => {
                        if(res.isSuccess) {
                              this.newBudgetGroup = { code: '', name: '' };
                              this.successMessage = 'Đã thêm nhóm ngân sách thành công!';
                              this.loadBudgetGroups();
                              setTimeout(() => this.successMessage = null, 3000);
                        } else {
                              this.errorMessages = res.errors ?? [res.message ?? 'Thêm nhóm ngân sách thất bại'];
                        }

                  },
                  error: (err) => alert(err.message)
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