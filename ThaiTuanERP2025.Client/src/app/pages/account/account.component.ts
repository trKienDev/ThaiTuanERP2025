import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { RouterModule } from '@angular/router';

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
