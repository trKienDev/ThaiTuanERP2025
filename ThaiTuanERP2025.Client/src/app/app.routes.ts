import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.page';
import { HomeComponent } from './pages/home/home.page';
import { LayoutShellComponent } from './layout/layout-shell/layout-shell.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
      {
            path: '',
            component: LayoutShellComponent,
            canActivate: [authGuard],
            children:  [
                  { path: '', redirectTo: 'home', pathMatch: 'full' },
                  { path: 'home', component: HomeComponent }
            ]
      }, 
      { path: 'login', component: LoginComponent }, 
      { 
            path: 'splash', 
            loadComponent: () => import('./pages/splash-screen/splash-screen.component').then(m => m.SplashScreenComponent),
            data: { animation: 'SplashScreen' }
      }
];
