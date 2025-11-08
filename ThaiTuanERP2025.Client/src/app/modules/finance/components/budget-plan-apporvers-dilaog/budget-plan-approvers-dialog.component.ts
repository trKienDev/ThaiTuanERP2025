import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { DepartmentOptionStore } from "../../../account/options/department-dropdown-options.option";
import { KitDropdownComponent } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { UserOptionStore } from "../../../account/options/user-dropdown-options.store";
import { ToastService } from '../../../../shared/components/kit-toast-alert/kit-toast-alert.service';

@Component({
      selector: 'budget-plan-approvers-dialog',
      standalone: true,
      imports: [CommonModule, KitDropdownComponent, KitSpinnerButtonComponent, ReactiveFormsModule ],
      templateUrl: './budget-plan-approvers-dialog.component.html',
})
export class BudgetPlanApproversDialogComponent {
      private readonly dialog = inject(MatDialogRef<BudgetPlanApproversDialogComponent>);
      private readonly formBuild = inject(FormBuilder);
      private readonly toast = inject(ToastService);
      
      private readonly departmentOptionStore = inject(DepartmentOptionStore);
      departmentOptions$ = this.departmentOptionStore.option$;

      private readonly userOptionStore = inject(UserOptionStore);
      userOptions$ = this.userOptionStore.option$;

      submitting: boolean = false;
      showErrors: boolean = true;

      form = this.formBuild.group({
            approverId: this.formBuild.control<string>("", { nonNullable: true, validators: [ Validators.required ]}),
            slaHours: this.formBuild.control<number>(8, { nonNullable: true, validators: [ Validators.required ]}),
            departmentIds: this.formBuild.control<string[]>([], { nonNullable: true, validators: [ Validators.required ]})
      })

      async onSubmit(): Promise<void> {
            this.showErrors = true;
            if(this.form.invalid)  {
                  this.form.markAllAsTouched();
                  this.toast.warningRich("Vui lòng điền đẩy đủ thông tin ");
            }

            try { 
                  this.submitting = true;
                  this.form.disable({ emitEvent: false });
                  
            }


      }

      close(result?: any) {
            this.dialog.close();
      }

}