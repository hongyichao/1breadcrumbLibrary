import { Book } from '../../types/book';
import { BookRow } from './BookRow';

interface Props {
  books: Book[];
  onToggleAvailability: (id: number) => void;
  onDelete: (id: number) => void;
  onRowClick: (book: Book) => void;
}

export function BookTable({ books, onToggleAvailability, onDelete, onRowClick }: Props) {
  if (books.length === 0) {
    return (
      <p style={{ textAlign: 'center', color: '#888', padding: '40px 0' }}>
        No books found.
      </p>
    );
  }

  return (
    <table style={{ width: '100%', borderCollapse: 'collapse' }}>
      <thead>
        <tr style={{ backgroundColor: '#f8f9fa' }}>
          <th style={thStyle}>Book</th>
          <th style={thStyle}>Owner</th>
          <th style={thStyle}>Availability</th>
          <th style={thStyle}>Actions</th>
        </tr>
      </thead>
      <tbody>
        {books.map((book) => (
          <BookRow
            key={book.id}
            book={book}
            onToggleAvailability={onToggleAvailability}
            onDelete={onDelete}
            onClick={onRowClick}
          />
        ))}
      </tbody>
    </table>
  );
}

const thStyle: React.CSSProperties = {
  padding: '10px 14px',
  textAlign: 'left',
  fontWeight: 600,
  fontSize: '13px',
  color: '#495057',
  borderBottom: '2px solid #dee2e6',
};
