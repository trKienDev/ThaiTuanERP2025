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

      addUser(newUser: Partial<User>) {
            this.userService.createUser(newUser).subscribe({
                  next: (res) => {
                        if(res.isSuccess && res.data) {
                              this.users.push(res.data);
                              this.showModal = false;
                        } else {
                              alert(res.message ?? 'Lỗi tạo user');
                        }
                  }, 
                  error: (err) => {
                        console.error('Lỗi khi tạo user: ', err);
                        alert('Không thể kết nối tới server');
                  }
            });
      }
}
