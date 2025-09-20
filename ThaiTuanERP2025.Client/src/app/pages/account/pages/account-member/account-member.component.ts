import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { UserDto } from "../../models/user.model";
import { MatDialog } from "@angular/material/dialog";
import { UserFacade } from "../../facades/user.facade";
import { MemberRequestDialog } from "./member-request-dialog/member-request-dialog.component";
import { ConnectedPosition, OverlayModule } from "@angular/cdk/overlay";
import { MemberManagerDialog } from "./member-manager-dialog/member-manager-dialog.component";

@Component({
      selector: 'account-member',
      standalone: true,
      imports: [CommonModule, OverlayModule ],
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

      openUserMenu: number | null = null;
      userMenuOpenIndex: number | null = null;
      stepMenuOverlayPosition: ConnectedPosition[] = [
            { originX: 'center', originY: 'bottom', overlayX: 'center', overlayY: 'top', offsetY: 8 },
            { originX: 'end', originY: 'top',    overlayX: 'end',    overlayY: 'bottom', offsetY: -8 },
      ]
      toggleUserMenu(i: number, ev: MouseEvent) {
            ev.stopPropagation();
            this.userMenuOpenIndex = (this.userMenuOpenIndex === i) ? null : i;
      }
      onUserMenuClosed() {
            this.userMenuOpenIndex = null;
      }

}
