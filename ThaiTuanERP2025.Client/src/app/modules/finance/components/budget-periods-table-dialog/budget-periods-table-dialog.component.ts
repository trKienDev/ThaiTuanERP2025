import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { BudgetPeriodFacade } from "../../facades/budget-period.facade";

@Component({
      selector: 'budget-periods-table-dialog',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './budget-periods-table-dialog.component.html',
})
export class BudgetPeriodsTableDialogComponent {
      private readonly dialogRef = inject(MatDialogRef<BudgetPeriodsTableDialogComponent>);
      private readonly toast = inject(ToastService);
      private readonly budgetPeriodFacade = inject(BudgetPeriodFacade);
      budgetPeriods$ = this.budgetPeriodFacade.budgetPeriods$;
      


      close(result?: any) {
            this.dialogRef.close();
      }


}
