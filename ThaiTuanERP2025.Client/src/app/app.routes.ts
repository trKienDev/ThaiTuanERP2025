import { Routes } from '@angular/router';
import { LayoutShellComponent } from './layouts/layout-shell/layout-shell.component';
import { LoginComponent } from './layouts/login/login.page';
import { AuthGuard } from './core/auth/auth.guard';

export const routes: Routes = [
      {
            path: '',
            component: LayoutShellComponent,
            canActivate: [AuthGuard],
            children:  [
                  { path: '', redirectTo: 'account', pathMatch: 'full' },
                  { path: 'account', loadChildren: () => import('./modules/account/account.routes').then((m) => m.accountRoutes) },
                  { path: 'finance', loadChildren: () => import('./modules/finance/finance.route').then((m) => m.financeRoutes )},
                  { path: 'expense', loadChildren: () => import('./modules/expense/expense.route').then((m) => m.expenseRoutes )},
                  { path: 'admin', loadComponent: () => import('./modules/admin/admin.component').then(m => m.AdminComponent)},
            ]
      }, 
      { path: 'login', component: LoginComponent }, 
      { 
            path: 'splash', 
            loadComponent: () => import('./layouts/splash-screen/splash-screen.component').then(m => m.SplashScreenComponent),
            data: { animation: 'SplashScreen' }
      }
];
