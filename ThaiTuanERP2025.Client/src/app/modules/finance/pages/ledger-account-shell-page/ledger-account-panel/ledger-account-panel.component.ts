import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { KiAbacusIconComponent } from "../../../../../shared/icons/kit-abacus-icon.component";
import { MatDialog } from "@angular/material/dialog";
import { LedgerAccountRequestDialogComponent } from "../../../components/ledger-account-request-dialog/ledger-account-request-dialog.component";
import { LedgerAccountApiService } from "../../../services/api/ledger-account-api.service";
import { LedgerAccountTreeDto } from "../../../models/ledger-account.model";
import { firstValueFrom } from "rxjs";
import { LedgerAccountBalanceKind } from "../../../pipes/ledger-account.pipe";
import { HasPermissionDirective } from "../../../../../core/auth/auth.directive";
import { CashoutGroupFacade } from "../../../facades/cashout-group.facade";
import { KitSquareDownArrowComponent } from "../../../../../shared/icons/arrows/kit-square-down-arrow.component";
import { KitSquareRightArrowComponent } from "../../../../../shared/icons/arrows/kit-square-right-arrow.component";

@Component({
      selector: 'ledger-account-panel',
      standalone: true,
      imports: [CommonModule, KiAbacusIconComponent, LedgerAccountBalanceKind, HasPermissionDirective, KitSquareDownArrowComponent, KitSquareRightArrowComponent],
      templateUrl: './ledger-account-panel.component.html',
})
export class LedgerAccountPanelComponent implements OnInit {
      private readonly dialog = inject(MatDialog);
      private readonly ledgerAccountApi = inject(LedgerAccountApiService);
      private readonly cashoutGroupFacade = inject(CashoutGroupFacade);
      public ledgerAccountTrees: LedgerAccountTreeDto[] = [];

      ngOnInit(): void {
            this.loadLedgerAccounTree();
      }

      private async loadLedgerAccounTree(): Promise<void> {
            this.ledgerAccountTrees = await firstValueFrom(this.ledgerAccountApi.getTreeAsync());
      } 

      openLedgerAccountRequestDialog() {
            const dialogRef = this.dialog.open(LedgerAccountRequestDialogComponent);
            dialogRef.afterClosed().subscribe((isSuccess: boolean) => {
                  if (isSuccess) {
                        this.loadLedgerAccounTree();
                        this.cashoutGroupFacade.refresh();
                  }
            });
      }

      // ==== TABLE ====
      public expanded: Set<string> = new Set();
      
      isExpanded(node: LedgerAccountTreeDto): boolean {
            return this.expanded.has(node.id);
      }

      hasChildren(node: LedgerAccountTreeDto): boolean {
            return this.ledgerAccountTrees.some(x => x.parentId === node.id);
      }

      toggle(node: LedgerAccountTreeDto) {
            if (this.expanded.has(node.id)) this.expanded.delete(node.id);
            else this.expanded.add(node.id);
      }

      isVisible(node: LedgerAccountTreeDto): boolean {
            if (node.level === 0) return true;

            let parent = this.ledgerAccountTrees.find(x => x.id === node.parentId);
            while (parent) {
                  if (!this.expanded.has(parent.id)) return false;
                  parent = this.ledgerAccountTrees.find(x => x.id === parent?.parentId);
            }
            return true;
      }
}