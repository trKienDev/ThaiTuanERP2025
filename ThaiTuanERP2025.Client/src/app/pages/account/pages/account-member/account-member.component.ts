import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { User } from "../../models/user.model";
import { AddUserModalComponent } from "../../components/add-user-modal/add-user-modal.component";
import { UserService } from "../../services/user.service";

@Component({
      selector: 'account-member',
      standalone: true,
      imports: [CommonModule, AddUserModalComponent],
      templateUrl: './account-member.component.html',
      styleUrl: './account-member.component.scss',
}) 
export class AccountMemberComponent {
      showModal = false;
      users: User[] = [];

      constructor(private userService: UserService){}

      addUser({ user, callback}: {
            user: Partial<User>,
            callback: (ok: boolean, message?: string) => void
      }) {
            this.userService.createUser(user).subscribe({
                  next: (res) => {
                        if(res.isSuccess && res.data) {
                              this.users.push(res.data);
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
