import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialog  } from "@angular/material/dialog";
import { RoleFacade } from "../../../facades/role.facade";
import { RoleDto } from "../../../models/role.model";
import { ActionMenuOption } from "../../../../../shared/components/kit-action-menu/kit-action-menu.model";
import { KitActionMenuComponent } from "../../../../../shared/components/kit-action-menu/kit-action-menu.component";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { firstValueFrom } from "rxjs";
import { ConfirmService } from "../../../../../shared/components/confirm-dialog/confirm.service";
import { AssignPermissionToRoleDialogComponent } from "../../../components/assign-permission-to-role-dialog/assign-permission-to-role-dialog.component";
import { RoleRequestDialogComponent } from "../../../components/role-request-dialog/role-request-dialog.component";

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
      private errorMessages: string[] = [];     
      private toast = inject(ToastService);
      constructor(
            private confirmService: ConfirmService
      ) {}

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
                  { label: '🔑 Phân quyền', action: () => this.assignPermissionsToRole(role) },
                  { label: '🗑️ Xóa vai trò', color: 'red' , action: () => this.delete(role.id) },
            ];
      }
      assignPermissionsToRole(role: RoleDto): void {
            const dialogRef = this.matDialog.open(AssignPermissionToRoleDialogComponent, {
                  data: role
            });
            dialogRef.afterClosed().subscribe();
      }

      async delete(roleId: string): Promise<void> {
            this.confirmService.warn$('Bạn có chắc chắn muốn xóa role này không?')
                  .subscribe(async confirmed => {
                        if (confirmed) {
                              try {
                                    await firstValueFrom(this.roleFacade.delete(roleId));
                                    this.toast.successRich('Xóa role thành công');
                              } catch (error) {
                                    this.toast.errorRich('Xóa role thất bại');
                                    const messages = handleHttpError(error);
                                    console.error('Error deleting role:', messages);
                              }
                        }
                  });
      }

      async onToggleActive(role: RoleDto): Promise<void> {
            const oldValue = role.isActive;
            role.isActive = !oldValue;

            try {
                  await firstValueFrom(this.roleFacade.toggleActive(role.id));
                  this.toast.successRich('Cập nhật trạng thái thành công' );
                  return;
            } catch (error) {
                  this.toast.errorRich('Cập nhật trạng thái thất bại' );
                  const messages = handleHttpError(error);
                  console.error('Error toggling role status:', messages);
                  role.isActive = oldValue;
            }
      
      }
}     