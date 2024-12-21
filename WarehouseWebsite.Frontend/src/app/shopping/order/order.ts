export class Order {
  orderTime: string;
  status: string;
  totalPrice: number;
  orderItems: OrderItem[];

  constructor(orderTime: string, status: string, totalPrice: number, orderItems: OrderItem[]) {
    this.orderTime = orderTime;
    this.status = status;
    this.totalPrice = totalPrice;
    this.orderItems = orderItems;
  }
}

export class OrderItem {
  itemId: string;
  quantity: number;
  price: number;

  constructor(itemId: string, quantity: number, price: number) {
    this.itemId = itemId;
    this.quantity = quantity;
    this.price = price;
  }
}
