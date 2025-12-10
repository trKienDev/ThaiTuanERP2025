import { PermissionDto } from "../../modules/account/models/permission.model";
import { RoleDto } from "../../modules/account/models/role.model";

export interface LoginResponseDto {
      userId: string;
      fullName: string;
      username: string;
      accessToken: string;
      refreshToken: string;
      expiresAt: string;
      roles: RoleDto[];
      permissions: PermissionDto[];
}
