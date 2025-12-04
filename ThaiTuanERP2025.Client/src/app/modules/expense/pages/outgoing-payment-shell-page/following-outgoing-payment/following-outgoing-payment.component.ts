import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { OutgoingPaymentDto, OutgoingPaymentLookupDto } from "../../../models/outgoing-payment.model";
import { OutgoingPaymentStatusPipe } from "../../../pipes/outgoing-payment-status.pipe";
import { MatDialog } from "@angular/material/dialog";
import { OutgoingPaymentFacade } from "../../../facades/outgoing-payment.facade";
import { OutgoingPaymentDetailDialogComponent } from "../../../components/dialogs/outgoing-payment-detail-dialog/outgoing-payment-detail-dialog.component";
import { OutgoingPaymentApiService } from "../../../services/api/outgoing-payment.service";
import { firstValueFrom } from "rxjs";

@Component({
      selector: 'following-outgoing-payment',
      standalone: true,
      templateUrl: './following-outgoing-payment.component.html',
      imports: [CommonModule, OutgoingPaymentStatusPipe],
})
export class FollowingOutgoingPaymentComponent implements OnInit {
      private dialog = inject(MatDialog);
      private outgoingPaymentFacade = inject(OutgoingPaymentFacade);
      private readonly outgoingPaymentApi = inject(OutgoingPaymentApiService);
      followingOutgoingPayments: OutgoingPaymentLookupDto[] = []; 

      ngOnInit(): void {
            this.loadFollowingOutgoingPayments();
      }

      private async loadFollowingOutgoingPayments() { 
            this.followingOutgoingPayments = await firstValueFrom(this.outgoingPaymentApi.getFollowing());
            console.log(this.followingOutgoingPayments);
      }

      trackById(index: number, item: OutgoingPaymentDto) { return item.id; }


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