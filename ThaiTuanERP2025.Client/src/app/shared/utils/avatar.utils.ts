export function resolveAvatarUrl(
      baseUrl: string,
      user: { avatarFileObjectKey?: string | null } | null | undefined
): string {
      return user?.avatarFileObjectKey ? `${baseUrl}/files/public/${user.avatarFileObjectKey}` : 'default-user-avatar.jpg'; // nhớ có sẵn ảnh mặc định
}