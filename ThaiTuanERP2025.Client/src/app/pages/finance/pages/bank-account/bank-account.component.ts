import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { PagedResult } from "../../../../shared/models/paged-result.model";
import { BankAccountDto } from "../../models/bank-account.model";
import { BankAccountService } from "../../services/bank-account.service";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";
import { FormsModule } from "@angular/forms";

@Component({
      selector: 'finance-bank-account',
      standalone: true,
      imports: [ CommonModule, FormsModule ],
      templateUrl: './bank-account.component.html',
      styleUrl: './bank-account.component.scss',
})
export class BankAccountComponent implements OnInit {
      bankAccounts: PagedResult<BankAccountDto> | null = null;
      isLoading = false;
      errorMessages?: string;

      // filters
      onlyActive: boolean | null = null;
      departmentId: string | null = null;

      // paging
      page = 1;
      pageSize = 20;

      constructor(private bankAccountService: BankAccountService) {}

      ngOnInit(): void {
            this.loadBankAccounts();
      }

      loadBankAccounts(): void {
            this.isLoading = true;
            this.errorMessages = undefined;

            this.bankAccountService.getPaged({
                  onlyActive: this.onlyActive ?? undefined,
                  departmentId: this.departmentId ?? undefined,
                  page: this.page,
                  pageSize: this.pageSize
            }).subscribe({
                  next: data => {
                        this.bankAccounts = data;
                        this.isLoading = false;
                  },
                  error: err => alert(handleHttpError(err).join('\n'))
            })
      }

      refreshFirstPage(): void {
            this.page = 1;
            this.loadBankAccounts();
      }

      totalPages(): number {
            if(!this.bankAccounts) return 1;
            return Math.max(1, Math.ceil(this.bankAccounts.totalCount / this.bankAccounts.pageSize));
      }

      prev(): void {
            if(this.page > 1) {
                  this.page--;
                  this.loadBankAccounts();
            }
      }
      next(): void {
            if(this.page < this.totalPages()) {
                  this.page++;
                  this.loadBankAccounts();
            }
      }

      onPageSizeChange(pageSize: number): void {
            this.pageSize = pageSize;
            this.page = 1;
            this.loadBankAccounts();
      }

      toggleActive(ba: BankAccountDto): void {
            this.bankAccountService.toggleStatus(ba.id, !ba.isActive).subscribe({
                  next: _ => this.loadBankAccounts(),
                  error: err => alert(handleHttpError(err).join('\n'))
            });
      }

      confirmDelete(ba: BankAccountDto): void {
            if(!confirm(`Xoá tài khoản ${ba.bankName} - ${ba.accountNumber}?`)) return;
            this.bankAccountService.delete(ba.id).subscribe({
                  next: _ => this.refreshFirstPage(),
                  error: err => alert(handleHttpError(err).join('\n'))
            })
      }
}