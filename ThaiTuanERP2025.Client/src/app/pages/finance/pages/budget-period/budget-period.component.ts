import { CommonModule } from "@angular/common";
import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { BudgetPeriodModel, CreateBudgetPeriodModel } from "../../models/budget-period.model";
import { BudgetPeriodService } from "../../services/budget-period.service";
import { handleApiResponse } from "../../../../core/utils/handle-api-response.utils";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";

@Component({
      selector: 'finance-budget-period',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './budget-period.component.html',
      styleUrl: './budget-period.component.scss',
})
export class BudgetPeriodComponent implements OnInit{
      newYear!: number;
      newMonth!: number;
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
                  next: res => handleApiResponse(res,
                        (data) => {
                              this.budgetPeriods = data.map(bp => ({ ...bp, selected: false }));
                              this.updateMasterCheckboxState();
                        }, 
                        (errors) => {
                              this.errorMessages = errors;
                        }
                  ),
                  error: err => {
                        this.errorMessages = handleHttpError(err);
                  }
            })
      } 

      createBudgetPeriod() {
            if (!this.newYear || !this.newMonth || this.newMonth < 1 || this.newMonth > 12) {
                  this.errorMessages = ['Vui lòng nhập năm và tháng hợp lệ (tháng từ 1 đến 12)'];
                  return;
            }
            this.budgetPeriodService.create(this.newMonth, this.newYear).subscribe({
                  next: res => handleApiResponse(res, 
                        (data) => {
                              this.newYear = 0;
                              this.newMonth = 0;
                              this.loadBudgetPeriods();
                        },
                        (errors) => {
                              this.errorMessages = errors;
                        }
                  ), 
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