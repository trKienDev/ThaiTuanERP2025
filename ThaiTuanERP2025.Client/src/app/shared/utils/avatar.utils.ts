export function resolveAvatarUrl(
      baseUrl: string,
      user: { avatarFileId?: string | null; avatarFileObjectKey?: string | null } | null | undefined
): string {
      if (user?.avatarFileId && user.avatarFileId.startsWith('data:image')) {
            return user.avatarFileId; // base64 preview
      }
      if (user?.avatarFileObjectKey) {
            return `${baseUrl}/files/public/${user.avatarFileObjectKey}`;
      }
      return 'default-user-avatar.jpg';
}