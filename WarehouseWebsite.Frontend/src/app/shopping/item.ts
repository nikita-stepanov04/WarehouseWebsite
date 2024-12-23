export class Item {
  id: string;
  name: string;
  quantity: number;
  description: string;
  price: number;
  weight: number;
  category: ItemCategory;
  photoUrl: string;

  constructor(
    id: string,
    name: string,
    quantity: number,
    description: string,
    price: number,
    weight: number,
    category: ItemCategory,
    photoUrl: string) {

    this.id = id;
    this.name = name;
    this.quantity = quantity;
    this.description = description;
    this.price = price;
    this.weight = weight;
    this.category = category;
    this.photoUrl = photoUrl;
  }
}

export enum ItemCategory {
  None = "None",
  Electronics = "Electronics",
  HomeGoods = "HomeGoods",
  Clothing = "Clothing",
  BuildingMaterials = "BuildingMaterials",
  Books = "Books"
}
