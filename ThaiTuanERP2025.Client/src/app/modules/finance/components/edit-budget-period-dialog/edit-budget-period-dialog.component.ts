import { UpdateBudgetPeriodPayload } from './../../models/budget-period.model';
import { CommonModule } from "@angular/common";
import { Component, Inject, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BudgetPeriodDto } from "../../models/budget-period.model";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { ConfirmService } from "../../../../shared/components/confirm-dialog/confirm.service";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";
import { firstValueFrom } from 'rxjs';
import { BudgetPeriodFacade } from '../../facades/budget-period.facade';
import { HttpErrorResponse } from '@angular/common/http';
import { endDateAfterStartDateValidator } from '../../../../shared/validators/date-range.validator';
import { toLocalISOString } from '../../../../shared/utils/date.utils';

@Component({
      selector: 'edit-budget-period-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitSpinnerButtonComponent, MatDatepickerModule],
      templateUrl: './edit-budget-period-dialog.component.html'
})
export class EditBudgetPeriodDialogComponent {
      private readonly dialog = inject(MatDialogRef<EditBudgetPeriodDialogComponent>);
      private readonly formBuilder = inject(FormBuilder);
      private readonly toast = inject(ToastService);
      private readonly confirm = inject(ConfirmService);
      private readonly budgetPeriodFacade = inject(BudgetPeriodFacade);
      budgetPeriodId: string = '';

      showErrors: boolean = false;
      submitting: boolean = false;

      periodTitle: string = '';

      form = this.formBuilder.group(
            {
                  startDate: this.formBuilder.control<Date>(new Date(), { nonNullable: true, validators: [Validators.required] }),
                  endDate: this.formBuilder.control<Date>(new Date(), { nonNullable: true, validators: [Validators.required] }),
            }, 
            { validators: [endDateAfterStartDateValidator()] }
      );

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: BudgetPeriodDto
      ) {
            this.periodTitle = `${data.month}/${data.year}`;
            this.budgetPeriodId = data.id;
            this.form.patchValue({
                  startDate: new Date(data.startDate),
                  endDate: new Date(data.endDate)
            });
      }

      async submit(): Promise<void> {
            this.showErrors = true;
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich("Thông tin không hợp lệ");
                  return;
            }

            try {
                  this.submitting = true;
                  this.form.disable({ emitEvent: false });

                  const { startDate, endDate } = this.form.getRawValue();

                  const payload: UpdateBudgetPeriodPayload = {
                        startDate: toLocalISOString(startDate),
                        endDate: toLocalISOString(endDate)
                  };
                  console.log('payload: ', payload);

                  await firstValueFrom(this.budgetPeriodFacade.update(this.budgetPeriodId, payload));
                  this.toast.successRich("Chỉnh sửa thời hạn thành công");
                  this.dialog.close(true);
            } catch(err) {   
                  if (err instanceof HttpErrorResponse && [401,403,500,0].includes(err.status ?? 0)) {
                        return;
                  }
                  if (err instanceof HttpErrorResponse && err.status === 404) {
                        this.toast?.warningRich('Không tìm thấy dữ liệu để thêm user duyệt ngân sách.');
                  } else if (err instanceof HttpErrorResponse && err.status === 400) {
                        this.toast?.errorRich(err.error?.message || 'Dữ liệu không hợp lệ.');
                  } else {
                        const messages = handleHttpError(err).join('\n');
                        this.confirm.error$(messages);
                        this.toast?.errorRich('Thêm user duyệt ngân sách thất bại.');
                  }
            } finally {
                  this.form.disable({ emitEvent: false });
                  this.submitting = false;
            }
      }

      close(isSuccess: boolean = false) {
            this.dialog.close(isSuccess);
      }
}