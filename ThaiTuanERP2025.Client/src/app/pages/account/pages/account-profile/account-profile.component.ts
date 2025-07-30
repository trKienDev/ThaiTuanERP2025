import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";

@Component({
      selector: 'account-profile',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './account-profile.component.html',
      styleUrl: './account-profile.component.scss',
})
export class AccountProfileComponent {}