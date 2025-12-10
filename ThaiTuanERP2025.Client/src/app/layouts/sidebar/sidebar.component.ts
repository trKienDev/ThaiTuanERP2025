import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { KitDebitCardIconComponent } from "../../shared/icons/kit-debit-card.component";
import { KitUserIconComponent } from "../../shared/icons/kit-user-icon.component";
import { KitMoneyBagIconComponent } from "../../shared/icons/kit-money-bag.component";

@Component({
      selector: 'app-sidebar',
      standalone: true,
      imports: [CommonModule, RouterModule, KitDebitCardIconComponent, KitUserIconComponent, KitMoneyBagIconComponent],
      templateUrl: './sidebar.component.html',
      styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
      
}
