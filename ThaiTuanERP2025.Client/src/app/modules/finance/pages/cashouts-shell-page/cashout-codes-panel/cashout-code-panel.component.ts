import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { CashoutCodeRequestDialogComponent } from "../../../components/cashout-code-request-dialog/cashout-code-request-dialog.component";
import { firstValueFrom } from "rxjs";
import { CashoutCodeApiService } from "../../../services/api/cashout-code-api.service";
import { CashoutGroupTreeWithCodeDto, CashoutTreeNode } from "../../../models/cashout-group.model";
import { KitSquareDownArrowComponent } from "../../../../../shared/icons/arrows/kit-square-down-arrow.component";
import { KitSquareRightArrowComponent } from "../../../../../shared/icons/arrows/kit-square-right-arrow.component";

@Component({
      selector: 'cashout-code-panel',
      standalone: true,
      imports: [CommonModule, KitSquareDownArrowComponent, KitSquareRightArrowComponent],
      templateUrl: './cashout-code-panel.component.html',
})
export class CashoutCodePanelComponent implements OnInit {
      private readonly dialog = inject(MatDialog);
      private readonly cashoutCodeApi = inject(CashoutCodeApiService);

      ngOnInit(): void {
            this.loadCashoutGroupTreeWithCode();
      }

      openCashoutCodeRequestDialog(): void {
            const dialogRef = this.dialog.open(CashoutCodeRequestDialogComponent);
      }

      // === TREE ===
      tree: CashoutGroupTreeWithCodeDto[] = []; // Tree hiển thị
      flatTree: CashoutTreeNode[] = []; // Danh sách phẳng (flatten) để *ngFor có thể duyệt toàn bộ group
      expanded: Set<string> = new Set();  // Track expand/collapse

      async loadCashoutGroupTreeWithCode() {
            const data = await firstValueFrom(this.cashoutCodeApi.getTree());
            console.log('data: ', data);
            this.tree = data;

            this.flatTree = [];
            this.flatten(data);
            console.log("FLAT TREE:", this.flatTree);
      }
      flatten(nodes: CashoutGroupTreeWithCodeDto[]) {
            for (const node of nodes) {

                  // Push group
                  this.flatTree.push({
                        ...node,
                        type: 'group'
                  });

                  // Auto expand 
                  this.expanded.add(node.id);

                  // Push codes ngay dưới group
                  for (const c of node.codes ?? []) {
                        this.flatTree.push({
                              ...c,
                              type: 'code',
                              parentId: node.id,
                              level: node.level + 1
                        });
                  }

                  // Đệ quy children
                  if (node.children?.length > 0) {
                        this.flatten(node.children);
                  }
            }
      }

      isExpanded(node: CashoutTreeNode): boolean {
            return this.expanded.has(node.id);
      }
      toggle(node: CashoutTreeNode) {
            if (node.type !== 'group') return;
            if (this.expanded.has(node.id)) this.expanded.delete(node.id);
            else this.expanded.add(node.id);
      }
      hasChildren(group: CashoutGroupTreeWithCodeDto): boolean {
            return group.children && group.children.length > 0;
      }

      isVisible(node: CashoutTreeNode): boolean {
            if (node.level === 0) return true;

            let parentId = node.parentId;

            while (parentId) {
                  const parent = this.flatTree.find(x => x.id === parentId && x.type === 'group');
                  if (!parent) return true;

                  if (!this.expanded.has(parent.id)) return false;

                  parentId = parent.parentId;
            }

            return true;
      }
      

}