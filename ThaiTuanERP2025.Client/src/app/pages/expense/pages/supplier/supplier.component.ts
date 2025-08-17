import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { SupplierDto } from "../../models/supplier.model";
import { SupplierService } from "../../services/supplier.service";
import { catchError, EMPTY } from "rxjs";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";

@Component({
      selector: 'expense-supplier',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './supplier.component.html',
      styleUrl: './supplier.component.scss',
})
export class SupplierComponent implements OnInit {
      suppliers: SupplierDto[] = [];
      total = 0;
      errorMessages: string[] = [];

      constructor(private supplierService: SupplierService) {}

      ngOnInit(): void {
            this.load();
      }

      load(page = 1): void {
            this.supplierService.getAll({page, pageSize: 20}).pipe(
                  catchError(err => {
                        this.errorMessages = handleHttpError(err);
                        return EMPTY;
                  })
            ).subscribe(res => {
                  this.suppliers = res.items;
                  this.total = res.totalCount;
            })
      }

      create() {
            this.supplierService.create({
                  code: 'VENDOR_ABC',
                  name: 'Công ty ABC',
                  defaultCurrency: 'VND',
                  paymentTermDays: 30
            }).pipe(
                  catchError(err => {
                        this.errorMessages = handleHttpError(err);
                        return EMPTY;
                  })
            ).subscribe(() => this.load());
      }

      toggle(item: SupplierDto) {
            this.supplierService.toggleStatus(item.id, !item.city).pipe(
                  catchError(err => {
                        this.errorMessages = handleHttpError(err);
                        return EMPTY;
                  })
            ).subscribe(updated => {
                  item.isActive = updated.isActive;
            })
      }
}