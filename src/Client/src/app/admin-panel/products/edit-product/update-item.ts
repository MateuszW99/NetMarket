import { Brand } from 'src/app/shared/items/brand.model';

export class UpdateItem {
  constructor(
    public id: string,
    public name: string,
    public make: string,
    public model: string,
    public gender: string,
    public retailPrice: number,
    public description: string,
    public imageUrl: string,
    public brand: Brand,
    public category: string
  ) {}
}
