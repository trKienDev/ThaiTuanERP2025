import { CommonModule } from "@angular/common";
import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { CreateGroupDto } from "../../dtos/group.dto";
import { UserDto } from "../../dtos/user.dto";
import { UserService } from "../../services/user.service";
import { GroupService } from "../../services/group.service";
import { trigger, transition, style, animate } from '@angular/animations';
import { forkJoin } from 'rxjs';

@Component({
      selector: 'add-group-modal',
      standalone: true,
      imports: [ CommonModule, FormsModule, ReactiveFormsModule ],
      templateUrl: './add-group-modal.component.html',
      styleUrl: './add-group-modal.component.scss',
      animations: [
            trigger('fadeInOut', [
                  transition(':enter', [
                        style({ opacity: 0 }),
                        animate('200ms ease-in', style({ opacity: 1}))
                  ]),
                  transition(':leave', [
                        animate('200ms ease-in', style({ opacity: 0 }))
                  ])
            ])
      ]
})
export class AddGroupModalComponent implements OnInit {
      @Output() close = new EventEmitter<void>();
      @Output() saved = new EventEmitter<void>();

      group: CreateGroupDto = {
            name: '',
            description: '',
            adminUserId: '',
      };
      form!: FormGroup;
      users: UserDto[] = [];
      memberIds: string[] = [];

      constructor(
            private fb: FormBuilder,
            private userService: UserService,
            private groupService: GroupService
      ) {}

      ngOnInit(): void {
            this.form = this.fb.group({
                  name: ['', Validators.required],
                  description: [''],
                  adminUserId: ['', Validators.required],
                  memberIds: [[]]
            });
            this.userService.getAllUsers().subscribe({
                  next: users => this.users = users,
                  error: err => alert('Không thể tải danh sách người dùng')
            });
      }

      onSave(): void {
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  return;
            }

            const group = this.form.value;

            this.groupService.createGroup({
                  name: group.name,
                  description: group.description,
                  adminUserId: group.adminUserId
            }).subscribe({
                  next: res => {
                        if(res.isSuccess && res.data) {
                              const groupDto = res.data;
                              const memberIds = group.memberIds.filter((id: string) => id !== this.group.adminUserId);
                              
                              if(memberIds.lenght === 0) {
                                    this.finish();
                                    return;
                              }

                              const request = memberIds.map((userId: string) => this.groupService.addUserToGroup(groupDto.id, userId));
                              
                              forkJoin(request).subscribe({
                                    next: () => this.finish(),
                                    error: err => {
                                          console.error(err);
                                          alert('Lỗi khi thêm thành viên')
                                    }
                              })
                        } else {
                              alert(res.message || 'Tạo nhóm thất bại');
                        }
                  },
                  error: err => {
                        alert('Lỗi khi tạo nhóm');
                        console.error(err);
                  }
            });
      }

      onCancel(): void {
            this.close.emit();
      }

      finish(): void {
            alert('Tạo nhóm thành công');
            this.saved.emit();
            this.close.emit();
      }
}
