import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { RoleRequestDialogComponent } from "../../../dialogs/role-request-dialog/role-request-dialog.component";
import { RoleFacade } from "../../../facades/role.facade";
import { RoleDto } from "../../../models/role.model";

@Component({
      selector: "account-role-panel",
      standalone: true,
      templateUrl: "./account-role.component.html",
      imports: [ CommonModule ],
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
}