import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
      selector: 'app-splash-screen',
      standalone: true,
      templateUrl: './splash-screen.component.html',
      styleUrl: './splash-screen.component.scss'
})
export class SplashScreenComponent {
      constructor(private router: Router) {
            setTimeout(() => this.router.navigateByUrl('/home'), 1500);
      }
}

