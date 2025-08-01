import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { User } from "../../models/user.model";
import { UserService } from "../../services/user.service";

@Component({
      selector: 'account-profile',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './account-profile.component.html',
      styleUrl: './account-profile.component.scss',
})
export class AccountProfileComponent implements OnInit {
      user: User | null = null;
      
      constructor(private userService: UserService) {}

      ngOnInit(): void {
            this.userService.getCurrentuser().subscribe({
                  next: (res) => {
                        if(res.isSuccess && res.data) {
                              this.user = res.data;
                        }
                  },
                  error: () => {
                        alert('Không thể lấy thông tin người dùng hiện tại');
                  }
            });
      }
}