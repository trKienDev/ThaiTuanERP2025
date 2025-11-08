import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { DepartmentOptionStore } from "../../../account/options/department-dropdown-options.option";
import { KitDropdownComponent } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { UserOptionStore } from "../../../account/options/user-dropdown-options.store";

@Component({
      selector: 'budget-plan-approvers-dialog',
      standalone: true,
      imports: [CommonModule, KitDropdownComponent, KitSpinnerButtonComponent],
      templateUrl: './budget-plan-approvers-dialog.component.html',
})
export class BudgetPlanApproversDialogComponent {
      private readonly dialog = inject(MatDialogRef<BudgetPlanApproversDialogComponent>);
      
      private readonly departmentOptionStore = inject(DepartmentOptionStore);
      departmentOptions$ = this.departmentOptionStore.option$;

      private readonly userOptionStore = inject(UserOptionStore);
      userOptions$ = this.userOptionStore.option$;

      submitting: boolean = false;


      close(result?: any) {
            this.dialog.close();
      }

}