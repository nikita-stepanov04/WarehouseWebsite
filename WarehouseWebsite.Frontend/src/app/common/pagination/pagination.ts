export class Pagination {
  get count(): number {
    return this._count;
  }

  set count(value: number) {
    this._count = value;
  }
  get page(): number {
    return this._page;
  }
  set page(value: number) {
    this._page = value;
  }

  private _page: number;
  private _count: number;

  constructor(page: number, count: number) {
    this._page = page;
    this._count = count;
  }
}
