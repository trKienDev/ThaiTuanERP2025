import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { MoneyFormatDirective } from "../../../../shared/directives/money/money-format.directive";
import { UserFacade } from "../../../account/facades/user.facade";
import { BudgetPlanService } from "../../services/budget-plan.service";
import { BudgetPeriodsTableDialogComponent } from "../budget-periods-table-dialog/budget-periods-table-dialog.component";
import { KitDropdownOption, KitDropdownComponent } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { BudgetPeriodService } from "../../services/budget-period.service";
import { BudgetPeriodDto } from "../../models/budget-period.model";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";
import { take } from "rxjs";


@Component({
      selector: 'budget-plan-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MoneyFormatDirective, KitDropdownComponent],
      templateUrl: './budget-plan-request-dialog.component.html',
})
export class BudgetPlanRequestDialogComponent implements OnInit {
      private readonly matdialogRef = inject(MatDialogRef<BudgetPlanRequestDialogComponent>);
      private readonly matDialog = inject(MatDialog);
      private readonly formBuilder = inject(FormBuilder);
      
      private readonly userFacade = inject(UserFacade);
      currentUser$ = this.userFacade.currentUser$;

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

      ngOnInit(): void {
            this.loadAvailableBudgetPeriods();
            // Lấy departmentId của user hiện tại và patch vào form
            this.userFacade.currentUser$
                  .pipe(take(1))
                  .subscribe(user => {
                        if (user?.departmentId) {
                              this.form.patchValue({ departmentId: user.departmentId });
                        }
                  });
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