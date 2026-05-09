export interface Book {
  id: number;
  title: string;
  author: string;
  isbn: string;
  publishedDate: string;
  owner: string;
  isAvailable: boolean;
}

export interface CreateBookRequest {
  title: string;
  author: string;
  isbn: string;
  publishedDate: string;
  owner: string;
}

export interface UpdateBookRequest extends CreateBookRequest {
  isAvailable: boolean;
}
