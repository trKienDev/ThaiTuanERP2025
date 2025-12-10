import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { ReactiveFormsModule, FormBuilder, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { KitDropdownComponent, KitDropdownOption } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { DepartmentOptionStore } from "../../../account/options/department-dropdown.option";
import { UserOptionStore } from "../../../account/options/user-dropdown.option";
import { BudgetApproverDto, BudgetApproversRequest } from "../../models/budget-approvers.model";
import { firstValueFrom } from "rxjs";
import { BudgetApproverFacade } from "../../facades/budget-approver.facade";
import { HttpErrorHandlerService } from "../../../../core/services/http-errror-handler.service";

@Component({
      selector: 'budget-plan-approvers-dialog',
      standalone: true,
      imports: [CommonModule, KitDropdownComponent, KitSpinnerButtonComponent, ReactiveFormsModule ],
      templateUrl: './budget-approver-request-dialog.component.html',
})
export class BudgetPlanApproversDialogComponent implements OnInit {
      private readonly dialog = inject(MatDialogRef<BudgetPlanApproversDialogComponent>);
      private readonly formBuild = inject(FormBuilder);
      private readonly toast = inject(ToastService);
      private readonly budgetApproverFacade = inject(BudgetApproverFacade);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);

      title: string = 'User phê duyệt ngân sách';

      private readonly departmentOptionStore = inject(DepartmentOptionStore);
      departmentOptions$ = this.departmentOptionStore.option$;

      private readonly userOptionStore = inject(UserOptionStore);
      userOptions$ = this.userOptionStore.option$;

      
      submitting: boolean = false;
      showErrors: boolean = false;

      form = this.formBuild.group({
            approverId: this.formBuild.control<string>("", { nonNullable: true, validators: [ Validators.required ]}),
            slaHours: this.formBuild.control<number>(8, { nonNullable: true, validators: [ Validators.required ]}),
            departmentIds: this.formBuild.control<string[]>([], { nonNullable: true, validators: [ Validators.required ]})
      })

      constructor(
            @Inject(MAT_DIALOG_DATA) public data?: BudgetApproverDto
      ) {}

      ngOnInit(): void {
            if(this.data) {
                  this.form.patchValue({ 
                        approverId: this.data.approverUser.id,
                        slaHours: this.data.slaHours,
                        departmentIds: this.data.departments.map(d => d.id),
                  })
            }
      }

      onApproverSelected(opt: KitDropdownOption) {
            this.form.patchValue({ approverId: opt.id });
      }

      onDepartmentsSelected(opt: KitDropdownOption) {
            const id = typeof opt === 'string' ? opt : opt.id;
            const ctrl = this.form.controls.departmentIds;
            const current = ctrl.getRawValue() ?? [];
            if (!current.includes(id)) ctrl.setValue([...current, id]);
            ctrl.markAsDirty();
            ctrl.updateValueAndValidity();
      }

      async onSubmit(): Promise<void> {
            this.showErrors = true;
            if(this.form.invalid)  {
                  this.form.markAllAsTouched();
                  this.toast.warningRich("Vui lòng điền đẩy đủ thông tin ");
                  return;
            }

            try { 
                  this.submitting = true;
                  this.form.disable({ emitEvent: false });
                  
                  const payload: BudgetApproversRequest = this.form.getRawValue();

                  if(this.data?.id) {
                        console.log('update');
                        await firstValueFrom(this.budgetApproverFacade.update(this.data.id, payload));
                        this.toast.successRich("Cập nhật user duyệt ngân sách thành công");
                  } else {
                        console.log('create');
                        await firstValueFrom(this.budgetApproverFacade.create(payload));
                        this.toast.successRich("Thêm người duyệt ngân sách thành công");
                  }
                  
                  this.showErrors = false;
                  this.form.reset();
                  this.dialog.close(true);
            } catch(error) {   
                  this.httpErrorHandler.handle(error, this.data?.id ? 'cập nhật user duyệt ngân sách' : 'thêm user duyệt ngân sách');
            } finally {
                  this.form.disable({ emitEvent: false });
                  this.submitting = false;
            }

      }

      close(result?: any) {
            this.dialog.close();
      }

}