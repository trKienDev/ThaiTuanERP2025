import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { User } from "../../models/user.model";
import { AddUserModalComponent } from "../../components/add-user-modal/add-user-modal.component";
import { UserService } from "../../services/user.service";
import { DepartmentService } from "../../services/department.service";

@Component({
      selector: 'account-member',
      standalone: true,
      imports: [CommonModule, AddUserModalComponent],
      templateUrl: './account-member.component.html',
      styleUrl: './account-member.component.scss',
}) 
export class AccountMemberComponent implements OnInit {
      showModal = false;
      users: User[] = [];
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
                        console.log('users: ', users);
                        const departmentIds = [...new Set(users.map(u => u.departmentId))];
                        console.log('departmentids: ', departmentIds);
                        this.departmentService.getByIds(departmentIds).subscribe({
                              next: (res) => {
                                    if(res.isSuccess && res.data) {
                                          this.departmentMap = {};
                                          for(const dept of res.data) {
                                                if (dept.id) this.departmentMap[dept.id] = dept.name;
                                          }
                                    } else {
                                          alert('Không thể tải phòng ban');
                                    }
                              }
                        })
                  }, 
                  error: (err) => alert(err.message)
            });
      }

      addUser({ user, callback}: {
            user: Partial<User>,
            callback: (ok: boolean, message?: string) => void
      }) {
            this.userService.createUser(user).subscribe({
                  next: (res) => {
                        if(res.isSuccess && res.data) {
                              this.loadUsers();
                              callback(true);
                        } else {
                              callback(false, res.message ?? 'Tạo user thất bại');
                        }
                  }, error: (err) => {
                        console.error('Lỗi khi tạo user: ', err);
                        callback(false, 'Không thể kết nối tới server');
                  }
            })
      };
}
