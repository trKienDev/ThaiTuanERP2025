import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { UserService } from "../../services/user.service";
import { DepartmentService } from "../../services/department.service";
import { UserDto } from "../../models/user.model";

@Component({
      selector: 'account-member',
      standalone: true,
      imports: [CommonModule, ],
      templateUrl: './account-member.component.html',
      styleUrl: './account-member.component.scss',
}) 
export class AccountMemberComponent implements OnInit {
      showModal = false;
      users: UserDto[] = [];
      departmentMap: { [id: string]: string } = {}; 

      constructor(
            private userService: UserService,
            private departmentService: DepartmentService
      ){}

      ngOnInit(): void {

      }


}
