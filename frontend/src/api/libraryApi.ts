import { Book, CreateBookRequest, UpdateBookRequest } from '../types/book';

const BASE_URL = 'http://localhost:5110/api';

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${BASE_URL}${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  });

  if (!response.ok) {
    const error = await response.text();
    throw new Error(error || `Request failed: ${response.status}`);
  }

  if (response.status === 204) return undefined as unknown as T;
  return response.json();
}

export const libraryApi = {
  getBooks: (search?: string, isAvailable?: boolean): Promise<Book[]> => {
    const params = new URLSearchParams();
    if (search) params.set('search', search);
    if (isAvailable !== undefined) params.set('isAvailable', String(isAvailable));
    const query = params.toString();
    return request<Book[]>(`/books${query ? `?${query}` : ''}`);
  },

  getBook: (id: number): Promise<Book> =>
    request<Book>(`/books/${id}`),

  createBook: (book: CreateBookRequest): Promise<Book> =>
    request<Book>('/books', { method: 'POST', body: JSON.stringify(book) }),

  updateBook: (id: number, book: UpdateBookRequest): Promise<Book> =>
    request<Book>(`/books/${id}`, { method: 'PUT', body: JSON.stringify(book) }),

  deleteBook: (id: number): Promise<void> =>
    request<void>(`/books/${id}`, { method: 'DELETE' }),

  toggleAvailability: (id: number): Promise<Book> =>
    request<Book>(`/books/${id}/availability`, { method: 'PATCH' }),
};
