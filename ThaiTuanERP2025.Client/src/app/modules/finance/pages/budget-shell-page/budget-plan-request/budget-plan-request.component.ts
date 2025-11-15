import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { BudgetPeriodService } from "../../../services/budget-period.service";
import { KitDropdownOption, KitDropdownComponent } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { DepartmentOptionStore } from "../../../../account/options/department-dropdown-options.option";
import { UserService } from "../../../../account/services/user.service";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { resolveAvatarUrl } from "../../../../../shared/utils/avatar.utils";
import { BudgetApproverService } from "../../../services/budget-approver.service";

@Component({
      selector: 'budget-plan-request',
      standalone: true,
      imports: [CommonModule, KitDropdownComponent],
      templateUrl: './budget-plan-request.component.html'
})
export class BudgetPlanRequestPanelComponent implements OnInit {
      private readonly formBuilder = inject(FormBuilder);
      public submitting: boolean = false;
      public showErrors: boolean = false;
      
      public budgetPeriodOptions: KitDropdownOption[] = [];
      private readonly userService = inject(UserService);

      ngOnInit(): void {
            this.loadBudgetApprovers();
            this.loadBudgetPeriodOptions();
            this.loadBudgetReviewers();
      }
      
      form = this.formBuilder.group({
            departmentId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            budgetPeriodId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            approverId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            reviewerId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
      });

      // ==== BudgetPeriod ====
      private readonly budgetPeriodService = inject(BudgetPeriodService);
      loadBudgetPeriodOptions() {
            this.budgetPeriodService.getAvailable().subscribe({
                  next: (budgetPeriods) => {
                        this.budgetPeriodOptions = budgetPeriods.map(bp => ({
                              id: bp.id,
                              label: `Tháng ${bp.month} - Năm ${bp.year}`
                        }))
                  },
                  error: (err => handleHttpError(err))
            })
      }

      // ==== Budget Reviewers ====
      public budgetReviewerOptions: KitDropdownOption[] = [];
      async loadBudgetReviewers(): Promise<void> {
            this.userService.getDepartmentManagersByUser().subscribe({
                  next: (managers) => {
                        this.budgetReviewerOptions = managers.map(m => ({
                              id: m.id,
                              label: m.fullName,
                              imgUrl: resolveAvatarUrl(m)
                        }))
                  },
                  error: (err => handleHttpError(err))
            })     
      }
      onReviewerSelected(opt: KitDropdownOption) {
            this.form.patchValue({ reviewerId: opt.id });
      }

      // ==== Budget Approvers ====
      private readonly budgetApproverService = inject(BudgetApproverService);
      budgetApproverOptions: KitDropdownOption[] = [];
      loadBudgetApprovers(): void {
            this.budgetApproverService.getByUserDepartment().subscribe({
                  next: (budgetApprovers) => {
                        this.budgetApproverOptions = budgetApprovers.map(ba => ({
                              id: ba.approverUser.id,
                              label: ba.approverUser.fullName,
                              imgUrl: resolveAvatarUrl(ba.approverUser)
                        }))
                  },
                  error: (err => handleHttpError(err))
            })
      }
      onApproverSelected(opt: KitDropdownOption) {
            this.form.patchValue({ approverId: opt.id });
      }

      // ==== Departments ====
      public departmentOptions$ = inject(DepartmentOptionStore).option$;
      onDepartmentSelected(opt: KitDropdownOption) {
            this.form.patchValue({ departmentId: opt.id });
      }
}