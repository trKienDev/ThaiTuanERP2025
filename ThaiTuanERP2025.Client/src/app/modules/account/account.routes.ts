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
                        loadComponent: () => import('./pages/account-member/account-member.component').then((m) => m.AccountMemberComponent)
                  },
                  {
                        path: 'departments', 
                        loadComponent: () => import('./pages/account-department/account-department.component').then((m) => m.AccountDepartmentComponent )
                  }, 
                  { 
                        path: 'rbac',
                        canActivate: [ adminGuard ],
                        loadComponent: () => import('./pages/rbac-shell-page/rbac-shell-page.component').then((m) => m.RBACShellPageComponent ),
                        children: [
                              { path: 'roles', loadComponent: () => import('./pages/rbac-shell-page/account-roles/account-role.component').then((m) => m.AccountRolePanelComponent) },
                              { path: 'permissions', loadComponent: () => import('./pages/rbac-shell-page/account-permissions/account-permission.component').then((m) => m.AccountPermissionPanelComponent) },
                        ]
                  }
            ],
      },
];

