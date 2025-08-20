import { CommonModule } from "@angular/common";
import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { BudgetPlanService } from "../../services/budget-plan.service";
import { BudgetPlanModel, CreateBudgetPlanModel } from "../../models/budget-plan.model";
import { BudgetCodeModel } from "../../models/budget-code.model";
import { DepartmentService } from "../../../account/services/department.service";
import { BudgetCodeService } from "../../services/budget-code.service";
import { BudgetPeriodService } from "../../services/budget-period.service";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";
import { FormsModule } from "@angular/forms";
import { DepartmentModel } from "../../../account/models/department.model";
import { BudgetPeriodModel } from "../../models/budget-period.model";
import { MatInputModule } from "@angular/material/input";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatAutocompleteModule } from "@angular/material/autocomplete";

@Component({
      selector: 'finance-budget-plan',
      standalone: true,
      imports: [ CommonModule, FormsModule, MatInputModule, MatFormFieldModule, MatAutocompleteModule ],
      templateUrl: './budget-plan.component.html',
      styleUrl: './budget-plan.component.scss',
})
export class BudgetPlanComponent implements OnInit {
      budgetPlans: (BudgetPlanModel & { selected: boolean })[] = [];

      departments: DepartmentModel[] = [];
      filteredDepartments: DepartmentModel[] = [];

      budgetCodes: BudgetCodeModel[] = [];
      budgetPeriods: BudgetPeriodModel[] = [];
      successMessage: string | null = null;

      formData: CreateBudgetPlanModel = {
            departmentId: '',
            budgetCodeId: '',
            budgetPeriodId: '',
            amount: 0
      };

      constructor(
            private budgetPlanService: BudgetPlanService,
            private departmentService: DepartmentService,
            private budgetCodeService: BudgetCodeService,
            private budgetPeriodService: BudgetPeriodService,
      ) {}

      @ViewChild('masterCheckbox', { static: false }) masterCheckBox!: ElementRef<HTMLInputElement>;

      ngOnInit(): void {
            this.loadBudgetPlans();
            this.loadDepartments();
      }

      loadBudgetPlans(): void {
            this.budgetPlanService.getAll().subscribe({
                  next: (data) => {
                        console.log('data: ', data);
                        this.budgetPlans = data.map(plan => ({ ...plan, selected: false}));
                        this.updateMasterCheckboxState();
                  },
                  error: err => alert(handleHttpError(err).join('\n'))
            });

            this.budgetCodeService.getAllActive().subscribe({
                  next: (data) => {
                        this.budgetCodes = data;
                  },
                  error: err => alert(handleHttpError(err).join('\n'))
            })

            this.budgetPeriodService.getAllActive().subscribe({
                  next: (data) => {
                        this.budgetPeriods = data;
                  },
                  error: err => alert(handleHttpError(err).join('\n'))
            })
      }

      loadDepartments(): void {
            this.departmentService.getAll().subscribe({
                  next: (data) => {
                        this.departments = data;
                        this.filteredDepartments = data;
                  },
                  error: err => alert(handleHttpError(err).join('\n'))
            })
      }
      onDepartmentInputChange(value: string) {
            const lowerValue = value.toLowerCase();
            this.filteredDepartments = this.departments.filter(dept => 
                  dept.name.toLowerCase().includes(lowerValue) ||
                  dept.code.toLowerCase().includes(lowerValue)
            );
      }
      onDepartmentSelected(deptId: string) {
            this.formData.departmentId = deptId;
      }
      displayDepartmentFn = (deptId: string): string => {
            const dept = this.departments.find(d => d.id === deptId);
            return dept ? `${dept.code} - ${dept.name}` : '';
      }

      create(): void {
            this.budgetPlanService.create(this.formData).subscribe({
                  next: () => {
                        this.loadBudgetPlans();
                        setTimeout(() => this.successMessage = null, 3000);
                  }, 
                  error: err => alert(handleHttpError(err).join('\n'))
            })
      }

      updateMasterCheckboxState(): void {
            const allSelected = this.budgetPlans.every(d => d.selected);
            const noneSelected = this.budgetPlans.every(d => !d.selected);
            const checkbox = this.masterCheckBox?.nativeElement;
            if(checkbox) {
                  checkbox.indeterminate = !allSelected && !noneSelected;
                  checkbox.checked = allSelected;
            }
      }
      isAllSelected(): boolean {
            return this.budgetPlans.length > 0 && this.budgetPlans.every(d => d.selected);
      }


      resetForm() {
            this.formData = {
                  departmentId: '',
                  budgetCodeId: '',
                  budgetPeriodId: '',
                  amount: 0
            };
      }
}