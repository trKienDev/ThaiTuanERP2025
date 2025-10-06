import { Component, inject, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ExpensePaymentDetailDto } from "../../models/expense-payment.model";
import { firstValueFrom } from "rxjs";
import { ExpensePaymentService } from "../../services/expense-payment.service";
import { ExpensePaymentStatusPipe } from "../../pipes/expense-payment-status.pipe";
import { CommonModule } from "@angular/common";

@Component({
      selector: 'expense-payment-detail',      
      standalone: true,
      templateUrl: './expense-payment-detail.component.html',
      styleUrls: ['./expense-payment-detail.component.scss'],
      imports: [ CommonModule, ExpensePaymentStatusPipe],
})
export class ExpensePaymentDetailComponent implements OnInit {
      private route = inject(ActivatedRoute);
      private expensePaymentService = inject(ExpensePaymentService);
      paymentId: string = '';
      paymentDetail: ExpensePaymentDetailDto | null = null;
      
      ngOnInit(): void {
            this.paymentId = this.route.snapshot.paramMap.get('id')!;
            this.getPaymentDetails();
      }

      async getPaymentDetails() {
            this.paymentDetail = await firstValueFrom(this.expensePaymentService.getDetailById(this.paymentId));
            console.log('payment detail', this.paymentDetail);
      }
}