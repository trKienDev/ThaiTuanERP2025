import { Component, ElementRef, inject, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ExpensePaymentDetailDto } from "../../models/expense-payment.model";
import { firstValueFrom } from "rxjs";
import { ExpensePaymentService } from "../../services/expense-payment.service";
import { ExpensePaymentStatusPipe } from "../../pipes/expense-payment-status.pipe";
import { CommonModule } from "@angular/common";
import { environment } from "../../../../../environments/environment";
import { AvatarUrlPipe } from "../../../../shared/pipes/avatar-url.pipe";
import { UserFacade } from "../../../account/facades/user.facade";
import { FormsModule } from "@angular/forms";
import { AutoResizeDirective } from "../../../../shared/directives/money/textarea/textarea-auto-resize.directive";
import { TextareaNoSpellcheckDirective } from "../../../../shared/directives/money/textarea/textarea-no-spellcheck.directive";

@Component({
      selector: 'expense-payment-detail',      
      standalone: true,
      templateUrl: './expense-payment-detail.component.html',
      styleUrls: ['./expense-payment-detail.component.scss'],
      imports: [CommonModule, FormsModule, ExpensePaymentStatusPipe, AvatarUrlPipe, AutoResizeDirective, TextareaNoSpellcheckDirective],
})
export class ExpensePaymentDetailComponent implements OnInit {
      private route = inject(ActivatedRoute);
      private expensePaymentService = inject(ExpensePaymentService);
      private userFacade = inject(UserFacade);
      currentUser$ = this.userFacade.currentUser$;

      paymentId: string = '';
      paymentDetail: ExpensePaymentDetailDto | null = null;
      baseUrl: string = environment.baseUrl;     
      
      ngOnInit(): void {
            this.paymentId = this.route.snapshot.paramMap.get('id')!;
            this.getPaymentDetails();
      }

      async getPaymentDetails() {
            this.paymentDetail = await firstValueFrom(this.expensePaymentService.getDetailById(this.paymentId));
            console.log('payment detail', this.paymentDetail);
      }

      // comment
      isCommenting: boolean = false;
      commentText: string = '';
      startCommenting() {
            this.isCommenting = true;
      }
      cancelComment() {
            this.commentText = '';
            this.isCommenting = false;
      }
      submitComment() {
            const content = this.commentText.trim();
            if(!content) return;
            // call api submit comment here
            console.log('submit comment', content);
            this.commentText = '';
            this.isCommenting = false;
      }
}