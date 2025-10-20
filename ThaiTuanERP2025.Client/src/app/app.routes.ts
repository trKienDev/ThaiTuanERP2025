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
                  { path: 'account',  data: { animation: 'AccountPage' }, loadChildren: () => import('./pages/account/account.routes').then((m) => m.accountRoutes) },
                  { path: 'finance',  data: { animation: 'FinancePage' }, loadChildren: () => import('./pages/finance/finance.route').then((m) => m.financeRoutes )},
                  { path: 'expense',  data: { animation: 'ExpensePage' }, loadChildren: () => import('./pages/expense/expense.route').then((m) => m.expenseRoutes )},
                  { path: 'admin',  data: { animation: 'AdminPage' }, loadComponent: () => import('./pages/admin/admin.component').then(m => m.AdminComponent)},
                  
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
