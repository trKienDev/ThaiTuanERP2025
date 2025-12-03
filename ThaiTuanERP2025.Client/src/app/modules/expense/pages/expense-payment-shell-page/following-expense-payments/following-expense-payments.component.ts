// following-expense-payments.component.ts (refactor)
import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ExpensePaymentStatusPipe } from '../../../pipes/expense-payment-status.pipe';
import { AvatarUrlPipe } from '../../../../../shared/pipes/avatar-url.pipe';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ExpensePaymentApiService } from '../../../services/api/expense-payment.service';
import {  ExpensePaymentLookupDto } from '../../../models/expense-payment.model';
import { firstValueFrom } from 'rxjs';
import { ExpensePaymentDetailDialogComponent } from '../../../components/expense-payment-detail-dialog/expense-payment-detail-dialog.component';
import { ConfirmService } from '../../../../../shared/components/confirm-dialog/confirm.service';

@Component({
      selector: 'expense-payments-panel',
      standalone: true,
      templateUrl: './following-expense-payments.component.html',
      styleUrls: ['./following-expense-payments.component.scss'],
      imports: [CommonModule, ExpensePaymentStatusPipe, AvatarUrlPipe ],
})
export class FollowingExpensePaymentsPanelComponent implements OnInit {
      private dialog = inject(MatDialog);
      private route = inject(ActivatedRoute);
      private readonly expensePaymentApi = inject(ExpensePaymentApiService);
      private readonly matDialog = inject(MatDialog);
      private readonly router = inject(Router);
      followingExpensePayments: ExpensePaymentLookupDto[] = [];

      trackById(index: number, item: ExpensePaymentLookupDto) { return item.id; }
      async ngOnInit(): Promise<void> {
            this.followingExpensePayments = await firstValueFrom(this.expensePaymentApi.getFollowing());

            this.listenOpenByQueryParam();
      }

      openExpensePaymentDetailDialog(paymentId: string) {
            this.showExpensePaymentDialog(paymentId);
      }

      private listenOpenByQueryParam(): void {
            this.route.queryParamMap.subscribe(params => {
                  const paymentId = params.get('openExpensePaymentId');

                  if (paymentId) {
                        this.showExpensePaymentDialog(paymentId);
                  }
            });
      }

      private showExpensePaymentDialog(paymentId: string): void {
            const dialogRef = this.matDialog.open(ExpensePaymentDetailDialogComponent, {
                  data: paymentId
            });

            dialogRef.afterClosed().subscribe(async (result) => {
                  // Nếu user đã approve/reject → result === true
                  if (result) {
                        await this.reloadFollowingPayments();
                  }

                  // Dọn query param nếu có
                  this.router.navigate([], {
                  relativeTo: this.route,
                  queryParams: { openExpensePaymentId: null },
                  queryParamsHandling: 'merge'
                  });
            });
      }

      private async reloadFollowingPayments(): Promise<void> {
           this.followingExpensePayments = await firstValueFrom(this.expensePaymentApi.getFollowing());
      }
}