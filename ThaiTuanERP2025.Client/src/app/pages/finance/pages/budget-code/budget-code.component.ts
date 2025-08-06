import { CommonModule } from "@angular/common";
import { Component, ElementRef, ViewChild } from "@angular/core";
import { BudgetCodeModel, CreateBudgetCodeModel } from "../../models/budget-code.model";
import { BudgetCodeService } from "../../services/budget-code.service";
import { handleApiResponse } from "../../../../core/utils/handle-api-response.utils";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";
import { AddBudgetCodeModalComponent } from "../../components/add-budget-code/add-budget-code-modal.component";

@Component({
      selector: 'finance-budget-code',
      standalone: true,
      imports: [CommonModule, AddBudgetCodeModalComponent ],
      templateUrl: './budget-code.component.html',
      styleUrl: './budget-code.component.scss',
})
export class BudgetCodeComponent {
      showModal = false;
      
      newBudgetCode = { code: '', name: '' };
      successMessage: string | null = null;
      budgetCodes: (BudgetCodeModel & { selected: boolean})[] = [];
      importedBudgetCodes: BudgetCodeModel[] = [];

      @ViewChild('masterCheckbox', { static: false}) masterCheckbox!: ElementRef<HTMLInputElement>;

      constructor( private budgetCodeService: BudgetCodeService ) {}

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
}