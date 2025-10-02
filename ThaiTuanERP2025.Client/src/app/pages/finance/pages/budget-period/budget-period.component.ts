import { CommonModule } from "@angular/common";
import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { BudgetPeriodModel } from "../../models/budget-period.model";
import { BudgetPeriodService } from "../../services/budget-period.service";
import { handleApiResponse } from "../../../../shared/utils/handle-api-response.utils";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";
import { FormsModule } from "@angular/forms";

@Component({
      selector: 'finance-budget-period',
      standalone: true,
      imports: [ CommonModule, FormsModule ],
      templateUrl: './budget-period.component.html',
      styleUrl: './budget-period.component.scss',
})
export class BudgetPeriodComponent implements OnInit{
      year!: number;
      month!: number;
      sucessMessage: string | null = null;
      errorMessages: string[] = [];
      budgetPeriods: (BudgetPeriodModel & { selected: boolean})[] = [];

      @ViewChild('masterCheckbox', {static: false}) masterCheckbox!: ElementRef<HTMLInputElement>;
 
      constructor(private budgetPeriodService: BudgetPeriodService) {}

      ngOnInit(): void {
            this.loadBudgetPeriods();
      }

      loadBudgetPeriods() {
            this.budgetPeriodService.getAll().subscribe({
                  next: (data) => {
                        this.budgetPeriods = data.map(bp => ({ ...bp, selected: false }));
                        this.updateMasterCheckboxState();
                  }, 
                  error: err => {
                        this.errorMessages = handleHttpError(err);
                  }
            })
      } 

      createBudgetPeriod() {
            if (!this.year || !this.month || this.month < 1 || this.month > 12) {
                  this.errorMessages = ['Vui lòng nhập năm và tháng hợp lệ (tháng từ 1 đến 12)'];
                  return;
            }

            this.budgetPeriodService.create({ month: this.month, year: this.year }).subscribe({
                  next: (data) => {
                        this.year = 0;
                        this.month = 0;
                        this.loadBudgetPeriods();
                  },
                  error: err => {
                        this.errorMessages = handleHttpError(err);
                  }
            })
      }

      toggleAll(event: Event): void {
            const checked = (event.target as HTMLInputElement).checked;
            this.budgetPeriods.forEach(bp => bp.selected = checked);
            this.updateMasterCheckboxState();
      }
      updateMasterCheckboxState(): void {
            const allSelected = this.budgetPeriods.every(bp => bp.selected);
            const noneSelected = this.budgetPeriods.every(bp => !bp.selected);
            const checkbox = this.masterCheckbox?.nativeElement;
            if(checkbox) {
                  checkbox.indeterminate = !allSelected && !noneSelected;
                  checkbox.checked = allSelected;
            }
      }
      isAllSelected(): boolean {
            return this.budgetPeriods.length > 0 && this.budgetPeriods.every(bp => bp.selected);
      }
}