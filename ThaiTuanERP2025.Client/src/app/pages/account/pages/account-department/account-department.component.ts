import { CommonModule } from "@angular/common";
import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ExcelImportService } from "../../../../shared/services/excel/excel-import.service";
import { Department } from "../../models/department.model";
import { DepartmentService } from "../../services/department.service";

@Component({
      selector: 'account-department',
      standalone: true,
      imports: [ CommonModule, FormsModule ],
      templateUrl: './account-department.component.html',
      styleUrl: './account-department.component.scss'
})
export class AccountDepartmentComponent implements OnInit {
      newDepartment = { code: '', name: '', };
      successMessage: string | null = null;
      departments: (Department & { selected: boolean })[] = [];
      importedDepartments: Department[] = [];

      @ViewChild('masterCheckbox', { static: false }) masterCheckbox!: ElementRef<HTMLInputElement>;
      constructor(
            private departmentService: DepartmentService,
            private excelService: ExcelImportService
      ) {}

      ngOnInit(): void {
            this.loadDepartments();
      }

      loadDepartments(): void {
            this.departmentService.getAll().subscribe({
                  next: (data) => {
                        this.departments = data.map(d => ({ ...d, selected: false })),
                        this.updateMasterCheckboxState();
                  },
                  error: (err) => alert(err.message)
            });
      }

      addDepartment(): void {
            this.departmentService.add(this.newDepartment).subscribe({
                  next: () => {
                        this.newDepartment = { code: '', name: ''};
                        this.successMessage = 'Đã thêm phòng ban thành công!';
                        this.loadDepartments();
                        setTimeout(() => this.successMessage = null, 3000);
                  }, 
                  error: (err) => alert(err.message)
            });
      }

      async onFileSelected(event: Event): Promise<void> {
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

      uploadExcel(): void {
            if(this.importedDepartments.length === 0) return;
            this.departmentService.importExcel(this.importedDepartments).subscribe({
                  next: (added) => {
                        this.successMessage = `Đã import ${added} phòng ban thành công`;
                        this.importedDepartments = [];
                        this.loadDepartments();
                        setTimeout(() => this.successMessage = null, 3000);
                  }, 
                  error: (err) => alert(err.message)
            });
      }

      toggleAll(event: Event): void {
            const checked = (event.target as HTMLInputElement).checked;
            this.departments.forEach(d => d.selected = checked);
            this.updateMasterCheckboxState();
      }
      updateMasterCheckboxState(): void {
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
}