import { CommonModule } from "@angular/common";
import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ExcelImportService } from "../../../../shared/services/excel/excel-import.service";
import { DepartmentService } from "../../services/department.service";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";
import { DepartmentModel } from "../../models/department.model";
import { EditDepartmentModalComponent } from "../../components/edit-department-modal/edit-department-modal.component";

@Component({
      selector: 'account-department',
      standalone: true,
      imports: [ CommonModule, FormsModule, EditDepartmentModalComponent ],
      templateUrl: './account-department.component.html',
      styleUrl: './account-department.component.scss'
})
export class AccountDepartmentComponent implements OnInit {
      newDepartment = { code: '', name: '', };
      successMessage: string | null = null;
      departments: (DepartmentModel & { selected: boolean })[] = [];
      importedDepartments: DepartmentModel[] = [];
      isEditing = false;
      selectedDepartment: DepartmentModel = { id: '', code: '', name: '' };

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
                        this.departments = data.map(d => ({ ...d, selected: false }));
                        this.updateMasterCheckboxState();
                  }, 
                  error: err => alert(handleHttpError(err).join('\n'))
            });
      }

      addDepartment(): void {
            this.departmentService.create(this.newDepartment).subscribe({
                  next: () => {
                        this.newDepartment = { code: '', name: '' };
                        this.successMessage = 'Đã thêm phòng ban thành công';
                        this.loadDepartments();
                        setTimeout(() => this.successMessage = null, 3000); 
                  },
                  error: err => alert(handleHttpError(err).join('\n'))
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
                  error: err => alert(handleHttpError(err).join('\n'))
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

      onDelete(dept: DepartmentModel): void {     
            if (!dept.id) {
                  alert('ID không hợp lệ');
                  return;
            }

            if (confirm('Bạn có chắc muốn xóa phòng ban này?')) {
                  this.departmentService.deleteDepartment(dept.id).subscribe({
                        next: () => {
                              this.successMessage = 'Xóa thành công';
                              this.loadDepartments();
                              setTimeout(() => this.successMessage = null, 3000);
                        },
                        error: (err) => alert('Lỗi xóa: ' + err.message),
                  });
            }
      }

      openEditModal(dept: DepartmentModel): void {
            this.selectedDepartment = { ...dept };            
            this.isEditing = true;
      }

      cancelEdit(): void {
            this.isEditing = false;
            this.selectedDepartment = { id: '', code: '', name: '' };
      }

      updateDepartment(updated: DepartmentModel): void {
            if (!updated.id) {
                  alert('ID phòng ban không hợp lệ');
                  return;
            }

            this.departmentService.updateDepartment(updated.id, {
                  id: updated.id,
                  code: updated.code,
                  name: updated.name
            }).subscribe({
                  next: () => {
                        this.successMessage = 'Cập nhật phòng ban thành công!';
                        this.loadDepartments();
                        this.cancelEdit();
                        setTimeout(() => this.successMessage = null, 3000);
                  },
                  error: (err) => alert(err.message)
            });
      }
}