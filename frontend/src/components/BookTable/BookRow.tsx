import { Book } from '../../types/book';

interface Props {
  book: Book;
  onToggleAvailability: (id: number) => void;
  onDelete: (id: number) => void;
  onClick: (book: Book) => void;
}

export function BookRow({ book, onToggleAvailability, onDelete, onClick }: Props) {
  return (
    <tr
      onClick={() => onClick(book)}
      style={{ cursor: 'pointer', backgroundColor: 'white' }}
      onMouseEnter={(e) => (e.currentTarget.style.backgroundColor = '#f5f5f5')}
      onMouseLeave={(e) => (e.currentTarget.style.backgroundColor = 'white')}
    >
      <td style={tdStyle}>{book.title}</td>
      <td style={tdStyle}>{book.owner}</td>
      <td style={tdStyle}>
        <span
          style={{
            padding: '2px 10px',
            borderRadius: '12px',
            fontSize: '12px',
            fontWeight: 600,
            backgroundColor: book.isAvailable ? '#d4edda' : '#f8d7da',
            color: book.isAvailable ? '#155724' : '#721c24',
          }}
        >
          {book.isAvailable ? 'Available' : 'Unavailable'}
        </span>
      </td>
      <td style={{ ...tdStyle, width: '120px' }}>
        <button
          onClick={(e) => { e.stopPropagation(); onToggleAvailability(book.id); }}
          title={book.isAvailable ? 'Mark as borrowed' : 'Mark as returned'}
          style={actionButtonStyle}
        >
          {book.isAvailable ? 'Borrow' : 'Return'}
        </button>
        <button
          onClick={(e) => { e.stopPropagation(); onDelete(book.id); }}
          title="Delete book"
          style={{ ...actionButtonStyle, color: '#dc3545', marginLeft: '6px' }}
        >
          Delete
        </button>
      </td>
    </tr>
  );
}

const tdStyle: React.CSSProperties = {
  padding: '10px 14px',
  borderBottom: '1px solid #dee2e6',
  fontSize: '14px',
};

const actionButtonStyle: React.CSSProperties = {
  padding: '3px 8px',
  fontSize: '12px',
  cursor: 'pointer',
  border: '1px solid #ccc',
  borderRadius: '4px',
  background: 'white',
};
