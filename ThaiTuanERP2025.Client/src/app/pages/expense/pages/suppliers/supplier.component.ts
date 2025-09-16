import { CommonModule } from "@angular/common";
import { Component, OnInit, ViewChild } from "@angular/core";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { SupplierRequestDialogComponent } from "./supplier-request-dialog/supplier-request-dialog.component";
import { SupplierService } from "../../services/supplier.service";
import { SupplierDto } from "../../models/supplier.model";
import { MatButtonModule } from "@angular/material/button";
import { ReactiveFormsModule } from "@angular/forms";
import { BankAccountRequestDrawerComponent } from "../bank-accounts/bank-account-request-drawer/bank-account-request-drawer.component";
import { MatSidenavModule, MatDrawer } from "@angular/material/sidenav";

@Component({
      selector: 'expense-supplier',
      standalone: true,
      imports: [ CommonModule, MatButtonModule, MatDialogModule, ReactiveFormsModule, 
            BankAccountRequestDrawerComponent , MatSidenavModule
      ],
      templateUrl: './supplier.component.html',
})
export class ExpenseSupplierComponent implements OnInit {
      suppliers: SupplierDto[] = [];

      selectedOwnerId?: string;
      ownerType = 'supplier';

      @ViewChild('drawer') drawer!: MatDrawer

      constructor(
            private supplierService: SupplierService,
            private dialog: MatDialog
      ) {}

      ngOnInit(): void {
            this.loadSuppliers();
      }

      loadSuppliers(): void {
            this.supplierService.getAll().subscribe({
                  next: (suppliers) => {
                        this.suppliers = suppliers;
                  }
            })
      }

      openCreateSupplierDialog(): void {  
            const dialogRef = this.dialog.open(SupplierRequestDialogComponent, {
                  width: '520px',
                  disableClose: true
            });
            dialogRef.afterClosed().subscribe((result) => {
                  if(result === 'created') {
                        this.loadSuppliers();
                  }
            });
      }

      openBankAcountDrawerForSupplier(supplierId: string) {
            this.selectedOwnerId = supplierId;
            this.drawer.open();
      }
      onDrawerClosed(result?: 'created' | 'cancel') {
            this.drawer.close().then(() => {
                  this.selectedOwnerId = undefined;
                  if(result === 'created') {
                        this.loadSuppliers();
                  }
            })
      }
}