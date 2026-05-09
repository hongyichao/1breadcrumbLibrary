interface Props {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}

export function Pagination({ currentPage, totalPages, onPageChange }: Props) {
  if (totalPages <= 1) return null;

  return (
    <div style={{ display: 'flex', gap: '8px', alignItems: 'center', justifyContent: 'center' }}>
      <button
        onClick={() => onPageChange(currentPage - 1)}
        disabled={currentPage === 1}
        style={{ padding: '6px 12px', cursor: currentPage === 1 ? 'default' : 'pointer' }}
      >
        &larr;
      </button>
      <span style={{ fontSize: '14px' }}>
        {currentPage} of {totalPages}
      </span>
      <button
        onClick={() => onPageChange(currentPage + 1)}
        disabled={currentPage === totalPages}
        style={{ padding: '6px 12px', cursor: currentPage === totalPages ? 'default' : 'pointer' }}
      >
        &rarr;
      </button>
    </div>
  );
}
