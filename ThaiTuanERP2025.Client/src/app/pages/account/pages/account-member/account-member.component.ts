import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { AddUserModalComponent } from "../../components/add-user-modal/add-user-modal.component";
import { UserService } from "../../services/user.service";
import { DepartmentService } from "../../services/department.service";
import { handleApiResponse } from "../../../../core/utils/handle-api-response.utils";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";
import { CreateUserModel, UserModel } from "../../models/user.model";

@Component({
      selector: 'account-member',
      standalone: true,
      imports: [CommonModule, AddUserModalComponent],
      templateUrl: './account-member.component.html',
      styleUrl: './account-member.component.scss',
}) 
export class AccountMemberComponent implements OnInit {
      showModal = false;
      users: UserModel[] = [];
      departmentMap: { [id: string]: string } = {}; 

      constructor(
            private userService: UserService,
            private departmentService: DepartmentService
      ){}

      ngOnInit(): void {
            this.loadUsers();
      }

      loadUsers(): void {
            this.userService.getAllUsers().subscribe({
                  next: (users) => {
                        this.users = users;

                        const departmentIds = [...new Set(
                              users.map(u => u.departmentId).filter((id): id is string => !!id)
                        )];

                        if(departmentIds.length === 0) {
                              this.departmentMap = {};
                              return;
                        }

                        this.departmentService.getByIds(departmentIds).subscribe({
                              next: (departments) => {
                                    this.departmentMap = {};
                                    for(const dept of departments) {
                                          if(dept.id) this.departmentMap[dept.id] = dept.name;
                                    }
                              },   
                              error: err => alert(handleHttpError(err).join('\n'))
                        });
                  },
                  error: err => alert(handleHttpError(err).join('\n'))
            });
      }

      addUser({ user, callback}: {
            user: CreateUserModel,
            callback: (ok: boolean, message?: string) => void
      }) {
            this.userService.createUser(user).subscribe({
                  next: () => {
                              this.loadUsers();
                              callback(true);
                        },
                  error: err => {
                        const messages = handleHttpError(err);
                        callback(false, messages.join(', '));
                  }
            })
      };
}
