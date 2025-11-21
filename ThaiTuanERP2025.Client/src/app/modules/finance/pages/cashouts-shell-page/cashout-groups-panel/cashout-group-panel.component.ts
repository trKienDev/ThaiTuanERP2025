import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { CashoutGroupRequestDialogComponent } from "../../../components/cashout-group-request-dialog/cashout-group-request-dialog.component";
import { MatDialog } from "@angular/material/dialog";
import { CashoutGroupTreeDto } from "../../../models/cashout-group.model";
import { firstValueFrom } from "rxjs";
import { CashoutGroupApiService } from "../../../services/api/cashout-group-api.service";

@Component({
      selector: 'cashout-group-panel',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './cashout-group-panel.component.html'
})
export class CashoutGroupPanelComponent implements OnInit {
      private readonly dialog = inject(MatDialog);
      private readonly cashoutGroupApi = inject(CashoutGroupApiService);
      cashoutGroupTrees: CashoutGroupTreeDto[] = [];

      ngOnInit(): void {
            this.loadCashoutGroupTree();
      }

      private async loadCashoutGroupTree(): Promise<void> {
            this.cashoutGroupTrees = await firstValueFrom(this.cashoutGroupApi.getTree());
            console.log('tree: ', this.cashoutGroupTrees);
      }

      openCashoutGroupRequestModal(): void {
            const dialogRef = this.dialog.open(CashoutGroupRequestDialogComponent);
            dialogRef.afterClosed().subscribe((isSuccess: boolean) => {
                  if(isSuccess)
                        this.loadCashoutGroupTree();
            });
      }

      // ==== TREE ====
      public expanded: Set<string> = new Set();

      isExpanded(node: CashoutGroupTreeDto): boolean {
            return this.expanded.has(node.id);
      }
      hasChildren(node: CashoutGroupTreeDto): boolean {
            return this.cashoutGroupTrees.some(x => x.parentId === node.id);
      }

      toggle(node: CashoutGroupTreeDto) {
            if (this.expanded.has(node.id)) this.expanded.delete(node.id);
            else this.expanded.add(node.id);
      }

      isVisible(node: CashoutGroupTreeDto): boolean {
            if (node.level === 0) return true;

            let parent = this.cashoutGroupTrees.find(x => x.id === node.parentId);
            while (parent) {
                  if (!this.expanded.has(parent.id)) return false;
                  parent = this.cashoutGroupTrees.find(x => x.id === parent?.parentId);
            }
            return true;
      }
}