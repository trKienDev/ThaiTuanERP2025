import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AuthService } from "../../core/services/auth/auth.service";

@Component({
      selector: 'app-finance',
      standalone: true,
      imports: [ CommonModule, RouterModule ],
      templateUrl: './finance.component.html',
      styleUrl: './finance.component.scss'
})
export class FinanceComponent implements OnInit {
      isAdmin = false;

      constructor(private auth: AuthService) {}

      ngOnInit(): void {
            this.isAdmin = this.auth.getUserRoles().some(r => r.toLowerCase() === 'superadmin');
      }
}