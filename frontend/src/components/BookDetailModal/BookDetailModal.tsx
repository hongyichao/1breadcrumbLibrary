import { Book } from '../../types/book';

interface Props {
  book: Book;
  onClose: () => void;
  onToggleAvailability: (id: number) => void;
  onDelete: (id: number) => void;
}

export function BookDetailModal({ book, onClose, onToggleAvailability, onDelete }: Props) {
  const handleDelete = () => {
    onDelete(book.id);
    onClose();
  };

  const handleToggle = () => {
    onToggleAvailability(book.id);
    onClose();
  };

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
          <h2 style={{ margin: 0, fontSize: '20px' }}>{book.title}</h2>
          <button onClick={onClose} style={closeButtonStyle}>✕</button>
        </div>

        <div style={{ marginTop: '20px', display: 'flex', flexDirection: 'column', gap: '10px' }}>
          <Row label="Author" value={book.author} />
          <Row label="ISBN" value={book.isbn || '—'} />
          <Row label="Published" value={book.publishedDate} />
          <Row label="Owner" value={book.owner} />
          <Row
            label="Status"
            value={
              <span style={{
                padding: '2px 10px', borderRadius: '12px', fontSize: '12px', fontWeight: 600,
                backgroundColor: book.isAvailable ? '#d4edda' : '#f8d7da',
                color: book.isAvailable ? '#155724' : '#721c24',
              }}>
                {book.isAvailable ? 'Available' : 'Unavailable'}
              </span>
            }
          />
        </div>

        <div style={{ display: 'flex', gap: '10px', marginTop: '24px' }}>
          <button onClick={handleToggle} style={primaryButtonStyle}>
            {book.isAvailable ? 'Borrow this book' : 'Return this book'}
          </button>
          <button onClick={handleDelete} style={dangerButtonStyle}>Delete</button>
          <button onClick={onClose} style={cancelButtonStyle}>Close</button>
        </div>
      </div>
    </div>
  );
}

function Row({ label, value }: { label: string; value: React.ReactNode }) {
  return (
    <div style={{ display: 'flex', gap: '8px' }}>
      <span style={{ fontWeight: 600, fontSize: '14px', minWidth: '90px', color: '#495057' }}>{label}:</span>
      <span style={{ fontSize: '14px' }}>{value}</span>
    </div>
  );
}

const overlayStyle: React.CSSProperties = {
  position: 'fixed', inset: 0, backgroundColor: 'rgba(0,0,0,0.4)', display: 'flex',
  alignItems: 'center', justifyContent: 'center', zIndex: 1000,
};
const modalStyle: React.CSSProperties = {
  background: 'white', borderRadius: '8px', padding: '28px', width: '440px',
  maxWidth: '90vw', boxShadow: '0 4px 24px rgba(0,0,0,0.15)',
};
const closeButtonStyle: React.CSSProperties = {
  background: 'none', border: 'none', fontSize: '18px', cursor: 'pointer', color: '#888',
};
const primaryButtonStyle: React.CSSProperties = {
  padding: '8px 16px', fontSize: '14px', cursor: 'pointer', border: 'none',
  borderRadius: '4px', background: '#007bff', color: 'white',
};
const dangerButtonStyle: React.CSSProperties = {
  padding: '8px 16px', fontSize: '14px', cursor: 'pointer', border: 'none',
  borderRadius: '4px', background: '#dc3545', color: 'white',
};
const cancelButtonStyle: React.CSSProperties = {
  padding: '8px 16px', fontSize: '14px', cursor: 'pointer', border: '1px solid #ccc',
  borderRadius: '4px', background: 'white',
};
