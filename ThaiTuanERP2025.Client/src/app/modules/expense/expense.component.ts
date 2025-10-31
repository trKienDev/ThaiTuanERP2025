import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../core/services/auth/auth.service";
import { Router, RouterModule } from "@angular/router";
import { loadDomAnimations } from "../../shared/animations/load-dom/load-dom.animation";

@Component({
      selector: 'app-expense',
      standalone: true,
      imports: [ CommonModule, RouterModule ],
      templateUrl: './expense.component.html',
      styleUrl: './expense.component.scss',
      animations: [loadDomAnimations]
})
export class ExpenseComponent implements OnInit {
      isAdmin = false;
      
      constructor(private auth: AuthService, private router: Router) {}

      ngOnInit(): void {
            this.isAdmin = this.auth.getUserRoles().some(r => r.toLowerCase() === 'superadmin');
      }

      get isPaymentDetailPage(): boolean {
            return this.router.url.startsWith("/expense/payment-detail");
      }
}