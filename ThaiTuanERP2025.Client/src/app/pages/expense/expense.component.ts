import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../core/services/auth/auth.service";
import { RouterModule } from "@angular/router";

@Component({
      selector: 'app-expense',
      standalone: true,
      imports: [ CommonModule, RouterModule ],
      templateUrl: './expense.component.html',
      styleUrl: './expense.component.scss',
})
export class ExpenseComponent implements OnInit {
      isAdmin = false;
      constructor(private auth: AuthService) {}

      ngOnInit(): void {
            this.isAdmin = this.auth.getUserRole() === 'admin';
      }
}