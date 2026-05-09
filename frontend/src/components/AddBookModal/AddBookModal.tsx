import React, { useState } from 'react';
import { CreateBookRequest } from '../../types/book';

interface Props {
  onClose: () => void;
  onSubmit: (book: CreateBookRequest) => Promise<void>;
}

const emptyForm: CreateBookRequest = {
  title: '',
  author: '',
  isbn: '',
  publishedDate: '',
  owner: '',
};

export function AddBookModal({ onClose, onSubmit }: Props) {
  const [form, setForm] = useState<CreateBookRequest>(emptyForm);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSubmitting(true);
    try {
      await onSubmit(form);
      onClose();
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : 'Failed to add book');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <h2 style={{ marginTop: 0 }}>Add Book</h2>
        {error && <p style={{ color: 'red', fontSize: '14px' }}>{error}</p>}
        <form onSubmit={handleSubmit}>
          {([
            { name: 'title', label: 'Title', type: 'text' },
            { name: 'author', label: 'Author', type: 'text' },
            { name: 'isbn', label: 'ISBN', type: 'text' },
            { name: 'publishedDate', label: 'Published Date', type: 'date' },
            { name: 'owner', label: 'Owner (your name)', type: 'text' },
          ] as { name: keyof CreateBookRequest; label: string; type: string }[]).map(({ name, label, type }) => (
            <div key={name} style={{ marginBottom: '14px' }}>
              <label style={{ display: 'block', fontSize: '13px', marginBottom: '4px', fontWeight: 600 }}>
                {label}
              </label>
              <input
                name={name}
                type={type}
                value={form[name]}
                onChange={handleChange}
                required={name !== 'isbn'}
                style={inputStyle}
              />
            </div>
          ))}
          <div style={{ display: 'flex', gap: '10px', justifyContent: 'flex-end', marginTop: '20px' }}>
            <button type="button" onClick={onClose} style={cancelButtonStyle}>Cancel</button>
            <button type="submit" disabled={submitting} style={submitButtonStyle}>
              {submitting ? 'Adding...' : 'Add Book'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

const overlayStyle: React.CSSProperties = {
  position: 'fixed', inset: 0, backgroundColor: 'rgba(0,0,0,0.4)', display: 'flex',
  alignItems: 'center', justifyContent: 'center', zIndex: 1000,
};
const modalStyle: React.CSSProperties = {
  background: 'white', borderRadius: '8px', padding: '28px', width: '400px',
  maxWidth: '90vw', boxShadow: '0 4px 24px rgba(0,0,0,0.15)',
};
const inputStyle: React.CSSProperties = {
  width: '100%', padding: '8px 10px', fontSize: '14px', border: '1px solid #ccc',
  borderRadius: '4px', boxSizing: 'border-box',
};
const cancelButtonStyle: React.CSSProperties = {
  padding: '8px 18px', fontSize: '14px', cursor: 'pointer', border: '1px solid #ccc',
  borderRadius: '4px', background: 'white',
};
const submitButtonStyle: React.CSSProperties = {
  padding: '8px 18px', fontSize: '14px', cursor: 'pointer', border: 'none',
  borderRadius: '4px', background: '#007bff', color: 'white',
};
