export class UserSettings {
  constructor(
    public id: string,
    public userId: string,
    public firstName: string,
    public lastName: string,
    public sellerLevel: string,
    public salesCompleted: number,
    public paypalEmail: string,
    public billingStreet: string,
    public billingAddressLine1: string,
    public billingAddressLine2: string,
    public billingZipCode: string,
    public billingCountry: string,
    public shippingStreet: string,
    public shippingAddressLine1: string,
    public shippingAddressLine2: string,
    public shippingZipCode: string,
    public shippingCountry: string
  ) {}
}
