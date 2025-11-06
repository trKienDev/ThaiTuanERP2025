import { CommonModule } from "@angular/common";
import { Component,  inject, OnDestroy, OnInit } from "@angular/core";
import { BudgetGroupDto, BudgetGroupRequest } from "../../../models/budget-group.model";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { firstValueFrom, Subject, takeUntil } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { KitSpinnerButtonComponent } from "../../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { HasPermissionDirective } from "../../../../../core/auth/auth.directive";
import { BudgetGroupFacade } from "../../../facades/budget-group.facade";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { ConfirmService } from "../../../../../shared/components/confirm-dialog/confirm.service";

@Component({
      selector: 'budget-groups-panel',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitSpinnerButtonComponent, HasPermissionDirective],
      templateUrl: './budget-group.component.html'
})
export class BudgetGroupPanelComponent implements OnInit, OnDestroy {
      formBuilder = inject(FormBuilder);
      toast = inject(ToastService);
      confirmService = inject(ConfirmService);
      private readonly budgetGroupFacade = inject(BudgetGroupFacade);
      budgetGroups$ = this.budgetGroupFacade.budgetGroups$;

      public submitting = false;
      showErrors = false; 

      private readonly destroy$ = new Subject<void>();

      ngOnInit() {
      // Mỗi khi người dùng thay đổi form → ẩn lỗi
            this.form.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(() => {
                  this.showErrors = false;
            });
      }
      ngOnDestroy() {
            this.destroy$.next();
            this.destroy$.complete();
      }

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            code: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}), 
      });

      trackById(index: number, item: BudgetGroupDto) { return item.id; }

      async createBudgetGroup() {
            this.showErrors = true;            
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich('Vui lòng điền đẩy đủ thông tin');
                  return;     
            }
            
            try {
                  this.submitting = true;
                  const payload = this.form.getRawValue() as BudgetGroupRequest;
                  await firstValueFrom(this.budgetGroupFacade.create(payload));
                  this.toast.successRich('Tạo nhóm ngân sách thành công');
                  this.form.reset();  
                  this.showErrors = false;  
            } catch(err) {   
                  if (err instanceof HttpErrorResponse && [401,403,500,0].includes(err.status ?? 0)) {
                        return;
                  }
                  if (err instanceof HttpErrorResponse && err.status === 404) {
                        this.toast?.warningRich('Không tìm thấy dữ liệu để tạo nhóm ngân sách.');
                  } else if (err instanceof HttpErrorResponse && err.status === 400) {
                        this.toast?.errorRich(err.error?.message || 'Dữ liệu không hợp lệ.');
                  } else {
                        const messages = handleHttpError(err).join('\n');
                        this.confirmService.error$(messages);
                        this.toast?.errorRich('Tạo nhóm ngân sách thất bại.');
                  }
            } finally {
                  this.submitting = false;
            }
      }
}