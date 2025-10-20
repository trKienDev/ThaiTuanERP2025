// src/app/shared/pipes/avatar-url.pipe.ts
import { Pipe, PipeTransform } from '@angular/core';
import { resolveAvatarUrl } from '../utils/avatar.utils'; // điều chỉnh đường dẫn cho đúng project
import { environment } from '../../../environments/environment';

type UserLike = { avatarFileId?: string | null; avatarFileObjectKey?: string | null } | null | undefined;

@Pipe({
      name: 'avatarUrlPipe',
      standalone: true,
      pure: true,
})
export class AvatarUrlPipe implements PipeTransform {
      transform(user: UserLike, baseUrl = environment.baseUrl): string {
            return resolveAvatarUrl(baseUrl, user);
      }
}
