// following-expense-payments.component.ts (refactor)
import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ExpensePaymentStatusPipe } from '../../../pipes/expense-payment-status.pipe';
import { AvatarUrlPipe } from '../../../../../shared/pipes/avatar-url.pipe';
import { MatDialog } from '@angular/material/dialog';
import { KitLoadingSpinnerComponent } from "../../../../../shared/components/kit-loading-spinner/kit-loading-spinner.component";
import { KitRefreshButtonComponent } from "../../../../../shared/components/kit-refresh-button/kit-refresh-button.component";
import { ActivatedRoute } from '@angular/router';
import { ExpensePaymentApiService } from '../../../services/api/expense-payment.service';
import {  ExpensePaymentLookupDto } from '../../../models/expense-payment.model';
import { firstValueFrom } from 'rxjs';
import { ExpensePaymentDetailDialogComponent } from '../../../components/expense-payment-detail-dialog/expense-payment-detail-dialog.component';

@Component({
      selector: 'expense-payments-panel',
      standalone: true,
      templateUrl: './following-expense-payments.component.html',
      styleUrls: ['./following-expense-payments.component.scss'],
      imports: [CommonModule, ExpensePaymentStatusPipe, AvatarUrlPipe, KitLoadingSpinnerComponent, KitRefreshButtonComponent],
})
export class FollowingExpensePaymentsPanelComponent {
      private dialog = inject(MatDialog);
      private route = inject(ActivatedRoute);
      private readonly expensePaymentApi = inject(ExpensePaymentApiService);
      followingExpensePayments: ExpensePaymentLookupDto[] = [];


      async ngOnInit(): Promise<void> {
            this.followingExpensePayments = await firstValueFrom(this.expensePaymentApi.getFollowing());
      }

      openExpensePaymentDetailDialog(paymentId: string) {
            this.dialog.open(ExpensePaymentDetailDialogComponent, {
                  data: paymentId
            });
      }

}
