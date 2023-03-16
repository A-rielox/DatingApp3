export interface Pagination {
   currentPage: number;
   itemsPerPage: number;
   totalItems: number;
   totalPages: number;
}

export class PaginatedResult<T> {
   result?: T; // la lista d lo q pedi
   pagination?: Pagination;
}
