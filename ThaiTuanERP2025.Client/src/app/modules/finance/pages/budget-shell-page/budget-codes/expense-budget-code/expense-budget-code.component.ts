import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BudgetCodeApiService } from "../../../../services/api/budget-code-api.service";
import { BudgetCodeWithAmountDto } from "../../../../models/budget-code.model";
import { handleHttpError } from "../../../../../../shared/utils/handle-http-errors.util";

@Component({
      selector: 'expense-budget-code',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './expense-budget-code.component.html',
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class ExpenseBudgetCodeDialogComponent implements OnInit {
      private ref = inject(MatDialogRef<ExpenseBudgetCodeDialogComponent>);
      private budgetCodeApi = inject(BudgetCodeApiService);
      private cdr = inject(ChangeDetectorRef);

      budgetCodes: BudgetCodeWithAmountDto[] = [];
      loading = false;
      selectedBudgetCodeId?: string;

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: any
      ) {
            this.selectedBudgetCodeId = data?.selectedBudgetCodeId;
      }

      ngOnInit(): void {
            this.getBudgetCodesForCurrentPeriod();
      }

      getBudgetCodesForCurrentPeriod(): void {
            this.loading = true;
            this.budgetCodeApi.getWithAmount().subscribe({
                  next: (list) => {
                        this.budgetCodes = list ?? [];
                        this.loading = false;
                        this.cdr.markForCheck();
                  },
                  error: (err) => {
                        this.loading = false;
                        this.cdr.markForCheck();
                        handleHttpError(err);
                  }
            })
      }

      selectBudgetCode(bc: BudgetCodeWithAmountDto) {
            this.ref.close({
                  success: true,
                  budgetCodeId: bc.id,
                  budgetAmount: bc.amount,
            });
      }

      close(): void {
            this.ref.close();
      }

      trackById = (_: number, bc: BudgetCodeWithAmountDto) => bc.id;
}