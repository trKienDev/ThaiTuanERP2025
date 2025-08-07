import { CommonModule } from "@angular/common";
import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { BudgetCodeModel, CreateBudgetCodeModel } from "../../models/budget-code.model";
import { BudgetCodeService } from "../../services/budget-code.service";
import { handleApiResponse } from "../../../../core/utils/handle-api-response.utils";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";
import { AddBudgetCodeModalComponent } from "../../components/add-budget-code/add-budget-code-modal.component";
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
      selector: 'finance-budget-code',
      standalone: true,
      imports: [CommonModule, AddBudgetCodeModalComponent, MatTooltipModule ],
      templateUrl: './budget-code.component.html',
      styleUrl: './budget-code.component.scss',
})
export class BudgetCodeComponent implements OnInit {
      showModal = false;
      newBudgetCode = { code: '', name: '' };
      successMessage: string | null = null;
      errorMessages: string[] = [];
      budgetCodes: (BudgetCodeModel & { selected: boolean})[] = [];
      importedBudgetCodes: BudgetCodeModel[] = [];

      @ViewChild('masterCheckbox', { static: false}) masterCheckbox!: ElementRef<HTMLInputElement>;

      constructor( private budgetCodeService: BudgetCodeService ) {}

      ngOnInit(): void {
            this.loadBudgetCodes();
      }

      loadBudgetCodes(): void {
            this.budgetCodeService.getAll().subscribe({
                  next: res => handleApiResponse(res, 
                        (data) => {
                              this.budgetCodes = data.map(bc => ({ ...bc, selected: false }));
                              this.updateMasterCheckboxState();
                        },
                        (errors) => {
                              alert(errors.join('\n'));
                        }
                  ),
                  error: err => alert(handleHttpError(err).join('\n'))
            });
      }

      createBudgetCode({ budgetCode, callback}: {
            budgetCode: CreateBudgetCodeModel,
            callback: (ok: boolean, message?: string) => void
      }) {
            this.budgetCodeService.create(budgetCode).subscribe({
                  next: res => handleApiResponse(res, 
                        () => {
                              this.loadBudgetCodes();
                              callback(true);
                        },
                        (errors) => callback(false, errors.join(', '))
                  ),
                  error: err => {
                        const messages = handleHttpError(err);
                        callback(false, messages.join(', '));
                  }
            })
      }

      toggleAll(event: Event): void {
            const checked = (event.target as HTMLInputElement).checked;
            this.budgetCodes.forEach(bc => bc.selected = checked);
            this.updateMasterCheckboxState();
      }
      updateMasterCheckboxState(): void {
            const allSelected = this.budgetCodes.every(bc => bc.selected);
            const noneSelected = this.budgetCodes.every(bc => !bc.selected);
            const checkbox = this.masterCheckbox?.nativeElement;
            if(checkbox) {
                  checkbox.indeterminate = !allSelected && !noneSelected;
                  checkbox.checked = allSelected;
            }
      }
      isAllSelected(): boolean {
            return this.budgetCodes.length > 0 && this.budgetCodes.every(bc => bc.selected);
      }

      onToggleStatus(budgetCode: BudgetCodeModel): void {
            const oldValue = budgetCode.isActive;
            budgetCode.isActive = !oldValue;

            this.budgetCodeService.updateStatus(budgetCode.id, budgetCode.isActive).subscribe({
                  next: res => handleApiResponse(res, 
                        () => {

                        },
                        (errors) => {
                              budgetCode.isActive = oldValue;
                              this.errorMessages = errors;
                              setTimeout(() => this.errorMessages = [], 4000);
                        }, 
                        () => {
                              budgetCode.isActive = !budgetCode.isActive;
                        }
                  ),
                  error: err => {
                        budgetCode.isActive = !budgetCode.isActive;
                        this.errorMessages = handleHttpError(err);
                  }
            })
      }
}