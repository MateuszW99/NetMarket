export class User {
  constructor(
    public id: string,
    public email: string,
    public role: string,
    public username: string,
    public tokenExpirationDate: Date,
    private _token: string
  ) {}

  get token(): string {
    if (!this.tokenExpirationDate || new Date() > this.tokenExpirationDate) {
      return null;
    }
    return this._token;
  }
}
