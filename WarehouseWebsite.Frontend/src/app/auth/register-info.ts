export class RegisterInfo {
  email: string;
  password: string;
  name: string;
  surname: string;

  constructor(email: string, password: string, name: string, surname: string) {
    this.email = email;
    this.password = password;
    this.name = name;
    this.surname = surname;
  }
}
