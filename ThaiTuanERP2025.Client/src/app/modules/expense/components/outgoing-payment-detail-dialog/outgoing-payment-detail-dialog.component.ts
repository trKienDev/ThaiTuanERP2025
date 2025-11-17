import { CommonModule } from "@angular/common";
import { Component, inject, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { firstValueFrom } from "rxjs";
import { Kit404PageComponent } from "../../../../shared/components/kit-404-page/kit-404-page.component";
import { KitLoadingSpinnerComponent } from "../../../../shared/components/kit-loading-spinner/kit-loading-spinner.component";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { AvatarUrlPipe } from "../../../../shared/pipes/avatar-url.pipe";
import { useOutgoingPaymentDetail } from "../../composables/use-outgoing-payment-detail";
import { OutgoingPaymentDetailDto } from "../../models/outgoing-payment.model";
import { OutgoingPaymentStatusPipe } from "../../pipes/outgoing-payment-status.pipe";
import { OutgoingPaymentApiService } from "../../services/outgoing-payment.service";

@Component({
      selector: 'outgoing-payment-detail-dialog',
      templateUrl: './outgoing-payment-detail-dialog.component.html',
      styleUrls: ['./outgoing-payment-detail-dialog.component.scss'],
      standalone: true,
      imports: [CommonModule, OutgoingPaymentStatusPipe, AvatarUrlPipe, KitLoadingSpinnerComponent, Kit404PageComponent]
})
export class OutgoingPaymentDetailDialogComponent {
      private dialogRef = inject(MatDialogRef<OutgoingPaymentDetailDialogComponent>);
      private outgoingPaymentLogic = useOutgoingPaymentDetail();
      private toastService = inject(ToastService);
      private outgoingPaymentService = inject(OutgoingPaymentApiService);
      loading = this.outgoingPaymentLogic.isLoading;
      error = this.outgoingPaymentLogic.error;
      processing = false;

      constructor(@Inject(MAT_DIALOG_DATA) private data: string) {
            if(data) {
                  this.outgoingPaymentLogic.load(data);
            }
      }

      get outgoingPaymentDetail(): OutgoingPaymentDetailDto | null { 
            return this.outgoingPaymentLogic.outgoingPaymentDetail();
      }

      async onApprove(): Promise<void> {
            this.processing = true;
            try {
                  await firstValueFrom(this.outgoingPaymentService.onApprove(this.outgoingPaymentDetail!.id));
                  this.toastService.successRich("Duyệt khoản chi thành công");
                  this.outgoingPaymentLogic.refresh();
            } catch (error) {
                  console.error('Error approving outgoing payment', error);
                  this.toastService.errorRich("Không thể duyệt khoản chi");
            } finally {
                  this.processing = false;
            }
      }

      async markCreated(): Promise<void> {
            this.processing = true;
            try {
                  await firstValueFrom(this.outgoingPaymentService.markCreated(this.outgoingPaymentDetail!.id));
                  this.toastService.successRich("Đánh dấu khoản chi đã tạo thành công");
                  this.outgoingPaymentLogic.refresh();
            } catch (error) {
                  console.error('Error marking outgoing payment as created', error);
                  this.toastService.errorRich("Không thể đánh dấu khoản chi đã tạo");
            } finally {
                  this.processing = false;
            }
      }

      close(): void {
            this.dialogRef.close();
      }
}     