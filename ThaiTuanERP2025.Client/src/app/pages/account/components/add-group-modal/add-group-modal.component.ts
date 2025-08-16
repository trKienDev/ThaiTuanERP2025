import { CommonModule } from "@angular/common";
import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { UserService } from "../../services/user.service";
import { GroupService } from "../../services/group.service";
import { trigger, transition, style, animate } from '@angular/animations';
import { forkJoin } from 'rxjs';
import { handleApiResponse } from "../../../../core/utils/handle-api-response.utils";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";
import { UserModel } from "../../models/user.model";
import { CreateGroupModel, GroupModel } from "../../models/group.model";

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

      group: CreateGroupModel = {
            name: '',
            description: '',
            adminUserId: '',
      };
      form!: FormGroup;
      users: UserModel[] = [];
      memberIds: string[] = [];
      errorMessages: string[] = [];

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
                  next: (data) => this.users = data,
                  error: err => this.errorMessages = handleHttpError(err)
            });
      }

      onSave(): void {
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  return;
            }

            const group = this.form.value;

            this.groupService.create({
                  name: group.name,
                  description: group.description,
                  adminUserId: group.adminUserId
            }).subscribe({
                  next: (groupDto) => {
                        const memberIds = group.memberIds.filter((id: string) => id !== group.adminUserId);
                        if(memberIds.length === 0) {
                              this.finish();
                              return;
                        }

                        const requests = memberIds.map((userId: string) => this.groupService.addUserToGroup(groupDto.id, userId));

                        forkJoin(requests).subscribe({
                              next: () => this.finish(),
                              error: err => {
                                    this.errorMessages = handleHttpError(err);
                              }
                        });
                  },
                  error: err => {
                        this.errorMessages = handleHttpError(err);
                        setTimeout(() => this.errorMessages = [], 5000);
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
