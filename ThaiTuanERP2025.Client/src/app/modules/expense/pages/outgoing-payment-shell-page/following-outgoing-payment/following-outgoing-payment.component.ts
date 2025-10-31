import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { OutgoingPaymentDto } from "../../../models/outgoing-payment.model";
import { OutgoingPaymentStatusPipe } from "../../../pipes/outgoing-payment-status.pipe";
import { MatDialog } from "@angular/material/dialog";
import { OutgoingPaymentFacade } from "../../../facades/outgoing-payment.facade";
import { ExpensePaymentDetailDialogComponent } from "../../../dialogs/expense-payment-detail-dialog/expense-payment-detail-dialog.component";
import { OutgoingPaymentDetailDialogComponent } from "../../../dialogs/outgoing-payment-detail-dialog/outgoing-payment-detail-dialog.component";

@Component({
      selector: 'following-outgoing-payment',
      standalone: true,
      templateUrl: './following-outgoing-payment.component.html',
      imports: [CommonModule, OutgoingPaymentStatusPipe],
})
export class FollowingOutgoingPaymentComponent implements OnInit {
      private dialog = inject(MatDialog);
      private outgoingPaymentFacade = inject(OutgoingPaymentFacade);
      public outgoingPayments$ = this.outgoingPaymentFacade.followingPayments$;

      ngOnInit(): void {
            console.log('following payments: ', this.outgoingPayments$);
      }

      trackById(index: number, item: OutgoingPaymentDto) { return item.id; }

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