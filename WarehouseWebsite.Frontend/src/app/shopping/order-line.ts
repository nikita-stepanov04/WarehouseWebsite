export class OrderLine {
  itemId: string;
  quantity: number;

  constructor(itemId: string, quantity: number) {
    this.itemId = itemId;
    this.quantity = quantity;
  }
}
