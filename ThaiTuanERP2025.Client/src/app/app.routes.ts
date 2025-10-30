import { Routes } from '@angular/router';
import { LayoutShellComponent } from './layout/layout-shell/layout-shell.component';
import { authGuard } from './core/guards/auth.guard';
import { HomeComponent } from './features/home/home.page';
import { LoginComponent } from './features/login/login.page';

export const routes: Routes = [
      {
            path: '',
            component: LayoutShellComponent,
            canActivate: [authGuard],
            children:  [
                  { path: '', redirectTo: 'home', pathMatch: 'full' },
                  { path: 'account',  data: { animation: 'AccountPage' }, loadChildren: () => import('./features/account/account.routes').then((m) => m.accountRoutes) },
                  { path: 'finance',  data: { animation: 'FinancePage' }, loadChildren: () => import('./features/finance/finance.route').then((m) => m.financeRoutes )},
                  { path: 'expense',  data: { animation: 'ExpensePage' }, loadChildren: () => import('./features/expense/expense.route').then((m) => m.expenseRoutes )},
                  { path: 'admin',  data: { animation: 'AdminPage' }, loadComponent: () => import('./features/admin/admin.component').then(m => m.AdminComponent)},
                  
                  { path: 'home', component: HomeComponent }
            ]
      }, 
      { path: 'login', component: LoginComponent }, 
      { 
            path: 'splash', 
            loadComponent: () => import('./features/splash-screen/splash-screen.component').then(m => m.SplashScreenComponent),
            data: { animation: 'SplashScreen' }
      }
];
