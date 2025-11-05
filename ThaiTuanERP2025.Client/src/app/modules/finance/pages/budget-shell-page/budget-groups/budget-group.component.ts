import { CommonModule } from "@angular/common";
import { Component,  inject,  OnInit } from "@angular/core";
import { BudgetGroupDto, BudgetGroupRequest } from "../../../models/budget-group.model";
import { BudgetGroupService } from "../../../services/budget-group.service";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { HasPermissionDirective } from "../../../../../shared/directives/has-permission.directive";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { firstValueFrom } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { KitSpinnerButtonComponent } from "../../../../../shared/components/kit-spinner-button/kit-spinner-button.component";

@Component({
      selector: 'budget-groups-panel',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, HasPermissionDirective, KitSpinnerButtonComponent ],
      templateUrl: './budget-group.component.html'
})
export class BudgetGroupPanelComponent implements OnInit {
      formBuilder = inject(FormBuilder);
      toast = inject(ToastService);
      budgetGroups: BudgetGroupDto[] = [];
      public submitting = false;
           
      constructor(private readonly budgetGroupService: BudgetGroupService) {
            console.log('%c[BudgetGroupPanelComponent constructed]', 'color: green');
      }

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            code: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}), 
      });

      ngOnInit(): void {
            this.loadBudgetGroups();
      }

      loadBudgetGroups(): void {
            this.budgetGroupService.getAll().subscribe({
                  next: (data) => {
                        this.budgetGroups = data.map(bg => ({ ...bg, selected: false }));
                  },
                  error: err => {
                        const error = handleHttpError(err);
                        console.error('Error loading budget groups: ', error);
                  }
            });
      }

      async createBudgetGroup() {
            if(this.form.invalid) {
                  this.toast.warningRich('Vui lòng điền đẩy đủ thông tin');
                  return;     
            }

            this.submitting = true;
            try {
                  const payload = this.form.getRawValue() as BudgetGroupRequest;
                  await firstValueFrom(this.budgetGroupService.create(payload));
                  this.toast.successRich('Tạo nhóm ngân sách thành công');
            } catch(err) {   
                  if (err instanceof HttpErrorResponse && [401,403,500,0].includes(err.status ?? 0)) {
                        return;
                  }
                  if (err instanceof HttpErrorResponse && err.status === 404) {
                        this.toast?.warningRich('Không tìm thấy dữ liệu để tạo nhóm ngân sách.');
                  } else if (err instanceof HttpErrorResponse && err.status === 400) {
                        this.toast?.errorRich(err.error?.message || 'Dữ liệu không hợp lệ.');
                  } else {
                        this.toast?.errorRich('Tạo nhóm ngân sách thất bại.');
                  }
                  console.error('[createBudgetGroup]:', err);
            }
      }
}