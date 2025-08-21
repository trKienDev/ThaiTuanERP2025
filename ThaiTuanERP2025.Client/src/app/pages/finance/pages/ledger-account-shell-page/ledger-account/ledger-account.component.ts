import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { LedgerAccountTreeDto } from '../../../models/ledger-account-tree.dto';
import { LedgerAccountService } from '../../../services/ledger-account.service';
import { CommonModule } from '@angular/common';
import { LedgerAccountFormDrawerComponent } from '../ledger-account-form-drawer/ledger-account-form-drawer.component';
import { LedgerAccountDto } from '../../../models/ledger-account.dto';

interface TreeNode {
  node: LedgerAccountTreeDto;
  children: TreeNode[];
  expanded: boolean;
  parentId?: string | null;
}

@Component({
  selector: 'app-ledger-account',
  imports: [ CommonModule, LedgerAccountFormDrawerComponent ],
  templateUrl: './ledger-account.component.html',
  styleUrls: ['./ledger-account.component.scss']
})
export class LedgerAccountComponent implements OnChanges {
  @Input() ledgerAccountTypeId!: string;
  
  accounts: LedgerAccountTreeDto[] = [];
  rawData: LedgerAccountTreeDto[] = [];
  tree: TreeNode[] = [];
  loading = false;

  selectedId: string | null = null;
  selectedNode: LedgerAccountTreeDto | null = null;

  drawerOpen = false;
  drawerMode: 'create' | 'edit' = 'create';
  selectedAccount: LedgerAccountDto | null = null;

  constructor(private service: LedgerAccountService) {}

  ngOnChanges(changes: SimpleChanges): void {
  if (changes['ledgerAccountTypeId'] && this.ledgerAccountTypeId) {
    this.loadAccounts();
  }
}

  loadAccounts() {
  this.service.getTreeByType(this.ledgerAccountTypeId).subscribe({
    next: (res) => {
  this.accounts = res;
  this.tree = this.buildTree(res);
},
    error: () => this.accounts = []
  });
}

  buildTree(items: LedgerAccountTreeDto[]): TreeNode[] {
    const map = new Map<string, TreeNode>();
    const roots: TreeNode[] = [];

    for (const item of items) {
      map.set(item.id, { node: item, children: [], expanded: true, parentId: item.parentId });
    }

    for (const item of items) {
      const treeNode = map.get(item.id)!;
      if (item.parentId && map.has(item.parentId)) {
        map.get(item.parentId)!.children.push(treeNode);
      } else {
        roots.push(treeNode);
      }
    }

    const sortFn = (a: TreeNode, b: TreeNode) => a.node.code.localeCompare(b.node.code);
    const sortTree = (nodes: TreeNode[]) => {
      nodes.sort(sortFn);
      nodes.forEach(n => sortTree(n.children));
    };
    sortTree(roots);
    return roots;
  }

  toggleExpand(node: TreeNode) {
    node.expanded = !node.expanded;
  }

  openCreate() {
  this.drawerMode = 'create';
  this.selectedAccount = null;
  this.drawerOpen = true;
}


  onSelect(node: TreeNode) {
    this.selectedId = node.node.id;
    this.selectedNode = node.node;
    this.drawerMode = 'edit';
    this.drawerOpen = true;
  }

  openCreateChild(node: TreeNode) {
    this.selectedNode = node.node;
    this.drawerMode = 'create';
    this.drawerOpen = true;
  }

  closeDrawer() {
    this.drawerOpen = false;
    this.selectedNode = null;
  }

  onSaved() {
    this.loadAccounts();
  }

  isSelected(node: TreeNode): boolean {
    return this.selectedId === node.node.id;
  }

  indent(level: number) {
    return { 'padding-left.px': level * 20 };
  }
}
