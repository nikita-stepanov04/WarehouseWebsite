export class Customer {
  email: string;
  name: string;
  surname: string;
  address: string;

  constructor(email: string, name: string, surname: string, address: string) {
    this.email = email;
    this.name = name;
    this.surname = surname;
    this.address = address;
  }
}
