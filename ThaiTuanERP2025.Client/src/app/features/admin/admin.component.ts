import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";

@Component({
      selector: 'app-admin',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './admin.component.html',
      styleUrls: ['./admin.component.scss']
})
export class AdminComponent {}