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
  get totalPages(): number {
    return this._totalPages;
  }
  set totalPages(value: number) {
    this._totalPages = value;
  }

  private _page: number;
  private _count: number;
  private _totalPages: number;

  constructor(page: number, count: number, totalPages: number) {
    this._page = page;
    this._count = count;
    this._totalPages = totalPages;
  }
}
