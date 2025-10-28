import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { PermissionRequestDialogComponent } from "../../../dialogs/permission-request-dialog/permission-request-dialog.component";
import { PermissionFacade } from "../../../facades/permission.facade";
import { PermissionDto } from "../../../models/permission.model";

@Component({
      selector: "account-permission-panel",
      templateUrl: './account-permission.component.html',
      standalone: true,
      imports: [ CommonModule ],
})
export class AccountPermissionPanelComponent {
      private matDialog = inject(MatDialog);
      private permissionFacade = inject(PermissionFacade);

      permissions$ = this.permissionFacade.permissions$;    

      trackById(index: number, item: PermissionDto) { return item.id; }

      openPermissionRequestDialog(): void {
            const dialogRef = this.matDialog.open(PermissionRequestDialogComponent, {});
            dialogRef.afterClosed().subscribe();
      }
}