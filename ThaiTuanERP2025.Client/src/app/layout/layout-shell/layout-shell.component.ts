import { Component } from '@angular/core';
import { TopbarComponent } from '../topbar/topbar.component';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';

@Component({
      selector: 'app-layout-shell',
      standalone: true,
      imports: [CommonModule, RouterOutlet, SidebarComponent, TopbarComponent],
      templateUrl: './layout-shell.component.html',
      styleUrl: './layout-shell.component.scss'
})
export class LayoutShellComponent {
      
}
