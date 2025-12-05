import { CommonModule } from "@angular/common";
import { Component, inject, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { firstValueFrom } from "rxjs";
import { Kit404PageComponent } from "../../../../../shared/components/kit-404-page/kit-404-page.component";
import { KitLoadingSpinnerComponent } from "../../../../../shared/components/kit-loading-spinner/kit-loading-spinner.component";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { AvatarUrlPipe } from "../../../../../shared/pipes/avatar-url.pipe";
import { useOutgoingPaymentDetail } from "../../../composables/use-outgoing-payment-detail";
import { OutgoingPaymentDetailDto } from "../../../models/outgoing-payment.model";
import { OutgoingPaymentStatusPipe } from "../../../pipes/outgoing-payment-status.pipe";
import { OutgoingPaymentApiService } from "../../../services/api/outgoing-payment.service";
import { ExpensePaymentItemsTableComponent } from "../../tables/expense-payment-items-table/expense-payment-items-table.component";
import { HttpErrorHandlerService } from "../../../../../core/services/http-errror-handler.service";
import { trigger, transition, style, animate } from "@angular/animations";
import { OutgoingPaymentsTableComponent } from "../../tables/outgoing-payments-table/outgoing-payments-table.component";
import { ExpensePaymentApiService } from "../../../services/api/expense-payment.service";
import { ExpensePaymentDetailDto } from "../../../models/expense-payment.model";

@Component({
      selector: 'outgoing-payment-detail-dialog',
      templateUrl: './outgoing-payment-detail-dialog.component.html',
      styleUrls: ['./outgoing-payment-detail-dialog.component.scss'],
      standalone: true,
      imports: [CommonModule, OutgoingPaymentStatusPipe, AvatarUrlPipe, KitLoadingSpinnerComponent, Kit404PageComponent, ExpensePaymentItemsTableComponent, OutgoingPaymentsTableComponent],
})
export class OutgoingPaymentDetailDialogComponent {
      private readonly dialogRef = inject(MatDialogRef<OutgoingPaymentDetailDialogComponent>);
      private readonly toastService = inject(ToastService);
      private readonly outgoingPaymentService = inject(OutgoingPaymentApiService);
      private readonly expensePaymentApi = inject(ExpensePaymentApiService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);

      private readonly outgoingPaymentLogic = useOutgoingPaymentDetail();

      expensePaymentDetail: ExpensePaymentDetailDto | null = null;
      loading = this.outgoingPaymentLogic.isLoading;
      error = this.outgoingPaymentLogic.error;
      submitting = false;

      constructor(@Inject(MAT_DIALOG_DATA) private data: string) {
            if(data) {
                  this.outgoingPaymentLogic.load(data);
            }
      }

      get outgoingPaymentDetail(): OutgoingPaymentDetailDto | null { 
            const detail = this.outgoingPaymentLogic.outgoingPaymentDetail();
            const expensePaymentId = detail?.expensePaymentId;
            if(expensePaymentId) this.getExpensePaymentDetail(expensePaymentId);
                  
            return detail;
      }
      
      async getExpensePaymentDetail(expensePaymentId: string): Promise<void> {
            if(expensePaymentId) {
                  this.expensePaymentDetail = await firstValueFrom(this.expensePaymentApi.getDetailById(expensePaymentId));
            } 
      }

      // === TAB NAVIGATION ===     
      activeTab: 'items' | 'outgoings' = 'items';
      setActiveTab(tab: 'items' | 'outgoings') {
            this.activeTab = tab;
      }

      async onApprove(): Promise<void> {
            this.submitting = true;
            try {
                  await firstValueFrom(this.outgoingPaymentService.approve(this.outgoingPaymentDetail!.id));
                  this.toastService.successRich("Duyệt khoản chi thành công");
                  this.outgoingPaymentLogic.refresh();

                  this.dialogRef.close(true);
            } catch (error) {
                  this.httpErrorHandler.handle(error, "Duyệt khoản chi thất bại");
            } finally {
                  this.submitting = false;
            }
      }

      async markCreated(): Promise<void> {
            this.submitting = true;
            try {
                  await firstValueFrom(this.outgoingPaymentService.markCreated(this.outgoingPaymentDetail!.id));
                  this.toastService.successRich("Đánh dấu khoản chi đã tạo thành công");
                  this.outgoingPaymentLogic.refresh();
                  this.dialogRef.close(true);
            } catch (error) {
                  console.error('Error marking outgoing payment as created', error);
                  this.httpErrorHandler.handle(error, "Tạo lệnh khoản chi thất bại");
            } finally {
                  this.submitting = false;
            }
      }

      close(isSuccess: boolean = false): void {
            this.dialogRef.close(isSuccess);
      }
}     