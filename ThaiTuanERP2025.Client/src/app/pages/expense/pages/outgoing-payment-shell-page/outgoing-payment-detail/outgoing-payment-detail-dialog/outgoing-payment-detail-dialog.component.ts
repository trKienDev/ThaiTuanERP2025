import { CommonModule } from "@angular/common";
import { Component, Inject, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { useOutgoingPaymentDetail } from "../../../../composables/use-outgoing-payment-detail";
import { OutgoingPaymentDetailDto } from "../../../../models/outgoing-payment.model";
import { OutgoingPaymentStatusPipe } from "../../../../pipes/outgoing-payment-status.pipe";
import { AvatarUrlPipe } from "../../../../../../shared/pipes/avatar-url.pipe";

@Component({
      selector: 'outgoing-payment-detail-dialog',
      templateUrl: './outgoing-payment-detail-dialog.component.html',
      styleUrls: ['./outgoing-payment-detail-dialog.component.scss'],
      standalone: true,
      imports: [CommonModule, OutgoingPaymentStatusPipe, AvatarUrlPipe]
})
export class OutgoingPaymentDetailDialogComponent {
      private dialogRef = inject(MatDialogRef<OutgoingPaymentDetailDialogComponent>);
      private outgoingPaymentLogic = useOutgoingPaymentDetail();
      loading = this.outgoingPaymentLogic.isLoading;
      error = this.outgoingPaymentLogic.error;

      constructor(@Inject(MAT_DIALOG_DATA) private data: string) {
            if(data) {
                  console.log('Loading outgoing payment detail for id', data);
                  this.outgoingPaymentLogic.load(data);
            }
      }

      get outgoingPaymentDetail(): OutgoingPaymentDetailDto | null { 
            console.log('outgoingPaymentDetail', this.outgoingPaymentLogic.outgoingPaymentDetail());
            return this.outgoingPaymentLogic.outgoingPaymentDetail();
      }

      close(): void {
            this.dialogRef.close();
      }
}     