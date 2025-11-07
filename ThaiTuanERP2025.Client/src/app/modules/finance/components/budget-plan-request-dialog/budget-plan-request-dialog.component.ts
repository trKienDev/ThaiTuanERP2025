import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { MoneyFormatDirective } from "../../../../shared/directives/money/money-format.directive";
import { UserFacade } from "../../../account/facades/user.facade";
import { UserDto } from "../../../account/models/user.model";
import { BudgetPlanService } from "../../services/budget-plan.service";
import { BudgetPeriodsTableDialogComponent } from "../budget-periods-table-dialog/budget-periods-table-dialog.component";
import { KitDropdownOption, KitDropdownComponent } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { BudgetPeriodService } from "../../services/budget-period.service";
import { BudgetPeriodDto } from "../../models/budget-period.model";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";


@Component({
      selector: 'budget-plan-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MoneyFormatDirective, KitDropdownComponent],
      templateUrl: './budget-plan-request-dialog.component.html',
})
export class BudgetPlanRequestDialogComponent {
      private readonly matdialogRef = inject(MatDialogRef<BudgetPlanRequestDialogComponent>);
      private readonly matDialog = inject(MatDialog);
      private readonly formBuilder = inject(FormBuilder);
      
      private readonly userFacade = inject(UserFacade);
      currentUser$ = this.userFacade.currentUser$;
      currentUser: UserDto | null = null;

      private readonly budgetPlanSer = inject(BudgetPlanService);
      
      private readonly budgetPeriodService = inject(BudgetPeriodService);
      availableBudgetPeriods: BudgetPeriodDto[] = [];

      budgetPeriodOptions: KitDropdownOption[] = [];

      form = this.formBuilder.group({
            departmentId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            budgetPeriodId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            budgetCodeId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            amount: this.formBuilder.control<number>(0, { nonNullable: true, validators: [ Validators.required ]}),
      });

      constructor() {
            this.loadAvailableBudgetPeriods();
      }

      loadAvailableBudgetPeriods(): void {
            this.budgetPeriodService.getAvailable().subscribe({
                  next: (budgetPeriods) => {
                        this.budgetPeriodOptions = budgetPeriods.map(bp => ({
                              id: bp.id,
                              label: `Tháng: ${bp.month} - Năm: ${bp.year}`
                        }));
                  },
                  error: (err => handleHttpError(err))
            })
      }

      openBudgetPeriodsTableDialog(): void {
            this.matDialog.open(BudgetPeriodsTableDialogComponent);
      }


      close(result: boolean = false): void {
            this.matdialogRef.close(result);
      }
}