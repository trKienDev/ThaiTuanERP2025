import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { OutgoingPaymentDto, OutgoingPaymentSummaryDto } from "../../../models/outgoing-payment.model";
import { OutgoingPaymentService } from "../../../services/outgoing-payment.service";
import { firstValueFrom } from "rxjs";
import { OutgoingPaymentStatusPipe } from "../../../pipes/outgoing-payment-status.pipe";
import { ExpensePaymentDetailDialogComponent } from "../../expense-payment-shell-page/expense-payment-detail/expense-payment-detail-dialog/expense-payment-detail-dialog.component";
import { MatDialog } from "@angular/material/dialog";
import { OutgoingPaymentDetailDialogComponent } from "../outgoing-payment-detail/outgoing-payment-detail-dialog/outgoing-payment-detail-dialog.component";

@Component({
      selector: 'following-outgoing-payment',
      standalone: true,
      templateUrl: './following-outgoing-payment.component.html',
      imports: [CommonModule, OutgoingPaymentStatusPipe],
})
export class FollowingOutgoingPaymentComponent implements OnInit {
      private dialog = inject(MatDialog);
      followingPayments: OutgoingPaymentSummaryDto[] = [];
      constructor(private outgoingPaymentService: OutgoingPaymentService) {}

      async ngOnInit(): Promise<void> {
            await this.loadFollowingOutgoingPayments();
      }

      private async loadFollowingOutgoingPayments() {
            this.followingPayments = await firstValueFrom(this.outgoingPaymentService.getFollowing()) ?? [];
      }

      openExpensePaymentDetailDialog(paymentId: string) {
            const dialogRef = this.dialog.open(ExpensePaymentDetailDialogComponent, {
                  data: paymentId,
            });

            dialogRef.afterClosed().subscribe((result: any) => {
                  if (result?.success) {
                        // Handle success result if needed
                  }     
            });
      }

      openOutgoingPaymentDetailDialog(outgoingPaymentId: string) {
            const dialogRef = this.dialog.open(OutgoingPaymentDetailDialogComponent, {
                  data: outgoingPaymentId,
            });
            
            dialogRef.afterClosed().subscribe((result: any) => {
                  if (result?.success) {
                        // Handle success result if needed
                  }
            });
      }
}