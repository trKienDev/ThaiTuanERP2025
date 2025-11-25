import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BudgetPlanDetailDto } from "../../../finance/models/budget-plan.model";
import { BudgetPlanApiService } from "../../../finance/services/api/budget-plan-api.service";
import { firstValueFrom } from "rxjs";

@Component({
      selector: 'available-budget-plans-dialog',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './available-budget-plans-dialog.component.html'
})
export class AvailableBudgetPlansDialogComponent implements OnInit {
      private readonly dialogRef = inject(MatDialogRef<AvailableBudgetPlansDialogComponent>);
      private readonly budgetPlanApi = inject(BudgetPlanApiService);
      
      public availableDetails: BudgetPlanDetailDto[] = [];
      public selectedDetailId?: string;

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: string
      ) {
            this.selectedDetailId = data;
      }

      ngOnInit(): void {
            this.loadAvailableDetails();
      }

      async loadAvailableDetails() {
            this.availableDetails = await firstValueFrom(this.budgetPlanApi.getAvailableDetails());
      }

      selectBudgetPlanDetail(detail: BudgetPlanDetailDto) {
            this.dialogRef.close({
                  detailId: detail.id,
                  amount: detail.amount
            });
      }

      close() {
            this.dialogRef.close();
      }
}