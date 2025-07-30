import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";

@Component({
      selector: 'account-group',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './account-group.component.html',
      styleUrl: './account-group.component.scss',
})
export class AccountGroupComponent {}