import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';

@Component({
      selector: 'app-account',
      standalone: true, 
      imports: [ CommonModule, RouterModule ],
      templateUrl: './account.component.html',
      styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {
      isSuperAdmin = false;

      constructor(private readonly auth: AuthService) {}

      ngOnInit(): void {
            this.isSuperAdmin = this.auth.getUserRoles().some(r => r.toLowerCase() === 'superadmin');
      }
}
