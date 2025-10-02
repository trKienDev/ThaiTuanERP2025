import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, OnInit } from "@angular/core";
import {  ReactiveFormsModule } from "@angular/forms";
import { TaxService } from "../../services/tax.service";
import { TaxDto } from "../../models/tax.model";
import { MatButtonModule } from "@angular/material/button";
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { TaxRequestDialogComponent } from "./tax-request-dialog/tax-request-dialog.component";


@Component({
      selector: 'finance-tax',
      standalone: true,
      imports: [ CommonModule, ReactiveFormsModule, MatButtonModule, MatDialogModule ],
      templateUrl: './finance-tax.component.html',
      styleUrls: ['./finance-tax.component.scss'],
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class FinanceTaxComponent implements OnInit {
      taxes: TaxDto[] = [];

      ngOnInit(): void {
            this.loadTaxes();
      }

      constructor(
            private taxService: TaxService,
            private dialog: MatDialog
      ) {}

      loadTaxes(): void {
            this.taxService.getAll().subscribe({
                  next: (taxes) => {
                        this.taxes = taxes;
                  }
            });
      }

      openCreateTaxModal(): void {
            const dialogRef = this.dialog.open(TaxRequestDialogComponent, {
                  width: '520px',
                  disableClose: true
            });

            dialogRef.afterClosed().subscribe((result) => {
                  if(result === 'created') {
                        this.loadTaxes();
                  }
            });
      }
}