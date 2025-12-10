import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { UserDto } from "../../models/user.model";
import { MatDialog } from "@angular/material/dialog";
import { UserFacade } from "../../facades/user.facade";
import { KitActionMenuComponent } from "../../../../shared/components/kit-action-menu/kit-action-menu.component";
import { ActionMenuOption } from "../../../../shared/components/kit-action-menu/kit-action-menu.model";
import { AvatarUrlPipe } from "../../../../shared/pipes/avatar-url.pipe";
import { MemberManagerDialog } from "../../components/member-manager-dialog/member-manager-dialog.component";
import { MemberRequestDialog } from "../../components/member-request-dialog/member-request-dialog.component";
import { HasPermissionDirective } from "../../../../core/auth/auth.directive";
import { ConfirmService } from "../../../../shared/components/confirm-dialog/confirm.service";
import { firstValueFrom } from "rxjs";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";

@Component({
      selector: 'account-member',
      standalone: true,
      imports: [CommonModule, KitActionMenuComponent, AvatarUrlPipe, HasPermissionDirective],
      templateUrl: './account-member.component.html',
}) 
export class AccountMemberComponent {
      private readonly dialog = inject(MatDialog);
      private readonly userFacade = inject(UserFacade);
      private readonly confirmService = inject(ConfirmService);
      private readonly toast = inject(ToastService);

      users$ = this.userFacade.users$;
      trackById(index: number, item: UserDto) { return item.id; }

      openUserRequestDialog(): void {
            const dialog = this.dialog.open(MemberRequestDialog);
            dialog.afterClosed().subscribe();
      }

      editUser(user: UserDto): void {
            this.dialog.open(MemberRequestDialog, { data: user });
      }

      addUserManager(user: UserDto): void {
            this.dialog.open(MemberManagerDialog, { data: user });
      }

      deleteUser(user: UserDto): void {
            this.confirmService.warn$(`Bạn có chắc chắn muốn xóa user ${user.fullName}`)
                  .subscribe(async confirmed => {
                        if(confirmed) {
                              try {
                                    await firstValueFrom(this.userFacade.delete(user.id));
                              } catch(error) {
                                    this.toast.errorRich('Xóa user thất bại');
                                   const messages = handleHttpError(error).join('\n');
                                    this.confirmService.error$(messages);
                              }
                        }
                  });
      }

      buildUserActions(user: UserDto) : ActionMenuOption[] {
            return [
                  { label: '👨🏻‍💼 Chỉnh sửa quản lý', action: () => this.addUserManager(user) },
                  { label: '⚙️ Sửa', action: () => this.editUser(user) },
                  { label: '⛔ Xóa', color: 'red', action: () => this.deleteUser(user) },
            ]
      }
}
