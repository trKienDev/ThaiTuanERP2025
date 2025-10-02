import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { UserFacade } from '../../pages/account/facades/user.facade';
import { UserDto } from '../../pages/account/models/user.model';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';

@Component({
      selector: 'app-topbar',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './topbar.component.html',
      styleUrl: './topbar.component.scss'
})
export class TopbarComponent implements OnInit {
      private userFacade = inject(UserFacade);

      baseUrl: string = environment.baseUrl;      
      currentUser$ = this.userFacade.currentUser$;
      currentUser: UserDto | null = null;

      async ngOnInit(): Promise<void> {
            this.currentUser = await firstValueFrom(this.currentUser$);
      }

      get avatarSrc(): string {
            if (this.currentUser?.avatarFileId && this.currentUser.avatarFileId.startsWith('data:image')) {
                  return this.currentUser.avatarFileId; // base64 preview
            }
            if (this.currentUser?.avatarFileObjectKey) {
                  return this.baseUrl + '/files/public/' + this.currentUser.avatarFileObjectKey;
            }
            return 'default-user-avatar.jpg';
      }
}
