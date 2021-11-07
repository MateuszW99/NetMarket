export class ResetPasswordRequest {
  constructor(
    public email: string,
    public password: string,
    public newPassword: string
  ) {}
}
