export interface PagedResult<T> {
  data: T[];
  totalItems: number;
  page: number;
  pageSize: number;
}
