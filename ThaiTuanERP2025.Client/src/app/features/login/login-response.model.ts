export interface LoginResponseDto {
      userId: string;
      fullName: string;
      username: string;
      accessToken: string;
      expiresAt: string;
      roles: string[];
      permissions: string[];
}
