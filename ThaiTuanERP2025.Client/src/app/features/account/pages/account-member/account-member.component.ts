import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { UserDto } from "../../models/user.model";
import { MatDialog } from "@angular/material/dialog";
import { UserFacade } from "../../facades/user.facade";
import { MemberManagerDialog } from "../../dialogs/member-manager-dialog/member-manager-dialog.component";
import { KitActionMenuComponent } from "../../../../shared/components/kit-action-menu/kit-action-menu.component";
import { ActionMenuOption } from "../../../../shared/components/kit-action-menu/kit-action-menu.model";
import { MemberRequestDialog } from "../../dialogs/member-request-dialog/member-request-dialog.component";

@Component({
      selector: 'account-member',
      standalone: true,
      imports: [CommonModule, KitActionMenuComponent],
      templateUrl: './account-member.component.html',
}) 
export class AccountMemberComponent {
      private dialog = inject(MatDialog);
      private userFacade = inject(UserFacade);

      users$ = this.userFacade.users$;
      trackById(index: number, item: UserDto) { return item.id; }

      openUserRequestDialog(): void {
            const dialog = this.dialog.open(MemberRequestDialog);
            dialog.afterClosed().subscribe();
      }

      editUser(user: UserDto): void {
            const dialogRef = this.dialog.open(MemberRequestDialog, {
                  data: user
            });
      }

      addUserManager(user: UserDto): void {
            const dialogRef = this.dialog.open(MemberManagerDialog, { data: user });
      }

      buildUserActions(user: UserDto) : ActionMenuOption[] {
            return [
                  { label: '👨🏻‍💼 Chỉnh sửa quản lý', action: () => this.addUserManager(user) },
                  { label: '⚙️ Sửa', action: () => this.editUser(user) },
                  { label: '⛔ Xóa', color: 'red' },
            ]
      }

      deleteUser(): void {

      }

}
