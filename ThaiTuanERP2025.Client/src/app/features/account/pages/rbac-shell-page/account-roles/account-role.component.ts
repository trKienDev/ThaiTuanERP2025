import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { RoleRequestDialogComponent } from "../../../dialogs/role-request-dialog/role-request-dialog.component";
import { RoleFacade } from "../../../facades/role.facade";
import { RoleDto } from "../../../models/role.model";
import { ActionMenuOption } from "../../../../../shared/components/kit-action-menu/kit-action-menu.model";
import { AssignPermissionToRoleDialogComponent } from "../../../dialogs/assign-permission-to-role-dialog/assign-permission-to-role-dialog.component";
import { KitActionMenuComponent } from "../../../../../shared/components/kit-action-menu/kit-action-menu.component";

@Component({
      selector: "account-role-panel",
      standalone: true,
      templateUrl: "./account-role.component.html",
      imports: [CommonModule, KitActionMenuComponent],
})
export class AccountRolePanelComponent {
      private matDialog = inject(MatDialog);
      private roleFacade = inject(RoleFacade);
      public roles$ = this.roleFacade.roles$;

      trackById(index: number, item: RoleDto) { return item.id; }

      openRoleRequestDialog() {
            const dialogRef = this.matDialog.open(RoleRequestDialogComponent, {
                  width: 'fit-content',
            });

            dialogRef.afterClosed().subscribe((result: { success?: boolean;} | undefined ) => {
                  if(result && result.success) {

                  }
            });
      }

      buildRoleActions(role: RoleDto) : ActionMenuOption[] {
            return [
                  { label: '⚙️ Gán quyền', action: () => this.assignPermissionsToRole(role) },
            ];
      }
      assignPermissionsToRole(role: RoleDto): void {
            const dialogRef = this.matDialog.open(AssignPermissionToRoleDialogComponent, {
                  data: role
            });
            dialogRef.afterClosed().subscribe();
      }

}     