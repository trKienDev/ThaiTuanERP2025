import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { AccountRolePanelComponent } from "./account-roles/account-role.component";
import { KitShellTabsComponent } from "../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component";

@Component({
      selector: 'account-permission',
      standalone: true,
      imports: [CommonModule, KitShellTabsComponent],
      templateUrl: './rbac-shell-page.component.html'
})
export class RBACShellPageComponent {
      readonly tabs = [
            { id: 'roles', label: 'Vai trò', component: AccountRolePanelComponent },
            { id: 'permissions', label: 'Quyền', component: AccountRolePanelComponent },
      ]
}