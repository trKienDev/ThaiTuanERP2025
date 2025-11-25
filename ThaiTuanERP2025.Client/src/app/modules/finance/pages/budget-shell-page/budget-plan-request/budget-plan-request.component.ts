import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { KitDropdownOption, KitDropdownComponent } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { DepartmentOptionStore } from "../../../../account/options/department-dropdown-options.option";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { resolveAvatarUrl } from "../../../../../shared/utils/avatar.utils";
import { MoneyFormatDirective } from "../../../../../shared/directives/money/money-format.directive";
import { KitSpinnerButtonComponent } from "../../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { BudgetPlanRequest } from "../../../models/budget-plan.model";
import { firstValueFrom } from "rxjs";
import { HttpErrorHandlerService } from "../../../../../core/services/http-errror-handler.service";
import { UserApiService } from "../../../../account/services/api/user-api.service";
import { BudgetPlanApiService } from "../../../services/api/budget-plan-api.service";
import { BudgetPeriodApiService } from "../../../services/api/budget-period-api.service";
import { BudgetCodeApiService } from "../../../services/api/budget-code-api.service";
import { BudgetApproverApiService } from "../../../services/api/budget-approver-api.service";
import { UserFacade } from "../../../../account/facades/user.facade";

interface BudgetPlanDetailsForm {
      budgetCodeId: FormControl<string | null>;
      amount: FormControl<number>;
      note: FormControl<string | null>;
}

