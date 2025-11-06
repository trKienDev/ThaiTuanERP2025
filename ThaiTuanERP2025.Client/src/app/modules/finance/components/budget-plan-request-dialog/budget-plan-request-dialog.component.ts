import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { MoneyFormatDirective } from "../../../../shared/directives/money/money-format.directive";
import { UserFacade } from "../../../account/facades/user.facade";
import { UserDto } from "../../../account/models/user.model";

@Component({
      selector: 'budget-plan-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MoneyFormatDirective],
      templateUrl: './budget-plan-request-dialog.component.html',
})
export class BudgetPlanRequestDialogComponent {
      private readonly matdialogRef = inject(MatDialogRef<BudgetPlanRequestDialogComponent>);
      private readonly formBuilder = inject(FormBuilder);
      
      private readonly userFacade = inject(UserFacade);
      currentUser$ = this.userFacade.currentUser$;
      currentUser: UserDto | null = null;

      form = this.formBuilder.group({
            departmentId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            budgetPeriodId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            budgetCodeId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            amount: this.formBuilder.control<number>(0, { nonNullable: true, validators: [ Validators.required ]}),
      })

      close(result: boolean = false): void {
            this.matdialogRef.close(result);
      }
}