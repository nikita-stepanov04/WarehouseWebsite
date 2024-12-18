export class JwtResponse {
  accessToken: string;
  refreshToken: string;
  roles: string[];

  constructor(accessToken: string, refreshToken: string, roles: string[]) {
    this.accessToken = accessToken;
    this.refreshToken = refreshToken;
    this.roles = roles;
  }
}
