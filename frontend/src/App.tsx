import { useCallback, useEffect, useState } from 'react';
import { libraryApi } from './api/libraryApi';
import { AddBookModal } from './components/AddBookModal/AddBookModal';
import { BookDetailModal } from './components/BookDetailModal/BookDetailModal';
import { BookTable } from './components/BookTable/BookTable';
import { Pagination } from './components/Pagination/Pagination';
import { SearchBar } from './components/SearchBar/SearchBar';
import { Book, CreateBookRequest } from './types/book';

const PAGE_SIZE = 8;

function App() {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [search, setSearch] = useState('');
  const [availabilityFilter, setAvailabilityFilter] = useState<boolean | undefined>(undefined);
  const [currentPage, setCurrentPage] = useState(1);
  const [showAddModal, setShowAddModal] = useState(false);
  const [selectedBook, setSelectedBook] = useState<Book | null>(null);

  const fetchBooks = useCallback(async () => {
    setLoading(true);
    setError('');
    try {
      const data = await libraryApi.getBooks(search || undefined, availabilityFilter);
      setBooks(data);
      setCurrentPage(1);
    } catch {
      setError('Failed to load books. Is the API running?');
    } finally {
      setLoading(false);
    }
  }, [search, availabilityFilter]);

  useEffect(() => {
    const debounce = setTimeout(fetchBooks, 300);
    return () => clearTimeout(debounce);
  }, [fetchBooks]);

  const handleAddBook = async (request: CreateBookRequest) => {
    await libraryApi.createBook(request);
    await fetchBooks();
  };

  const handleToggleAvailability = async (id: number) => {
    await libraryApi.toggleAvailability(id);
    await fetchBooks();
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Delete this book?')) return;
    await libraryApi.deleteBook(id);
    await fetchBooks();
  };

  const totalPages = Math.ceil(books.length / PAGE_SIZE);
  const paginatedBooks = books.slice((currentPage - 1) * PAGE_SIZE, currentPage * PAGE_SIZE);

  return (
    <div style={{ maxWidth: '900px', margin: '0 auto', padding: '32px 20px', fontFamily: 'system-ui, sans-serif' }}>
      <h1 style={{ fontSize: '28px', fontWeight: 700, marginBottom: '4px' }}>Library</h1>
      <p style={{ color: '#6c757d', marginBottom: '24px' }}>Crumb-to-Crumb Book Lending</p>

      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '16px' }}>
        <SearchBar
          search={search}
          availabilityFilter={availabilityFilter}
          onSearchChange={setSearch}
          onAvailabilityChange={setAvailabilityFilter}
        />
        <span style={{ fontSize: '13px', color: '#888' }}>
          {books.length} book{books.length !== 1 ? 's' : ''}
        </span>
      </div>

      {loading && <p style={{ textAlign: 'center', color: '#888' }}>Loading...</p>}
      {error && <p style={{ color: 'red', textAlign: 'center' }}>{error}</p>}

      {!loading && !error && (
        <>
          <div style={{ border: '1px solid #dee2e6', borderRadius: '6px', overflow: 'hidden' }}>
            <BookTable
              books={paginatedBooks}
              onToggleAvailability={handleToggleAvailability}
              onDelete={handleDelete}
              onRowClick={setSelectedBook}
            />
          </div>

          <div style={{ marginTop: '16px' }}>
            <Pagination
              currentPage={currentPage}
              totalPages={totalPages}
              onPageChange={setCurrentPage}
            />
          </div>
        </>
      )}

      {/* Floating Add Book button — anchored bottom-right */}
      <button
        onClick={() => setShowAddModal(true)}
        style={{
          position: 'fixed', bottom: '32px', right: '32px', padding: '14px 24px',
          fontSize: '15px', fontWeight: 600, background: '#007bff', color: 'white',
          border: 'none', borderRadius: '28px', cursor: 'pointer',
          boxShadow: '0 4px 12px rgba(0,123,255,0.4)',
        }}
      >
        + Add Book
      </button>

      {showAddModal && (
        <AddBookModal onClose={() => setShowAddModal(false)} onSubmit={handleAddBook} />
      )}

      {selectedBook && (
        <BookDetailModal
          book={selectedBook}
          onClose={() => setSelectedBook(null)}
          onToggleAvailability={handleToggleAvailability}
          onDelete={handleDelete}
        />
      )}
    </div>
  );
}

export default App;
