import { BudgetPlanService } from './../../services/budget-plan.service';
import { HttpErrorHandlerService } from './../../../../core/services/http-errror-handler.service';
import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { MoneyFormatDirective } from "../../../../shared/directives/money/money-format.directive";
import { UserFacade } from "../../../account/facades/user.facade";
import { BudgetPeriodsTableDialogComponent } from "../budget-periods-table-dialog/budget-periods-table-dialog.component";
import { KitDropdownOption, KitDropdownComponent } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { BudgetPeriodService } from "../../services/budget-period.service";
import { BudgetPeriodDto } from "../../models/budget-period.model";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";
import { BudgetApproverOptionStore } from "../../options/budget-approvers-options.store";
import { firstValueFrom, take } from "rxjs";
import { BudgetApproverService } from "../../services/budget-approver.service";
import { resolveAvatarUrl } from "../../../../shared/utils/avatar.utils";
import { environment } from "../../../../../environments/environment";
import { UserService } from "../../../account/services/user.service";
import { AmountToWordsPipe } from "../../../../shared/pipes/amount-to-words.pipe";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { BudgetPlanRequest } from "../../models/budget-plan.model";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { BudgetCodeService } from '../../services/budget-code.service';

@Component({
      selector: 'budget-plan-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MoneyFormatDirective, KitDropdownComponent, AmountToWordsPipe, KitSpinnerButtonComponent],
      templateUrl: './budget-plan-request-dialog.component.html',
})
export class BudgetPlanRequestDialogComponent implements OnInit {
      private readonly matdialogRef = inject(MatDialogRef<BudgetPlanRequestDialogComponent>);
      private readonly matDialog = inject(MatDialog);
      private readonly formBuilder = inject(FormBuilder);
      private readonly toast = inject(ToastService);
      private readonly baseUrl = environment.baseUrl;
      
      private readonly userFacade = inject(UserFacade);
      currentUser$ = this.userFacade.currentUser$;

      private readonly budgetApproverOptionsStore = inject(BudgetApproverOptionStore);
      budgetApprovers$ = this.budgetApproverOptionsStore.options$;
      private readonly budgetApproverService = inject(BudgetApproverService);
      budgetApproverOptions: KitDropdownOption[] = [];

      budgetReviewerOptions: KitDropdownOption[] = [];

      private readonly budgetPeriodService = inject(BudgetPeriodService);
      availableBudgetPeriods: BudgetPeriodDto[] = [];
      budgetPeriodOptions: KitDropdownOption[] = [];

     private readonly budgetCodeService = inject(BudgetCodeService);
     budgetCodeOptions: KitDropdownOption[] = [];

      private readonly userService = inject(UserService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly budgetPlanService = inject(BudgetPlanService);

      submitting: boolean = false;
      showErrors: boolean = false;

      form = this.formBuilder.group({
            departmentId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            budgetPeriodId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            budgetCodeId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            amount: this.formBuilder.control<number>(0, { nonNullable: true, validators: [ Validators.required ]}),
            approverId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            reviewerId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
      });

      ngOnInit(): void {
            this.loadAvailableBudgetPeriods();
            this.loadBudgetApproversByUserDepartment();
            this.loadAvailableBudgetPeriods();
            this.loadDepartmentManagers();
            this.loadAvailableBudgetCodes();
            // Lấy departmentId của user hiện tại và patch vào form
            this.userFacade.currentUser$
                  .pipe(take(1))
                  .subscribe(user => {
                        if (user?.departmentId) {
                              this.form.patchValue({ departmentId: user.departmentId });
                        }
                  });
      }

      async loadDepartmentManagers(): Promise<void> {
            this.userService.getDepartmentManagersByUser().subscribe({
                  next: (managers) => {
                        this.budgetReviewerOptions = managers.map(m => ({
                              id: m.id,
                              label: m.fullName,
                              imgUrl: resolveAvatarUrl(this.baseUrl, m)
                        }))
                  },
                  error: (err => handleHttpError(err))
            })     
      }

      loadBudgetApproversByUserDepartment(): void {
            this.budgetApproverService.getByUserDepartment().subscribe({
                  next: (budgetApprovers) => {
                        this.budgetApproverOptions = budgetApprovers.map(ba => ({
                              id: ba.approverUser.id,
                              label: ba.approverUser.fullName,
                              imgUrl: resolveAvatarUrl(this.baseUrl, ba.approverUser)
                        }))
                  },
                  error: (err => handleHttpError(err))
            })
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

      loadAvailableBudgetCodes() {
            this.budgetCodeService.getAvailable().subscribe({
                  next: (budgetCodes) => {
                        this.budgetCodeOptions = budgetCodes.map(bc => ({
                              id: bc.id,
                              label: `${bc.code} - ${bc.name}`
                        }));
                  },
                  error: (err => handleHttpError(err))
            })
      }

      onBudgetPeriodSelected(opt: KitDropdownOption) {
            this.form.patchValue({ budgetPeriodId: opt.id });
      }

      onBudgetCodeSelected(opt: KitDropdownOption) {
            this.form.patchValue({ budgetCodeId: opt.id });
      }

      onApproverSelected(opt: KitDropdownOption) {
            this.form.patchValue({ approverId: opt.id });
      }

      onReviewerSelected(opt: KitDropdownOption) {
            this.form.patchValue({ reviewerId: opt.id });
      }

      openBudgetPeriodsTableDialog(): void {
            this.matDialog.open(BudgetPeriodsTableDialogComponent);
      }

      async submit(): Promise<void> {
            this.showErrors = true;

            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich("Vui lòng điền đầy đủ thông tin");
                  return;
            }

            try {
                  this.submitting = true;
                  this.form.disable({ emitEvent: false });

                  const payload: BudgetPlanRequest = this.form.getRawValue();
                  console.log('payload: ', payload);
                  await firstValueFrom(this.budgetPlanService.create(payload));
                  this.toast.successRich("Tạo kế hoạch ngân sách thành công")
                  this.form.reset();
                  this.matdialogRef.close();
            } catch(error) {
                  this.httpErrorHandler.handle(error, 'Tạo kế hoạch ngân sách thất bại');
            } finally {
                  this.form.enable({ emitEvent: false })
                  this.showErrors = false;
                  this.submitting = false;
            }
      }

      close(result: boolean = false): void {
            this.matdialogRef.close(result);
      }
}