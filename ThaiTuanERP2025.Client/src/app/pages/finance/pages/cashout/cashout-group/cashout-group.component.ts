import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { CashoutGroupRequestDialogComponent } from "../../cashouts-shell-page/cashout-groups/cashout-group-request-dialog/cashout-group-request-dialog.component";
import { MatButton } from "@angular/material/button";
import { CashoutGroupDto } from "../../../models/cashout-group.model";
import { CashoutGroupService } from "../../../services/cashout-group.service";

@Component({
      selector: 'finance-cashout-group',
      standalone: true,
      imports: [CommonModule, MatButton],
      templateUrl: './cashout-group.component.html',
      styleUrl: './cashout-group.component.scss',
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class CashoutGroupComponent {
      cashoutGroups: CashoutGroupDto[] = [];

      constructor(
            private cashoutGroupService: CashoutGroupService,
            private dialog: MatDialog,
      ) {}

      loadCashoutGroups(): void {
            this.cashoutGroupService.getAll().subscribe({
                  next: (groups) => {
                        this.cashoutGroups = groups;
                  }
            })
      }

      openCreateCashoutGroupModal(): void {
            const dialogRef = this.dialog.open(CashoutGroupRequestDialogComponent, {
                  width: '520px',
                  disableClose: true
            });

            dialogRef.afterClosed().subscribe((result) => {
                  if(result === 'created') {
                        close();
                  }
            })
      }
}