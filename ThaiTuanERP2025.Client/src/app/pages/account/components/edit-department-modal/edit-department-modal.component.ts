import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DepartmentModel } from "../../models/department.model";

@Component({
  selector: 'app-edit-department-modal',
  standalone: true,
  templateUrl: './edit-department-modal.component.html',
  styleUrls: ['./edit-department-modal.component.scss'],
  imports: [CommonModule, FormsModule],
})
export class EditDepartmentModalComponent {
  @Input() selectedDepartment: DepartmentModel = { id: '', code: '', name: '' };
  @Input() visible: boolean = false;

  @Output() submitUpdate = new EventEmitter<DepartmentModel>();
  @Output() cancel = new EventEmitter<void>();

  onSubmit(): void {
    if (this.selectedDepartment && this.selectedDepartment.id) {
      this.submitUpdate.emit(this.selectedDepartment);
    }
  }

  onCancel(): void {
    this.cancel.emit();
  }

  
}