import { DepartmentDto } from "../../account/models/department.model";
import { UserDto } from "../../account/models/user.model";

export interface BudgetApproversRequest {
      approverId: string;
      slaHours: number;
      departmentIds: string[];
}

export interface BudgetApproverDto {
      id: string;
      approverUser: UserDto;
      slaHours: number;
      departments: DepartmentDto[];
}

export interface UpdateBudgetApproverDepartmentRequest {
      departmentIds: string[];
}
