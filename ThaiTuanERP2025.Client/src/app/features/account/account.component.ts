import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth/auth.service';

@Component({
      selector: 'app-account',
      standalone: true, 
      imports: [ CommonModule, RouterModule ],
      templateUrl: './account.component.html',
      styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {
      isAdmin = false;

      constructor(private auth: AuthService) {}

      ngOnInit(): void {
            this.isAdmin = this.auth.getUserRole() === 'admin';
      }
}
