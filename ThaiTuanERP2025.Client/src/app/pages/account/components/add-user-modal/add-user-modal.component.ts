import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Output } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { User } from "../../models/user.model";
import { UserRole } from "../../models/user-roles.enum";
import { Department } from "../../models/department.model";
import { DepartmentService } from "../../services/department.service";
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from '@angular/material/button';

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
      @Output() save = new EventEmitter<any>();
      
      readonly roles = Object.values(UserRole);

      user: User = {
            fullName: '',
            username: '',
            employeeCode: '',
            email: '',
            password: '',
            role: UserRole.User,
            phone: undefined,
            department: '',
            position: '',
      };
      departments: Department[] = [];
      filteredDepartments: Department[] = [];
      selectedDepartmentName: string = '';

      showPassword = false;
      showConfirmPassword = false;
      confirmPassword: string = '';

      get passwordMismatch(): boolean {
            console.log('passsword mismatch');
            return this.user.password !== this.confirmPassword && this.confirmPassword !== '';
      }

      constructor(private departmentService: DepartmentService) {
            this.loadDepartments();
      }
      
      loadDepartments(): void {
            this.departmentService.getAll().subscribe({
                  next: (data) => {
                        this.departments = data;
                        this.filteredDepartments = data;
                  },
                  error: (err) => {
                        alert(err.message);
                        console.error(err);
                  }
            });
      }

      onDepartmentInputChange(input: string): void {
            this.filteredDepartments = this.departments.filter(dept => 
                  `${dept.code} ${dept.name}`.toLowerCase().includes(input.toLowerCase())
            )
      }

      onDepartmentSelected(deptName: string): void {
            const selected = this.departments.find(d => d.name === deptName);
            if(selected) {
                  this.user.department = selected.id ?? '';
            }
      }

      onDepartmentBlur(event: FocusEvent): void {
            const input = event.target as HTMLInputElement | null;
            if(!input) return;
            this.onDepartmentSelected(input.value);
      }

      onSubmit() {
            this.save.emit(this.user);
            this.onClose();
      }

      onClose() {
            this.close.emit();
      }
}