@Component({
      selector: 'budget-plan-request',
      standalone: true,
      imports: [CommonModule, KitDropdownComponent, ReactiveFormsModule, MoneyFormatDirective, KitSpinnerButtonComponent],
      templateUrl: './budget-plan-request.component.html'
})
export class BudgetPlanRequestPanelComponent implements OnInit {
      private readonly formBuilder = inject(FormBuilder);
      submitting = false;
      showErrors = false;
      detailOptions: KitDropdownOption[][] = [];
      private readonly toast = inject(ToastService);
      private readonly userApi = inject(UserApiService);
      private readonly budgetPlanApi = inject(BudgetPlanApiService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly budgetPeriodApi = inject(BudgetPeriodApiService);
      private readonly currentUser$ = inject(UserFacade).currentUser$;

     
      
      // ===== FORM =====
      form = this.formBuilder.group({
            departmentId: this.formBuilder.control<string | null>(null, { nonNullable: true, validators: [ Validators.required ]}),
            budgetPeriodId: this.formBuilder.control<string | null>(null, { nonNullable: true, validators: [ Validators.required ]}),
            selectedApproverId: this.formBuilder.control<string | null>(null, { nonNullable: true, validators: [ Validators.required ]}),
            selectedReviewerId: this.formBuilder.control<string | null>(null, { nonNullable: true, validators: [ Validators.required ]}),
            details: this.formBuilder.array<FormGroup<BudgetPlanDetailsForm>>([ this.budgetPlanDetails()]),
      });

      async ngOnInit(): Promise<void> {
            this.loadBudgetApprovers();
            this.loadBudgetPeriodOptions();
            this.loadBudgetReviewers();
            this.loadAllBudgetCodes();

            let currentUser = await firstValueFrom(this.currentUser$); 
            this.form.patchValue({ departmentId: currentUser.departmentId }); 
      }

      budgetPlanDetails(): FormGroup<BudgetPlanDetailsForm> {
            return this.formBuilder.group({
                  budgetCodeId: this.formBuilder.control<string | null>(null, { nonNullable: true, validators: Validators.required }),
                  amount: this.formBuilder.control<number>(0, { nonNullable: true, validators: Validators.required }),
                  note: this.formBuilder.control<string | null>(null)
            });
      }
      get details(): FormArray<FormGroup<BudgetPlanDetailsForm>> {
            return this.form.controls.details;
      }

      trackByIndex = (_: number, __: any) => _;
      addDetail(): void {
            this.details.push(this.budgetPlanDetails());
            this.detailOptions.push([...this.budgetCodeOptions]);
            this.rebuildAvailableBudgetCodes();
      }
      removeDetail(i: number): void {
            this.details.removeAt(i);
            this.detailOptions.splice(i, 1);
            this.rebuildAvailableBudgetCodes();
      }

      // ==== Budget Period ====
      budgetPeriodOptions: KitDropdownOption[] = [];
      loadBudgetPeriodOptions() {
            this.budgetPeriodApi.getAvailable().subscribe({
                  next: (budgetPeriods) => {
                        this.budgetPeriodOptions = budgetPeriods.map(bp => ({
                              id: bp.id,
                              label: `Tháng ${bp.month} - Năm ${bp.year}`
                        }))
                  },
                  error: (err => handleHttpError(err))
            })
      }

      // ==== Bugdet Codes ====
     private readonly budgetCodeApi = inject(BudgetCodeApiService);
     budgetCodeOptions: KitDropdownOption[] = [];
     loadAllBudgetCodes() {
            this.budgetCodeApi.getAll().subscribe({
                  next: (budgetCodes) => {
                        this.budgetCodeOptions = budgetCodes.map(bc => ({
                              id: bc.id,
                              label: `${bc.code} - ${bc.name}`
                        }));
                        this.detailOptions = this.details.controls.map(_ => [...this.budgetCodeOptions]);
                  },
                  error: (err => handleHttpError(err))
            })
      }
      onBudgetCodeSelected(index: number, opt: KitDropdownOption) {
            this.details.at(index).patchValue({ budgetCodeId: opt.id });
            this.rebuildAvailableBudgetCodes();
      }

      // ==== Budget Reviewers ====
      public budgetReviewerOptions: KitDropdownOption[] = [];
      async loadBudgetReviewers(): Promise<void> {
            this.userApi.getDepartmentManagersByUser().subscribe({
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
            this.form.patchValue({ selectedReviewerId: opt.id });
      }

      // ==== Budget Approvers ====
      private readonly budgetApproverApi = inject(BudgetApproverApiService);
      budgetApproverOptions: KitDropdownOption[] = [];
      loadBudgetApprovers(): void {
            this.budgetApproverApi.getByUserDepartment().subscribe({
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
            this.form.patchValue({ selectedApproverId: opt.id });
      }

      // ==== Departments ====
      public departmentOptions$ = inject(DepartmentOptionStore).option$;
      onDepartmentSelected(opt: KitDropdownOption) {
            this.form.patchValue({ departmentId: opt.id });
      }

      // ==== SUBMIT ====
      async submit(): Promise<void> {
            this.showErrors = true;

            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich("Vui lòng điền đẩy đủ thông tin");
                  return;
            }

            try {
                  this.submitting = true;
                  this.form.disable({ emitEvent: false });

                  const raw = this.form.getRawValue();
                  const payload: BudgetPlanRequest = {
                        departmentId: raw.departmentId!,
                        budgetPeriodId: raw.budgetPeriodId!,
                        selectedReviewerId: raw.selectedReviewerId!,
                        selectedApproverId: raw.selectedApproverId!,
                        details: raw.details.map(d => ({
                              budgetCodeId: d.budgetCodeId!,
                              amount: d.amount!,
                              note: d.note
                        }))
                  };

                  await firstValueFrom(this.budgetPlanApi.create(payload));
                  this.toast.successRich("Tạo kế hoạch ngân sách thành công");
                  this.form.reset();
            } catch(error) {
                  this.httpErrorHandler.handle(error, 'Tạo kế hoạch ngân sách thất bại');
            } finally {
                  this.form.enable({ emitEvent: false })
                  this.showErrors = false;
                  this.submitting = false;
            }     
      }

      private rebuildAvailableBudgetCodes() {
            const selectedIds = this.details.controls
                  .map(ctrl => ctrl.get('budgetCodeId')?.value)
                  .filter(v => !!v);

            this.detailOptions = this.details.controls.map((ctrl, i) => {
                  const currentSelected = ctrl.get('budgetCodeId')?.value;

                  return this.budgetCodeOptions.filter(opt =>
                        opt.id === currentSelected || !selectedIds.includes(opt.id)
                  );
            });
      }
}