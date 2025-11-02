import { Routes } from '@angular/router';
import { LayoutShellComponent } from './layouts/layout-shell/layout-shell.component';
import { HomeComponent } from './layouts/home/home.page';
import { LoginComponent } from './layouts/login/login.page';
import { AuthGuard } from './core/auth/auth.guard';

export const routes: Routes = [
      {
            path: '',
            component: LayoutShellComponent,
            canActivate: [AuthGuard],
            children:  [
                  { path: '', redirectTo: 'home', pathMatch: 'full' },
                  { path: 'account',  data: { animation: 'AccountPage' }, loadChildren: () => import('./modules/account/account.routes').then((m) => m.accountRoutes) },
                  { path: 'finance',  data: { animation: 'FinancePage' }, loadChildren: () => import('./modules/finance/finance.route').then((m) => m.financeRoutes )},
                  { path: 'expense',  data: { animation: 'ExpensePage' }, loadChildren: () => import('./modules/expense/expense.route').then((m) => m.expenseRoutes )},
                  { path: 'admin',  data: { animation: 'AdminPage' }, loadComponent: () => import('./modules/admin/admin.component').then(m => m.AdminComponent)},
                  
                  { path: 'home', component: HomeComponent }
            ]
      }, 
      { path: 'login', component: LoginComponent }, 
      { 
            path: 'splash', 
            loadComponent: () => import('./layouts/splash-screen/splash-screen.component').then(m => m.SplashScreenComponent),
            data: { animation: 'SplashScreen' }
      }
];
