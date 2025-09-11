import { CommonModule } from "@angular/common";
import { Component, ElementRef, inject, OnInit, ViewChild } from "@angular/core";
import { BudgetCodeService } from "../../services/budget-code.service";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";
import { MatTooltipModule } from '@angular/material/tooltip';
import { BudgetCodeDto } from "../../models/budget-code.model";
import { MatDialog } from "@angular/material/dialog";
import { AddBudgetCodeRequestDialogComponent } from "./add-budget-code-request/add-budget-code-request.component";

@Component({
      selector: 'finance-budget-code',
      standalone: true,
      imports: [CommonModule,  MatTooltipModule ],
      templateUrl: './budget-code.component.html',
      styleUrl: './budget-code.component.scss',
})
export class BudgetCodeComponent implements OnInit {
      private dialog = inject(MatDialog);

      showModal = false;
      newBudgetCode = { code: '', name: '' };
      successMessage: string | null = null;
      errorMessages: string[] = [];
      budgetCodes: (BudgetCodeDto & { selected: boolean})[] = [];
      importedBudgetCodes: BudgetCodeDto[] = [];

      @ViewChild('masterCheckbox', { static: false}) masterCheckbox!: ElementRef<HTMLInputElement>;

      constructor( private budgetCodeService: BudgetCodeService ) {}

      ngOnInit(): void {
            this.loadBudgetCodes();
      }

      loadBudgetCodes(): void {
            this.budgetCodeService.getAll().subscribe({
                  next: (data) => {
                        this.budgetCodes = data.map(bc => ({ ...bc, selected: false }));
                        this.updateMasterCheckboxState();
                  },
                  error: err => alert(handleHttpError(err).join('\n'))
            });
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

      onToggleStatus(budgetCode: BudgetCodeDto): void {
            const oldValue = budgetCode.isActive;
            budgetCode.isActive = !oldValue;

            this.budgetCodeService.toggleActive(budgetCode.id, budgetCode.isActive).subscribe({
                  next: () => {},
                  error: err => {
                        budgetCode.isActive = !budgetCode.isActive;
                        this.errorMessages = handleHttpError(err);
                  }
            })
      }

      openAddBudgetCodeRequestDialog() {
            const ref = this.dialog.open(AddBudgetCodeRequestDialogComponent, {
                  width: 'fit-content',
            });

            ref.afterClosed().subscribe((result: { success?: boolean;} | undefined ) => {
            });
      }
}