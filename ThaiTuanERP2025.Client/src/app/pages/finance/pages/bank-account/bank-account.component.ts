import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { PagedResult } from "../../../../core/models/paged-result.model";
import { BankAccountDto } from "../../models/bank-account.model";
import { BankAccountService } from "../../services/bank-account.service";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatAutocompleteModule } from "@angular/material/autocomplete";

@Component({
      selector: 'finance-bank-account',
      standalone: true,
      imports: [ CommonModule, FormsModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatAutocompleteModule ],
      templateUrl: './bank-account.component.html',
      styleUrl: './bank-account.component.scss',
})
export class BankAccountComponent implements OnInit {
      bankAccounts: PagedResult<BankAccountDto> | null = null;
      isLoading = false;
      errorMessages?: string;
      // filters
      onlyActive: boolean | null = null;
      // paging
      page = 1;
      pageSize = 20;
      // create form
      creating = false;
      createForm!: FormGroup;

      constructor(
            private bankAccountService: BankAccountService, 
            private formBuilder: FormBuilder,
      ) {}

      ngOnInit(): void {
            this.buildForm();
            this.loadBankAccounts();
      }

      private buildForm() {
            this.createForm = this.formBuilder.group({
                  accountNumber: ['', [Validators.required, Validators.maxLength(50)]],
                  bankName: ['', [Validators.required, Validators.maxLength(100)]],
                  accountHolder: ['', Validators.required, Validators.maxLength(100)],
                  ownerName: [''],
            });
      }

      loadBankAccounts(): void {
            this.isLoading = true;
            this.errorMessages = undefined;

            this.bankAccountService.getPaged({
                  onlyActive: this.onlyActive ?? undefined,
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

      toggleCreate() { this.creating = !this.creating };
      submitCreate() {
            if(this.createForm.invalid) {
                  this.createForm.markAllAsTouched();
                  return;
            }

            const command = this.createForm.value;
            this.bankAccountService.create(command).subscribe({
                  next: _ => {
                        this.creating = false;
                        this.createForm.reset();
                        this.refreshFirstPage();
                  },
                  error: err => alert(handleHttpError(err).join('\n'))
            });
      }
}