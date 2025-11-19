import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { KiBudgetIconComponent } from "../../shared/icons/kit-budget-icon.component";
import { KitDebitCardIconComponent } from "../../shared/icons/kit-debit-card.component";
import { KitUserIconComponent } from "../../shared/icons/kit-user-icon.component";

@Component({
      selector: 'app-sidebar',
      standalone: true,
      imports: [CommonModule, RouterModule, KiBudgetIconComponent, KitDebitCardIconComponent, KitUserIconComponent],
      templateUrl: './sidebar.component.html',
      styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
      
}
