import { CommonModule } from "@angular/common";
import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpClient } from '@angular/common/http';
import { environment } from "../../../../../environments/environment";
import * as XLSX from 'xlsx';
import { ExcelImportService } from "../../../../shared/services/excel/excel-import.service";
import { ApiResponse } from "../../../../core/models/api-response.model";

@Component({
      selector: 'account-department',
      standalone: true,
      imports: [ CommonModule, FormsModule ],
      templateUrl: './account-department.component.html',
      styleUrl: './account-department.component.scss'
})
export class AccountDepartmentComponent implements OnInit {
      private readonly API_URL = `${environment.apiUrl}/department`;
      newDepartment = {
            code: '',
            name: '',
      };
      successMessage: string | null = null;
      departments: { id: string, code: string, name: string, selected: boolean }[] = [];
      importedDepartments: { code: string; name: string }[] = [];

      @ViewChild('masterCheckbox', { static: false}) masterCheckbox!: ElementRef<HTMLInputElement>;

      constructor(private http: HttpClient, private excelService: ExcelImportService){}

      ngOnInit(): void {
            this.loadDepartments();
      }

      toggleAll(event: Event) {
            const checked = (event.target as HTMLInputElement).checked;
            this.departments.forEach(d => d.selected = checked);
            this.updateMasterCheckboxState();
      }

      updateMasterCheckboxState() {
            const allSelected = this.departments.every(d => d.selected);
            const noneSelected = this.departments.every(d => !d.selected);

            const checkbox = this.masterCheckbox?.nativeElement;
            if(checkbox) {
                  checkbox.indeterminate = !allSelected && !noneSelected;
                  checkbox.checked = allSelected;
            }
      }

      isAllSelected(): boolean {
            return this.departments.length > 0 && this.departments.every(d => d.selected);
      }

      loadDepartments() {
            this.http.get<ApiResponse<{ id: string, name: string, code: string }[]>>(`${this.API_URL}/all`).subscribe({
                  next: (res) => {
                        if(res.isSuccess && res.data) {
                              console.log('success: ', res.data);
                              this.departments = res.data.map(d => ({ ...d, selected: false }));
                              this.updateMasterCheckboxState();
                        } else {
                              alert(res.message || 'Không thể tải danh sách phòng ban');
                        }
                  },
                  error: (err) => {
                        const message = err?.error?.message || 'Không thể thêm phòng ban';
                        alert(`Lỗi: ${message}`);
                        throw new Error(`Lỗi khi tải phòng ban: ${err}`);
                  }
            });
      }

      addDepartment() {
            this.http.post<ApiResponse<{departmentId: string}>>(this.API_URL, this.newDepartment).subscribe({
                  next: (res) => {
                        if(res.isSuccess) {
                              this.newDepartment = { code: '', name: '' };
                              this.successMessage = 'Đã thêm phòng ban thành công!';
                              this.loadDepartments();
                              setTimeout(() => this.successMessage = null, 3000);
                        } else {
                              alert(res.message || 'Thêm phòng ban thất bại');
                        }
                  }, 
                  error: (err) => {
                        const message = err?.error?.message || 'Không thể thêm phòng ban';
                        alert(`Lỗi: ${message}`);
                        console.log('err: ', err);
                        throw new Error(`Lỗi khi thêm phòng ban: ${err}`);
                  }
            });
      }

      async onFileSelected(event: Event) {
            const file = (event.target as HTMLInputElement).files?.[0];
            if(!file) return;

            try {
                  const rows = await this.excelService.parseExcelFile(file);
                  this.importedDepartments = this.excelService.mapToDepartment(rows);
            } catch(err) {
                  alert('Lỗi khi đọc file excel');
                  console.error(err);
            }
      }

      uploadExcel() {
            if(this.importedDepartments.length === 0) return;
            const payload = { departments: this.importedDepartments };
            this.http.post<ApiResponse<{ added: number }>>(`${this.API_URL}/bulk`, payload).subscribe({
                  next: (res) => {
                        this.successMessage = res.message || `Đã import ${res.data?.added} phòng ban thành công`;
                        this.importedDepartments = [];
                        this.loadDepartments();
                        setTimeout(() => this.successMessage = null, 3000);
                  }, 
                  error: (err) => {
                        alert('Lỗi khi import file excel');
                        console.error(err);
                  }
            });
      }
}