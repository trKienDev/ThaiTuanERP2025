import { Routes } from "@angular/router";
import { AccountComponent } from "./account.component";
import { adminGuard } from "../../core/guards/admin.guard";

export const accountRoutes: Routes = [
      {
            path: '',
            component: AccountComponent,
            children: [
                  { path: '', redirectTo: 'profile', pathMatch: 'full' },
                  { path: 'profile', loadComponent: () => import('./pages/account-profile/account-profile.component').then((m) => m.AccountProfileComponent)},
                  { path: 'setting', loadComponent: () => import('./pages/account-setting/account-setting.component').then((m) => m.AccountSettingComponent)},
                  { path: 'group', loadComponent: () => import('./pages/account-group/account-group.component').then((m) => m.AccountGroupComponent)},
                  { 
                        path: 'members', 
                        canActivate: [adminGuard], 
                        loadComponent: () => import('./pages/account-member/account-member.component').then((m) => m.AccountMemberComponent)
                  },
                  {
                        path: 'departments', 
                        canActivate: [adminGuard], 
                        loadComponent: () => import('./pages/account-department/account-department.component').then((m) => m.AccountDepartmentComponent )
                  }
            ],
      },
];

