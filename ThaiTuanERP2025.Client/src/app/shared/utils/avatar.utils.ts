import { environment } from "../../../environments/environment";

export function resolveAvatarUrl(
      user: { avatarFileId?: string | null; avatarFileObjectKey?: string | null } | null | undefined
): string {
      const baseUrl = environment.server.baseUrl;
      if (user?.avatarFileId && user.avatarFileId.startsWith('data:image')) {
            return user.avatarFileId; // base64 preview
      }
      if (user?.avatarFileObjectKey) {
            return `${baseUrl}/files/public/${user.avatarFileObjectKey}`;
      }
      return 'default-user-avatar.jpg';
}