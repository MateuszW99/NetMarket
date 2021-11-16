export class AddItem {
  constructor(
    public name: string,
    public make: string,
    public model: string,
    public gender: string,
    public retailPrice: number,
    public description: string,
    public imageUrl: string,
    public smallImageUrl: string,
    public thumbUrl: string,
    public brand: string,
    public category: string
  ) {}
}
