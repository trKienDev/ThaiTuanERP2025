import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { GroupService } from "../../services/group.service";
import { FormsModule } from "@angular/forms";
import { AddGroupModalComponent } from "../../components/add-group-modal/add-group-modal.component";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";
import { GroupModel } from "../../models/group.model";

@Component({
      selector: 'account-group',
      standalone: true,
      imports: [CommonModule, FormsModule, AddGroupModalComponent ],
      templateUrl: './account-group.component.html',
      styleUrl: './account-group.component.scss',
})
export class AccountGroupComponent {
      groups: GroupModel[] = [];
      searchKeyword = '';
      showAddGroupModal = false;

      constructor(private groupService: GroupService) {}

      ngOnInit(): void {
            this.loadGroups();
      }

      loadGroups(): void {
            this.groupService.getAll().subscribe({
                  next: (data) => this.groups = data,
                  error: err => alert(handleHttpError(err).join('\n'))
            })
      }

      filteredGroups(): GroupModel[] {
            return this.groups.filter(
                  g => g.name.toLowerCase().includes(this.searchKeyword.toLowerCase())
            );
      }

      onAddGroup() {
            this.showAddGroupModal = true;
      }

      onViewGroup(group: GroupModel) {
      // ví dụ: điều hướng đến route `/groups/:id`
            console.log('Xem nhóm:', group);
      }

      onEditGroup(group: GroupModel) {
      // mở modal hoặc chuyển sang trang chỉnh sửa nhóm
            console.log('Chỉnh sửa nhóm:', group);
      }

      onDeleteGroup(group: GroupModel) {
            const confirmDelte = confirm(`Bạn có chắc muốn xóa nhóm "${group.name} không ?`);
            if(!confirmDelte) return;

            const requestorId = '00000000-0000-0000-0000-000000000001'; // TODO: lấy ID user thực tế
            this.groupService.deleteGroup(group.id, requestorId).subscribe({
                  next: () => {
                        this.groups = this.groups.filter(g => g.id !== group.id);
                        alert('Đã xóa nhóm thành công');
                  },
                  error: err => alert(handleHttpError(err).join('\n'))
            });
      }
}