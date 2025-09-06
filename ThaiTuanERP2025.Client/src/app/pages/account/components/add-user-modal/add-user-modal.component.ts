import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Output } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { UserRole } from "../../models/user-roles.enum";
import { DepartmentService } from "../../services/department.service";
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from '@angular/material/button';
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";
import { CreateUserRequest, UserDto } from "../../models/user.model";
import { DepartmentDto } from "../../models/department.model";

@Component({
      selector: 'add-user-modal',
      standalone: true,
      imports: [CommonModule, FormsModule, ReactiveFormsModule, 
            MatInputModule, MatFormFieldModule, MatAutocompleteModule,
            MatIconModule, MatButtonModule
      ],
      templateUrl: './add-user-modal.component.html',
      styleUrl: './add-user-modal.component.scss',
})
export class AddUserModalComponent {
      @Output() close = new EventEmitter<void>();
      @Output() save = new EventEmitter<{
            user: CreateUserRequest,
            callback: (ok: boolean, message?: string) => void // truyền callback
      }>();
      
      readonly roles = Object.values(UserRole);

      user: UserDto = {
            id: '',
            fullName: '',
            username: '',
            employeeCode: '',
            email: '',
            password: '',
            role: UserRole.User,
            phone: undefined,
            departmentId: '',
            department: undefined,
            position: '',
      };
      departments: DepartmentDto[] = [];
      filteredDepartments: DepartmentDto[] = [];
      selectedDepartmentName: string = '';
      showPassword = false;

      constructor(private departmentService: DepartmentService) {
            this.loadDepartments();
      }
      
      loadDepartments(): void {
            this.departmentService.getAll().subscribe({
                  next: (data) => {
                        this.departments = data;
                        this.filteredDepartments = data;
                  },
                  error: err => alert(handleHttpError(err).join('\n'))
            });
      }

      onDepartmentInputChange(value: string) {
            const lowerValue = value.toLowerCase(); 
            this.filteredDepartments = this.departments.filter(dept => 
                  dept.name.toLowerCase().includes(lowerValue) ||
                  dept.code.toLowerCase().includes(lowerValue)
            );
      }
      onDepartmentSelected(deptId: string) {
            this.user.department?.id ?? deptId;
      }
      displayDepartmentFn = (deptId: string): string => {
            const dept = this.departments.find(d => d.id === deptId);
            return dept ? `${dept.code} - ${dept.name}` : '';
      }

      onSubmit() {
            const dto: CreateUserRequest  = {
                  fullName: this.user.fullName,
                  username: this.user.username,
                  employeeCode: this.user.employeeCode,
                  email: this.user.email,
                  password: this.user.password,
                  role: this.user.role,
                  phone: this.user.phone,
                  departmentId: this.user.departmentId,
                  position: this.user.position
            };
            this.save.emit({
                  user: dto,
                  callback: (ok, message?: string) => {
                        if(ok) { 
                              alert('Tạo user thành công');
                              this.onClose(); 
                        }
                        else alert(message || 'Lỗi tạo user');
                  }
            });
      }

      onClose() {
            this.close.emit();
      }
}