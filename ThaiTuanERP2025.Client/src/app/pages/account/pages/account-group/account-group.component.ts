import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { GroupDto } from "../../dtos/group.dto";
import { GroupService } from "../../services/group.service";
import { catchError, throwError } from "rxjs";
import { FormsModule } from "@angular/forms";
import { AddGroupModalComponent } from "../../components/add-group-modal/add-group-modal.component";

@Component({
      selector: 'account-group',
      standalone: true,
      imports: [CommonModule, FormsModule, AddGroupModalComponent ],
      templateUrl: './account-group.component.html',
      styleUrl: './account-group.component.scss',
})
export class AccountGroupComponent {
      groups: GroupDto[] = [];
      searchKeyword = '';
      showAddGroupModal = false;

      constructor(private groupService: GroupService) {}

      ngOnInit(): void {
            this.loadGroups();
      }

      loadGroups(): void {
            this.groupService.getAllGroups().pipe(
                  catchError(err => {
                        alert('Lỗi khi tải nhóm');
                        console.error('Lỗi khi tải nhóm: ', err.message);
                        return throwError(() => err);
                  })
            ).subscribe(groups => this.groups = groups);
      }

      filteredGroups(): GroupDto[] {
            return this.groups.filter(
                  g => g.name.toLowerCase().includes(this.searchKeyword.toLowerCase())
            );
      }

      onAddGroup() {
            this.showAddGroupModal = true;
      }

      onViewGroup(group: GroupDto) {
      // ví dụ: điều hướng đến route `/groups/:id`
            console.log('Xem nhóm:', group);
      }

      onEditGroup(group: GroupDto) {
      // mở modal hoặc chuyển sang trang chỉnh sửa nhóm
            console.log('Chỉnh sửa nhóm:', group);
      }

      onDeleteGroup(group: GroupDto) {
            const confirmDelte = confirm(`Bạn có chắc muốn xóa nhóm "${group.name} không ?`);
            if(!confirmDelte) return;

            const requestorId = '00000000-0000-0000-0000-000000000001'; // TODO: lấy ID user thực tế
            this.groupService.deleteGroup(group.id, requestorId).subscribe({
                  next: () => {
                        this.groups = this.groups.filter(
                              g => g.id !== group.id
                        );
                        alert('Đã xóa nhóm thành công');
                  },
                  error: err => {
                        console.error('Lỗ khi xóa nhóm: ', err.message);
                        alert('Xóa nhóm thất bại');
                  }
            });
      }
}