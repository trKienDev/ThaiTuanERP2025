export interface PagedRequest {
      pageIndex: number;
      pageSize: number;
      keyword?: string;
      sort?: string; // e.g. "name:asc,createdDate:desc"
      filters?: Record<string, string | number | boolean | undefined>;
}

